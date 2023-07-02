using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Views.Dialogs;

public partial class NewQuestionChoiceDialog : ComponentBase
{
    [Inject] private IBulkActionsService<QuestionChoiceDto> BulkActionsService { get; set; }
    [Inject] private IService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] private IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] private IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    public string QuestionChoiceTableName;
    private string _questionTypeName;
    private string _questionText;
    private List<string> _chooseableList = new();
    private string _questionTypeNameVisibility;
    private string _choiceControlsVisibility;
    private AppSettings _appSettings;

    protected SurveyQuestionChoices Model = new();
    protected IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private IEnumerable<QuestionChoiceDto> _questionChoices;
    private string _statusMessage;
    private Color _statusMessageColor;

    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        QuestionChoiceTableName = nameof(QuestionChoiceDto).Replace("Dto", "");
        _questionTypeNameVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _choiceControlsVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _chooseableList.Add(_appSettings.QuestionChoicesMultipleChoice);
        _chooseableList.Add(_appSettings.QuestionChoicesSingleChoice);
        _questionChoices = await QuestionChoiceService.GetAllAsync();
        var items = await SurveyQuestionService.GetAllAsync();
        _surveyQuestions = items
            .Where(item1 => _chooseableList.Contains(item1.QuestionTypeName) /*&& _questionChoices.Any(item2 => item2.QuestionId == item1.QuestionId)*/
            ).ToList();
    }

    private void SetStatusMessage()
    {
        if(_surveyQuestions is null) return;
        if (!_surveyQuestions.Any())
        {
            _statusMessage = "Create a question in the SurveyQuestion table first";
            _statusMessageColor = Color.Error;
        }
        else
        {
            _statusMessage = "Only questions that allow user-defined choices are displayed";
            _statusMessageColor = Color.Info;
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

    private void Save()
    {
        var modelCollection = CreateListFromObject(Model);
        MudDialog.Close(DialogResult.Ok(modelCollection));
    }

    private List<QuestionChoiceDto> CreateListFromObject(SurveyQuestionChoices surveyQuestionChoices)
    {
        List<QuestionChoiceDto> choices = new();
        for (var c = 0; c < surveyQuestionChoices.QuestionChoices.Count; c++)
        {
            var choice = surveyQuestionChoices.QuestionChoices[c];
            var example = surveyQuestionChoices.QuestionExamples[c];
            var questionChoice = new QuestionChoiceDto();
            questionChoice.QuestionId = (int)surveyQuestionChoices.QuestionId;
            questionChoice.ChoiceId = surveyQuestionChoices.ChoiceId;
            questionChoice.ChoiceText = choice.Replace("'", "''");
            questionChoice.ChoiceExample = example.Replace("'", "''");
            choices.Add(questionChoice);
        }

        return choices;
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