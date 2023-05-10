using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Dialogs;

public partial class NewQuestionChoiceDialog : ComponentBase
{
    [Inject] 
    IService<SurveyQuestionDto> SurveyQuestionService { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }
    private string? _questionTypeName;
    private string? _questionText;
    private List<string> _choosableList = new();
    private string _questionTypeNameVisibility = "visibility: hidden;";
    private string _choiceControlsVisibility = "visibility: hidden;";

    public int? SelectedSurveyQuestionId
    {
        get => _dto.QuestionId == 0 ? null : _dto.QuestionId;
        set => _dto.QuestionId = value ?? 0;
    }

    private QuestionChoiceDto _dto = new();
    private List<QuestionChoiceDto> _dtoList = new();
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;

    protected override async Task OnInitializedAsync()
    {
        _choosableList.Add("Multiple Choice");
        _choosableList.Add("Checkbox");
        _choosableList.Add("Demographic");
        _dtoList.Add(_dto);
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _surveyQuestions = _surveyQuestions.Where(s => _choosableList.Contains(s.QuestionTypeName));
    }

    void AddQuestionChoice()
    {
        if (_dtoList.Count < 10) _dtoList.Add(new QuestionChoiceDto());
    }


    void RemoveQuestionChoice(int index)
    {
        if (_dtoList.Count > 1) _dtoList.RemoveAt(index);
    }

    protected void QuestionTypeChangeEvent(string value)
    {
        SelectedSurveyQuestionId = Convert.ToInt32(value);
        var dto = _surveyQuestions.FirstOrDefault(q => q.QuestionId == SelectedSurveyQuestionId);
        _questionTypeName = dto?.QuestionTypeName;
        _questionText = dto?.QuestionText;
        _questionTypeNameVisibility = "visibility: visible;";
        _choiceControlsVisibility = _choosableList.Contains(_questionTypeName) ? "visibility: visible;" : "visibility: hidden;";
    }

    void Save()
    {
        foreach (var choice in _dtoList)
        {
            choice.QuestionId = _dto.QuestionId;
        }
        
        MudDialog.Close(DialogResult.Ok(_dtoList));
    }

    void Cancel() => MudDialog.Cancel();
}