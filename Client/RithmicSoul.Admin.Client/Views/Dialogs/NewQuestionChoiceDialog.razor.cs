using Google.Protobuf;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Admin.Core.Utilities;
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
    private IEnumerable<QuestionChoiceDto> _questionChoices;
    private string _message;
    private Color _messageColor;

    protected override async Task OnInitializedAsync()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        _message = "Only questions that allow user-defined choices are displayed";
        _messageColor = Color.Info;
        _appSettings = AppSettingsOptions.Value;
        _questionTypeNameVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _choiceControlsVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _chooseableList.Add(_appSettings.QuestionChoicesMultipleChoice);
        _chooseableList.Add(_appSettings.QuestionChoicesSingleChoice);
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _questionChoices = await QuestionChoiceService.GetAllAsync();
        _surveyQuestions = _surveyQuestions
            .Where(item1 => _chooseableList.Contains(item1.QuestionTypeName) 
                && _questionChoices.Any(item2 => item2.QuestionId == item1.QuestionId 
                && item2.ChoiceText is null)
            ).ToList();
        if (!_surveyQuestions.Any())
        {
            _message = "Create a question in the SurveyQuestion table first";
            _messageColor = Color.Error;
        }

    }

    private void AddQuestionChoice()
    {
        if (Model.QuestionChoices.Count <= _appSettings.SurveyQuestionsMaxCount)
        {
            Model.QuestionChoices.Add(string.Empty);
            Model.QuestionExamples.Add(string.Empty);
        }
    }


    private void RemoveQuestionChoice(int index)
    {
        if (Model.QuestionChoices.Count > _appSettings.SurveyQuestionsMinCount)
        {
            Model.QuestionChoices.RemoveAt(index);
            Model.QuestionExamples.RemoveAt(index);
        }
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
        var modelCollection = Utilities.MapToModelWithCollection<QuestionChoiceDto, SurveyQuestionChoices>(Model);
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