using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Utilities;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Admin.Infrastructure.Services;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulDatabaseLibrary.Utilities;

namespace RithmicSoul.Admin.Client.Views.Dialogs;

public partial class NewQuestionChoiceDialog : ComponentBase
{
    [Inject] IService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Inject] private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    public string QuestionChoiceTableName = EntityUtility.GetTableName<QuestionChoice>();
    private string? _questionTypeName;
    private string? _questionText;
    private List<string> _chooseableList = new();
    private string _questionTypeNameVisibility;
    private string _choiceControlsVisibility;
    private AppSettings _appSettings;

    protected SurveyQuestionChoices Model = new();
    protected IEnumerable<SurveyQuestionDto> _surveyQuestions;

    protected override async Task OnInitializedAsync()
    {
        await SetFields();
    }

    private async Task SetFields()
    {
        _appSettings = AppSettingsOptions.Value;
        _questionTypeNameVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _choiceControlsVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _chooseableList.Add(_appSettings.QuestionChoicesMultipleChoice);
        _chooseableList.Add(_appSettings.QuestionChoicesCheckbox);
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _surveyQuestions = _surveyQuestions.Where(s => _chooseableList.Contains(s.QuestionTypeName));
    }

    private void AddQuestionChoice()
    {
        if (Model.QuestionChoices.Count <= _appSettings.SurveyQuestionsMaxCount) Model.QuestionChoices.Add(string.Empty);
    }


    private void RemoveQuestionChoice(int index)
    {
        if (Model.QuestionChoices.Count > _appSettings.SurveyQuestionsMinCount) Model.QuestionChoices.RemoveAt(index);
    }

    protected void QuestionTypeChangeEvent(string value)
    {
        if (_surveyQuestions is null) return;
        Model.QuestionId = Convert.ToInt32(value);
        var dto = _surveyQuestions.FirstOrDefault(q => q.QuestionId == Model.QuestionId);
        _questionTypeName = dto?.QuestionTypeName;
        _questionText = dto?.QuestionText;
        _questionTypeNameVisibility = _appSettings.QuestionTypeNameCssVisibilityVisible;
        _choiceControlsVisibility = _chooseableList.Contains(_questionTypeName) ? _appSettings.QuestionTypeNameCssVisibilityVisible 
            : _appSettings.QuestionTypeNameCssVisibilityHidden;
    }

    private async Task SaveAsync()
    {
        var modelCollection = ModelUtilities.MapToModelWithCollection<QuestionChoiceDto, SurveyQuestionChoices>(Model);
        var isBulkInsertSuccessful = await QuestionChoiceService.BulkInsertAsync(modelCollection.ToList());
        ShowSnackBar(isBulkInsertSuccessful);
        MudDialog.Close(DialogResult.Ok(modelCollection));
    }

    private void Cancel() => MudDialog.Cancel();

    private void ShowSnackBar(bool isSuccessful)
    {
        Snackbar?.Clear();
        string message;
        if (isSuccessful)
        {
            message = string.Format(_appSettings.ItemsCreatedSuccessMessageTemplate, QuestionChoiceTableName);
            Snackbar?.Add(message, Severity.Success);
        }
        else
        {
            message = string.Format(_appSettings.ItemsCreatedFailureMessageTemplate, QuestionChoiceTableName);
            Snackbar?.Add(message, Severity.Error);
        }
    }
}