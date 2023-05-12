using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Dialogs;

public partial class EditQuestionChoiceDialog : ComponentBase
{
    [Inject] 
    private IService<QuestionChoiceDto>? QuestionChoiceService { get; set; }

    [Inject] 
    private IService<SurveyQuestionDto>? SurveyQuestionService { get; set; }

    [Inject] 
    private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] 
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public QuestionChoiceDto Model { get; set; }

    private string? _questionTypeName;
    private readonly List<string> _chooseableList = new();

    private readonly List<QuestionChoiceDto> _dtoList = new();
    private readonly List<QuestionChoiceDto> _oldDtoList = new();
    private IEnumerable<SurveyQuestionDto>? _surveyQuestions;
    private readonly DialogOptions _options = new();
    private bool _isLoading;
    private bool _isVisible;

    protected override async Task OnInitializedAsync()
    {
        ToggleLoadingScreen(true);

        _chooseableList.Add("Multiple Choice");
        _chooseableList.Add("Checkbox");
        _chooseableList.Add("Demographic");
        var dtos = await QuestionChoiceService?.GetAsync(q => q.QuestionId == Model.QuestionId);
        _dtoList?.AddRange(dtos.ToList());
        _oldDtoList?.AddRange(dtos.ToList());
        _surveyQuestions = await SurveyQuestionService?.GetAllAsync();
        _surveyQuestions = _surveyQuestions.Where(s => _chooseableList.Contains(s.QuestionTypeName));
        _questionTypeName = _surveyQuestions?.FirstOrDefault()?.QuestionTypeName;

        ToggleLoadingScreen(false);
    }

    private void ToggleLoadingScreen(bool hide)
    {
        if (hide)
        {
            _isLoading = true;
            _isVisible = true;
        }
        else
        {
            _isLoading = false;
            _isVisible = false;
        }

        _options.NoHeader = hide;
        MudDialog?.SetOptions(_options);
    }

    private void AddQuestionChoice()
    {
        if (_dtoList.Count < 10) _dtoList?.Add(new QuestionChoiceDto());
    }


    private void RemoveQuestionChoice(int index)
    {
        if (_dtoList.Count > 1) _dtoList?.RemoveAt(index);
    }

    private async Task SaveAsync()
    {
        var isDeleteSuccessful = await QuestionChoiceService?.BulkDeleteAsync(_oldDtoList);
        if (!isDeleteSuccessful) ShowSnackBar();

        foreach (var choice in _dtoList)
        {
            choice.QuestionId = Model.QuestionId;
        }
        
        MudDialog?.Close(DialogResult.Ok(_dtoList));
    }

    private void Cancel() => MudDialog?.Cancel();

    private void ShowSnackBar()
    {
        Snackbar?.Clear();
        Snackbar?.Add("Failed deleting existing question choice(s).", Severity.Error);
    }
}