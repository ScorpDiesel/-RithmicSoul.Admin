using Azure;
using Microsoft.AspNetCore.Components;
using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class ProductSurvey : ComponentBase
{
    [Inject] IService<AuthoredSurveyDto> AuthoredSurveyService { get; set; }
    [CascadingParameter] public EventCallback HideMenus { get; set; }
    
    private string _surveyDescription;
    private IEnumerable<AuthoredSurveyDto> _authoredSurveys;
    private IEnumerable<string> _questions;
    private int _pageSize = 1;
    private int _currentPage;
    private Dictionary<string, (string questionType, List<object> responses)> _questionResponses = new();

    private string CurrentQuestion => _questions is null ? null : _questions.Skip(_currentPage * _pageSize).Take(_pageSize).First();


    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await HideMenus.InvokeAsync();
        _authoredSurveys = await AuthoredSurveyService.GetAllAsync();
        _surveyDescription = _authoredSurveys.First().SurveyDescription;
        _questions = _authoredSurveys.Select(q => q.QuestionText).Distinct();
    }

    private bool HasPreviousPage => _currentPage > 0;
    private bool HasNextPage => _questions is null ? false : (_currentPage + 1) * _pageSize < _questions.Count();

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
        if (response.QuestionType == "Checkbox")
        {
            if (response.isSelected)
            {
                List<object> existingResponses;
                if (_questionResponses.ContainsKey(response.QuestionText))
                {
                    existingResponses = _questionResponses[response.QuestionText].responses;
                    existingResponses.Add(response.Response);
                    _questionResponses[response.QuestionText] = (response.QuestionType, existingResponses);
                }
                else
                {
                    existingResponses = new() { response.Response };
                    _questionResponses.Add(response.QuestionText, (response.QuestionType, existingResponses));
                }
            }
            else
            {
                var existingResponses = _questionResponses[response.QuestionText].responses;
                existingResponses.Remove(response.Response);
                if (!existingResponses.Any())
                {
                    _questionResponses.Remove(response.QuestionText);
                }
                else
                {
                    _questionResponses[response.QuestionText] = (response.QuestionType, existingResponses);
                }
            }
        }
        else
        {
            List<object> responses = new() { response.Response };
            _questionResponses[response.QuestionText] = (response.QuestionType, responses);
        }
    }
}