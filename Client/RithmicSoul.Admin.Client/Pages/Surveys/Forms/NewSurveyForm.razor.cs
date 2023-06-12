using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Forms;

public partial class NewSurveyForm : ComponentBase
{
    protected IEnumerable<SurveyTypeDto> _surveyTypes;
    protected IEnumerable<SurveyQuestionDto> _surveyQuestions;
    protected IEnumerable<QuestionTypeDto> _questionTypes;
    protected IEnumerable<QuestionChoiceDto> _questionChoices;
    protected DraftSurvey Model = new();
    protected string? _selectedQuestionTypeName;
    private AppSettings _appSettings;

    [Inject] NavigationManager Navigation { get; set; }
    [Inject] IService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IService<QuestionTypeDto> QuestionTypeService { get; set; }
    [Inject] IService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _surveyTypes = await SurveyTypeService.GetAllAsync();
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _questionTypes = await QuestionTypeService.GetAllAsync();
        _questionChoices = await QuestionChoiceService.GetAllAsync();
    }

    void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar.Clear();
        Snackbar.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    private void AddQuestion()
    {
        if (Model.SurveyQuestions.Count < _appSettings.SurveyQuestionsMaxCount) Model.SurveyQuestions.Add(new int());
    }

    private void QuestionChanged(int id)
    {
        if (_surveyQuestions is null) return;
        var dto = _surveyQuestions.FirstOrDefault(q => q.QuestionId == id);
        _selectedQuestionTypeName = dto?.QuestionTypeName;
    }

    private void RemoveQuestion(int index)
    {
        if (Model.SurveyQuestions.Count > _appSettings.SurveyQuestionsMinCount) Model.SurveyQuestions.RemoveAt(index);
    }


    protected void Cancel() => Navigation.NavigateTo("/surveys");

    protected async Task Save()
    {
        //var response = await AuthoredSurveyService.SaveAsync(Model);
        //var message = response ? _appSettings.AuthoredSurveyCreationSuccessMessage : _appSettings.AuthoredSurveyCreationFailureMessage;
        //ShowSnackBar(response, message);
    }
}