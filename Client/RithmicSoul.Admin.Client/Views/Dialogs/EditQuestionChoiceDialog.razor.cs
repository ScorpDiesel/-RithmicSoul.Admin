using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulDatabaseLibrary.Utilities;

namespace RithmicSoul.Admin.Client.Views.Dialogs;

public partial class EditQuestionChoiceDialog : ComponentBase
{
    [Inject] private IService<QuestionChoiceDto>? QuestionChoiceService { get; set; }
    [Inject] private IService<SurveyQuestionDto>? SurveyQuestionService { get; set; }
    [Inject] private ISnackbar? Snackbar { get; set; }
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
    [Parameter] public QuestionChoiceDto? Model { get; set; }
    
    private string? _questionTypeName;
    private readonly List<string> _chooseableList = new();
    private string QuestionChoiceTableName = EntityUtility.GetTableName<QuestionChoice>();
    protected readonly List<QuestionChoiceDto> DtoList = new();
    private readonly List<QuestionChoiceDto> _oldDtoList = new();
    private IEnumerable<SurveyQuestionDto>? _surveyQuestions;
    private readonly DialogOptions _options = new();
    private bool _isLoading;
    private bool _isVisible;
    private AppSettings _appSettings;

    protected override async Task OnInitializedAsync()
    {
        ToggleLoadingScreen(true);
        await SetFieldsAsync();
        ToggleLoadingScreen(false);
    }

    private async Task SetFieldsAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _chooseableList.Add(_appSettings.QuestionChoicesMultipleChoice);
        _chooseableList.Add(_appSettings.QuestionChoicesCheckbox);
        var dtos = await QuestionChoiceService?.GetAsync(q => q.QuestionId == Model.QuestionId);
        DtoList.AddRange(dtos.ToList());
        _oldDtoList.AddRange(dtos.ToList());
        _surveyQuestions = await SurveyQuestionService?.GetAllAsync();
        _questionTypeName = _surveyQuestions.FirstOrDefault(q => q.QuestionText == Model.QuestionText)?.QuestionTypeName;
        _surveyQuestions = _surveyQuestions.Where(s => _chooseableList.Contains(s.QuestionTypeName));
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
        if (DtoList.Count < 15) DtoList.Add(new QuestionChoiceDto());
    }


    private void RemoveQuestionChoice(int index)
    {
        if (DtoList.Count > 1) DtoList.RemoveAt(index);
    }

    private async Task SaveAsync()
    {
        var isDeleteSuccessful = await QuestionChoiceService?.BulkDeleteAsync(_oldDtoList);
        if (!isDeleteSuccessful) ShowSnackBar(isDeleteSuccessful);

        foreach (var choice in DtoList)
        {
            choice.QuestionId = Model.QuestionId;
        }

        var isBulkInsertSuccessful = await QuestionChoiceService.BulkInsertAsync(DtoList);
        ShowSnackBar(isBulkInsertSuccessful);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog?.Cancel();

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