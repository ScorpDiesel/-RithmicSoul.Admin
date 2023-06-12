using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Forms;

public partial class SurveyForm : ComponentBase
{
    [Inject] NavigationManager Navigation { get; set; }
    [Inject] IService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IDraftSurveyService DraftSurveyService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public int? Id { get; set; }

    private static IEnumerable<SurveyTypeDto> _surveyTypes;
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private DraftSurveyDto? Model = new();
    private AppSettings _appSettings;
    private string _scrollToBottom;

    protected override async Task OnInitializedAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _surveyTypes = await SurveyTypeService.GetAllAsync();
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();

        if (Id is not null) Model = await DraftSurveyService.GetByIdAsync((int)Id);
    }


    private MudBlazor.Converter<int, string> ConvertIdToName = new()
    {
        GetFunc = str => int.TryParse(str, out var id) ? id : 0,
        SetFunc = id => _surveyTypes.FirstOrDefault(qt => qt.SurveyTypeId == id)?.SurveyTypeName ?? "Select..."
    };

    private string? GetSelectedQuestionTypeName(int id)
    {
        return _surveyQuestions.FirstOrDefault(s => s.QuestionId == id)?.QuestionTypeName;
    }

    private void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar.Clear();
        Snackbar.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    private void AddQuestion()
    {
        if (Model?.SurveyQuestionIds.Count < _appSettings.SurveyQuestionsMaxCount)
        {
            Model.SurveyQuestionIds.Add(new int());
            _scrollToBottom = Model?.SurveyQuestionIds.Count > 4 ? "display: flex;flex-direction: column-reverse;" : "";
        }
    }

    private void RemoveQuestion(int index)
    {
        if (Model?.SurveyQuestionIds.Count > _appSettings.SurveyQuestionsMinCount)
        {
            Model.SurveyQuestionIds.RemoveAt(index);
            _scrollToBottom = Model?.SurveyQuestionIds.Count > 4 ? "display: flex;flex-direction: column-reverse;" : "";
        }
    }


    private void Cancel() => Navigation.NavigateTo("/surveys");

    private async Task Save()
    {
        var response = await DraftSurveyService.SaveAsync(Model);
        var message = response ? _appSettings.AuthoredSurveyCreationSuccessMessage : _appSettings.AuthoredSurveyCreationFailureMessage;
        ShowSnackBar(response, message);
    }
}