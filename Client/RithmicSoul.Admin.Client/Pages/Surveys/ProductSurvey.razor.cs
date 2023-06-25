using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Application.Interfaces.Services.Survey;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.ValueObjects;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class ProductSurvey : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IDatabaseService<AuthoredSurveyDto> AuthoredSurveyService { get; set; }
    [Inject] private IService<SurveyResponseDto> SurveyResponseService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [CascadingParameter] public EventCallback<bool> HideMenus { get; set; }
    [Parameter] public Guid Id { get; set; }

    private string _surveyDescription;
    private IEnumerable<AuthoredSurveyDto> _authoredSurveys;
    private SurveyResponseDto _surveyResponse;
    private IEnumerable<string> _questions;
    private int _pageSize = 1;
    private int _currentPage;
    private bool _endOfSurveyReached;
    private Dictionary<int, List<string>> _questionResponses = new();

    private string CurrentQuestion => _questions?.Skip(_currentPage * _pageSize).Take(_pageSize).First();


    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await HideMenus.InvokeAsync(true);
        var uriList = Navigation.Uri.Split("/");
        var surveyNameRoute = uriList[^2];
        var items = await SurveyResponseService.GetByIdAsync(Id);
        _surveyResponse = items.First();
        _authoredSurveys = await AuthoredSurveyService.GetFromViewAsync(v => v.ActiveSurveyName == surveyNameRoute);
        _surveyDescription = _authoredSurveys.First().SurveyDescription;
        _questions = _authoredSurveys.Select(q => q.QuestionText).Distinct();
    }

    private bool HasPreviousPage => _currentPage > 0;
    private bool HasNextPage
    {
        get
        {
            var hasNextPag = _questions is not null && (_currentPage + 1) * _pageSize < _questions.Count();
            _endOfSurveyReached = !hasNextPag;
            return hasNextPag;
        }
    }

    private void PreviousPage()
    {
        if (!HasPreviousPage) return;
        _currentPage--;
    }

    private void NextPage()
    {
        if (!HasNextPage) return;
        _currentPage++;
    }

    private void QuestionValueChanged(QuestionResponseObject response)
    {
        if (response.QuestionType == "Multiple Choice")
        {
            if (response.IsSelected)
            {
                List<string> existingResponses;
                if (_questionResponses.ContainsKey(response.QuestionId))
                {
                    existingResponses = _questionResponses[response.QuestionId];
                    existingResponses.AddRange(response.Responses);
                    _questionResponses[response.QuestionId] = existingResponses;
                }
                else
                {
                    existingResponses = response.Responses;
                    _questionResponses.Add(response.QuestionId, existingResponses);
                }
            }
            else
            {
                var existingResponses = _questionResponses[response.QuestionId];
                if (existingResponses.Any())
                {
                    _questionResponses.Values.First(v => v.Remove(response.Responses.First()));
                }
            }
        }
        else
        {
            List<string> responses = response.Responses;
            _questionResponses[response.QuestionId] = responses;
        }
    }

    private async Task SaveSurveyAsync()
    {
        var options = new DialogOptions { CloseButton = true };
        var contentText = _endOfSurveyReached
            ? "Save the survey?"
            : "Looks like you haven't completed the survey.\r\nSave anyway?";
        var parameters = new DialogParameters
        {
            { "ContentText", contentText },
            { "CloseButtonText", "Yes" },
            { "CancelButtonText", "No" },
            { "Style", "min-width:300px" },
            { "Color", Color.Success }
        };
        var dialog = await DialogService?.ShowAsync<ActionDialog>("Save", parameters, options)!;
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            if (!_questionResponses.Any()) return;
            var answers = _questionResponses.Select(q => new QuestionAnswers(QuestionId: q.Key, Answers: q.Value)).ToList();
            _surveyResponse.QuestionAnswers = answers;
            _surveyResponse.DateCreated = DateTime.UtcNow;
            var isSuccess = await SurveyResponseService.UpdateAsync(_surveyResponse);
        }
    }
}