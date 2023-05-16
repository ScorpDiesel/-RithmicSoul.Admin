using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Utilities;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Views.Dialogs;

public partial class NewQuestionChoiceDialog : ComponentBase
{
    [Inject] IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private string? _questionTypeName;
    private string? _questionText;
    private List<string> _chooseableList = new();
    private string _questionTypeNameVisibility;
    private string _choiceControlsVisibility;
    private AppSettings _appSettings;

    //public int? SelectedSurveyQuestionId
    //{
    //    get => _model.QuestionId == 0 ? null : _model.QuestionId;
    //    set => _model.QuestionId = value ?? 0;
    //}

    protected SurveyQuestionChoices Model = new();
    //private QuestionChoiceDto _model = new();
    //private List<QuestionChoiceDto> _dtoList = new();
    protected IEnumerable<SurveyQuestionDto> _surveyQuestions;

    protected override async Task OnInitializedAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _questionTypeNameVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _choiceControlsVisibility = _appSettings.QuestionTypeNameCssVisibilityHidden;
        _chooseableList.Add(_appSettings.QuestionChoicesChooseableMultipleChoice);
        _chooseableList.Add(_appSettings.QuestionChoicesDeleteCheckbox);
        _chooseableList.Add(_appSettings.QuestionChoicesChooseableDemographic);
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

    private void Save()
    {
        var modelCollection = ModelUtilities.MapToModelWithCollection<QuestionChoiceDto, SurveyQuestionChoices>(Model);
        MudDialog.Close(DialogResult.Ok(modelCollection));
    }

    private void Cancel() => MudDialog.Cancel();
}