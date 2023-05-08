using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Client.Enums;
using RithmicSoul.Admin.Client.Logging;
using RithmicSoul.Admin.Client.Pages.Surveys.Dialogs;
using RithmicSoul.Admin.Client.Services;
using RithmicSoul.Models.Survey;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulDatabaseLibrary.Utilities;
using RithmicSoulSharedLibrary.Extensions;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class SurveyTablesBase : ComponentBase
{
    public MudDataGrid<QuestionTypeDto> MudGridQuestionType;
    public MudDataGrid<SurveyTypeDto> MudGridSurveyType;
    public MudDataGrid<SurveyQuestionDto> MudGridSurveyQuestion;
    public MudDataGrid<SurveyDto> MudGridSurvey;
    public MudDataGrid<SurveyQuestionnaireDto> MudGridSurveyQuestionnaire;
    public MudDataGrid<QuestionChoiceDto> MudGridQuestionChoice;

    [Inject]
    IService<QuestionTypeDto> QuestionTypeService { get; set; }

    [Inject]
    IService<SurveyDto> SurveyService { get; set; }

    [Inject]
    IService<SurveyQuestionDto> SurveyQuestionService { get; set; }

    [Inject]
    IService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }

    [Inject]
    IService<SurveyTypeDto> SurveyTypeService { get; set; }

    [Inject]
    IService<QuestionChoiceDto> QuestionChoiceService { get; set; }


    [Inject]
    IDialogService DialogService { get; set; }

    [Inject]
    ISnackbar Snackbar { get; set; }

    [Inject]
    NavigationManager Navigation { get; set; }

    [Inject]
    protected ConsoleRedirectLogger<SurveyTables> Logger { get; set; }

    [Inject]
    protected PeriodicTimerService TimerService { get; set; }

    public IEnumerable<QuestionTypeDto> QuestionTypes = new List<QuestionTypeDto>();
    public IEnumerable<QuestionChoiceDto> QuestionChoices;
    public IEnumerable<SurveyTypeDto> SurveyTypes;
    public IEnumerable<SurveyDto> Surveys;
    public IEnumerable<SurveyQuestionnaireDto> SurveyQuestionnaires;
    public IEnumerable<SurveyQuestionDto> SurveyQuestions;
    public string QuestionTypeTableName = EntityUtility.GetTableName<QuestionType>();
    public string QuestionChoiceTableName = EntityUtility.GetTableName<QuestionChoice>();
    public string SurveyTypeTableName = EntityUtility.GetTableName<SurveyType>();
    public string SurveyTableName = EntityUtility.GetTableName<Survey>();
    public string SurveyQuestionnaireTableName = EntityUtility.GetTableName<SurveyQuestionnaire>();
    public string SurveyQuestionTableName = EntityUtility.GetTableName<SurveyQuestion>();
    protected bool _isLoading;

    public Dictionary<(string, int), string> RowHighlight = new();
    private Dictionary<string, (Func<dynamic, Task<dynamic>>, Func<dynamic, Task<dynamic>>, Func<dynamic, Task<dynamic>>, Func<Task>)> _callbacks = new();

    protected string ConsoleOutput;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        QuestionTypes = await QuestionTypeService.GetAllAsync();
        QuestionChoices = await QuestionChoiceService.GetAllAsync();
        SurveyTypes = await SurveyTypeService.GetAllAsync();
        Surveys = await SurveyService.GetAllAsync();
        SurveyQuestionnaires = await SurveyQuestionnaireService.GetAllAsync();
        SurveyQuestions = await SurveyQuestionService.GetAllAsync();

        AddCallbackServices();
        _isLoading = false;
        await TimerService.StartExecutingAsync();
        TimerService.JobExecuted += (_, _) => UpdateConsoleOutput();
    }

    void UpdateConsoleOutput()
    {
        var message = Logger.GetLogMessages();
        ConsoleOutput = $"{message.Replace("\n", " ")}\n{ConsoleOutput}";
        StateHasChanged();
    }

    private void AddCallbackServices()
    {
        _callbacks.Add(nameof(QuestionTypeDto),
            (async (dto) => await QuestionTypeService.InsertAsync(dto),
                async (dto) => await QuestionTypeService.UpdateAsync(dto),
                async (dto) => await QuestionTypeService.DeleteAsync(dto), GetAllQuestionTypesAsync));
        _callbacks.Add(nameof(QuestionChoiceDto),
            (async (dto) => await QuestionChoiceService.BulkInsertAsync(dto),
                async (dto) => await QuestionChoiceService.BulkUpdateAsync(dto),
                async (dto) => await QuestionChoiceService.BulkDeleteAsync(dto), GetAllQuestionChoicesAsync));
        _callbacks.Add(nameof(SurveyTypeDto),
            (async (dto) => await SurveyTypeService.InsertAsync(dto),
                async (dto) => await SurveyTypeService.UpdateAsync(dto),
                async (dto) => await SurveyTypeService.DeleteAsync(dto), GetAllSurveyTypesAsync));
        _callbacks.Add(nameof(SurveyDto),
            (async (dto) => await SurveyService.InsertAsync(dto), async (dto) => await SurveyService.UpdateAsync(dto),
                async (dto) => await SurveyService.DeleteAsync(dto), GetAllSurveysAsync));
        _callbacks.Add(nameof(SurveyQuestionnaireDto),
            (async (dto) => await SurveyQuestionnaireService.InsertAsync(dto),
                async (dto) => await SurveyQuestionnaireService.UpdateAsync(dto),
                async (dto) => await SurveyQuestionnaireService.DeleteAsync(dto), GetAllSurveyQuestionnairesAsync));
        _callbacks.Add(nameof(SurveyQuestionDto),
            (async (dto) => await SurveyQuestionService.InsertAsync(dto),
                async (dto) => await SurveyQuestionService.UpdateAsync(dto),
                async (dto) => await SurveyQuestionService.DeleteAsync(dto), GetAllSurveyQuestionsAsync));
    }

    protected void SetRowHighlight(object dto)
    {
        var backgroundColor = "rgba(224, 224, 224, 1)";
        RowHighlight[(dto.GetType().Name, dto.GetHashCode())] = backgroundColor;
    }

    protected string GetRowHighlight(object dto, int rowIndex)
    {
        return RowHighlight.TryGetValue((dto.GetType().Name, dto.GetHashCode()), out string bgColor) ? $"background-color: {bgColor};" : string.Empty;
    }
    protected void ResetRowHighlight(object dto)
    {
        RowHighlight.Remove((dto.GetType().Name, dto.GetHashCode()));
    }

    void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar.Clear();
        Snackbar.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    protected void OpenSurveyForm()
    {
        Navigation.NavigateTo("/SurveyForm");
    }

    void HandleResponse(dynamic response, object dto, string tableName)
    {

        ResetRowHighlight(dto);
        StateHasChanged();
    }

    public async Task DeleteAsync<T>(T dto, int id)
    {
        SetRowHighlight(dto);
        var options = new DialogOptions { Position = DialogPosition.Center };
        var dialog = await DialogService.ShowAsync<DeleteItemDialog>(null, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var dtoName = dto.GetType().Name;
            var tableName = dtoName.Replace("Dto", string.Empty);
            var dtoList = new List<T> { dto }; //TODO: Implement checkbox selection in each row to add to collection to be sent to BulkDelete
            var (insert, update, delete, getAll) = _callbacks[dtoName];
            var response = await delete.Invoke(dtoList);
            string message;

            if (response)
            {
                message = $"An item from {tableName} has been deleted.";
                await getAll.Invoke();
                StateHasChanged();
            }
            else
            {
                message = $"There was an error deleting the item in {tableName}.";
            }

            ShowSnackBar(response, message);
        }

        ResetRowHighlight(dto);
        StateHasChanged();
    }

    public async Task CreateAsync<T, T1>() where T1 : ComponentBase
    {
        var options = new DialogOptions { Position = DialogPosition.Center };
        var tableName = typeof(T).Name.Replace("Dto", string.Empty);
        var dialog = await DialogService.ShowAsync<T1>($"New {tableName}", options);
        var result = await dialog.Result;

        await CreateNewItemAsync<T>(result, tableName);
    }

    private async Task CreateNewItemAsync<T>(DialogResult result, string tableName)
    {
        if (!result.Canceled)
        {
            object resultData;
            if (result.Data.IsGenericList())
            {
                resultData = (List<T>)result.Data;
            }
            else
            {
                resultData = (T)result.Data;
            }

            var dtoName = typeof(T).Name;

            var (insert, update, delete, getAll) = _callbacks[dtoName];
            var response = await insert.Invoke(resultData);
            string message;

            if (response)
            {
                message = $"A new Item was created in {tableName}.";
                await getAll.Invoke();
                StateHasChanged();
            }
            else
            {
                message = $"There was an error creating a new Item in {tableName}.";
            }

            ShowSnackBar(response, message);
        }
    }

    protected async Task EditMultiItemsAsync<T, T1>(T dto) where T1 : ComponentBase
    {
        var parameters = new DialogParameters { { nameof(dto), dto } };
        var tableName = typeof(T).Name.Replace("Dto", string.Empty);
        var dialog = await DialogService.ShowAsync<T1>($"Edit {tableName}", parameters);
        var result = await dialog.Result;

        await CreateNewItemAsync<T>(result, tableName);
    }

    public async Task UpdateItemAsync(object dto)
    {
        SetRowHighlight(dto);
        var dtoName = dto.GetType().Name;
        var tableName = dtoName.Replace("Dto", string.Empty);
        var (insert, update, delete, getAll) = _callbacks[dtoName];
        var response = await update.Invoke(dto);
        ResetRowHighlight(dto);
        StateHasChanged();
        ShowSnackBar(response, $"A Item was updated in {tableName}.");
    }

    public async Task EditByContextAsync<T>(CellContext<T> context)
    {
        var dto = context.Item;
        SetRowHighlight(dto);
        await context.Actions.StartEditingItemAsync();

    }

    public async Task RefreshAsync(EntityType entityType)
    {
        switch (entityType)
        {
            case EntityType.QuestionChoice:
                await GetAllQuestionChoicesAsync();
                break;
            case EntityType.QuestionType:
                await GetAllQuestionTypesAsync();
                break;
            case EntityType.SurveyQuestionnaire:
                await GetAllSurveyQuestionnairesAsync();
                break;
            case EntityType.SurveyQuestion:
                await GetAllSurveyQuestionsAsync();
                break;
            case EntityType.Survey:
                await GetAllSurveysAsync();
                break;
            case EntityType.SurveyType:
                await GetAllSurveyTypesAsync();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
        }

        StateHasChanged();
    }

    public async Task GetAllSurveyTypesAsync()
    {
        SurveyTypes = await SurveyTypeService.GetAllAsync();
        MudGridSurveyType.Items = SurveyTypes;
    }

    public async Task GetAllQuestionTypesAsync()
    {
        QuestionTypes = await QuestionTypeService.GetAllAsync();
        MudGridQuestionType.Items = QuestionTypes;
    }

    public async Task GetAllSurveyQuestionnairesAsync()
    {
        SurveyQuestionnaires = await SurveyQuestionnaireService.GetAllAsync();
        MudGridSurveyQuestionnaire.Items = SurveyQuestionnaires;
    }

    public async Task GetAllSurveyQuestionsAsync()
    {
        SurveyQuestions = await SurveyQuestionService.GetAllAsync();
        MudGridSurveyQuestion.Items = SurveyQuestions;
    }

    public async Task GetAllSurveysAsync()
    {
        Surveys = await SurveyService.GetAllAsync();
        MudGridSurvey.Items = Surveys;
    }

    public async Task GetAllQuestionChoicesAsync()
    {
        QuestionChoices = await QuestionChoiceService.GetAllAsync();
        MudGridSurvey.Items = Surveys;
    }
}