using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Client.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using System;
using RithmicSoul.Admin.Client.Utilities;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Dialogs;

public partial class NewQuestionChoiceDialog : ComponentBase
{
    [Inject] 
    IService<SurveyQuestionDto> SurveyQuestionService { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private string? _questionTypeName;
    private string? _questionText;
    private List<string> _choosableList = new();
    private string _questionTypeNameVisibility = "visibility: hidden;";
    private string _choiceControlsVisibility = "visibility: hidden;";

    //public int? SelectedSurveyQuestionId
    //{
    //    get => _model.QuestionId == 0 ? null : _model.QuestionId;
    //    set => _model.QuestionId = value ?? 0;
    //}

    private SurveyQuestionChoices _model = new();
    //private QuestionChoiceDto _model = new();
    //private List<QuestionChoiceDto> _dtoList = new();
    protected IEnumerable<SurveyQuestionDto> _surveyQuestions;

    protected override async Task OnInitializedAsync()
    {
        _choosableList.Add("Multiple Choice");
        _choosableList.Add("Checkbox");
        _choosableList.Add("Demographic");
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _surveyQuestions = _surveyQuestions.Where(s => _choosableList.Contains(s.QuestionTypeName));
    }

    private void AddQuestionChoice()
    {
        if (_model.QuestionChoices.Count < 10) _model.QuestionChoices.Add(string.Empty);
    }


    private void RemoveQuestionChoice(int index)
    {
        if (_model.QuestionChoices.Count > 1) _model.QuestionChoices.RemoveAt(index);
    }

    protected void QuestionTypeChangeEvent(string value)
    {
        if (_surveyQuestions is null) return;
        _model.QuestionId = Convert.ToInt32(value);
        var dto = _surveyQuestions.FirstOrDefault(q => q.QuestionId == _model.QuestionId);
        _questionTypeName = dto?.QuestionTypeName;
        _questionText = dto?.QuestionText;
        _questionTypeNameVisibility = "visibility: visible;";
        _choiceControlsVisibility = _choosableList.Contains(_questionTypeName) ? "visibility: visible;" : "visibility: hidden;";
    }

    private void Save()
    {
        var modelCollection = ModelUtilities.MapToModelWithCollection<QuestionChoiceDto, SurveyQuestionChoices>(_model);
        MudDialog.Close(DialogResult.Ok(modelCollection));
    }

    private void Cancel() => MudDialog.Cancel();
}