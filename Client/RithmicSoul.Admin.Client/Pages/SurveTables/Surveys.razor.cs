using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Client.Enums;
using RithmicSoul.Models.Survey;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulDatabaseLibrary.Utilities;

namespace RithmicSoul.Admin.Client.Pages.SurveTables;

public partial class SurveysBase : ComponentBase
{
    public MudDataGrid<QuestionTypeDto> MudGridQuestionType;
    public MudDataGrid<RatingTypeDto> MudGridRatingType;
    public MudDataGrid<SurveyTypeDto> MudGridSurveyType;
    public MudDataGrid<SurveyQuestionDto> MudGridSurveyQuestion;
    public MudDataGrid<SurveyDto> MudGridSurvey;
    public MudDataGrid<SurveyQuestionnaireDto> MudGridSurveyQuestionnaire;
    public MudDataGrid<RatingTypeQuestionTypeDto> MudGridRatingTypeQuestionType;

    [Inject] 
    IService<QuestionTypeDto> QuestionTypeService { get; set; }

    [Inject] 
    IService<RatingTypeDto> RatingTypeService { get; set; }

    [Inject] 
    IService<SurveyDto> SurveyService { get; set; }

    [Inject]
    IService<SurveyQuestionDto> SurveyQuestionService { get; set; }

    [Inject]
    IService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }

    [Inject]
    IService<SurveyTypeDto> SurveyTypeService { get; set; }

    [Inject] 
    IService<RatingTypeQuestionTypeDto> RatingTypeQuestionTypeService { get; set; }


    [Inject]
    IDialogService DialogService { get; set; }

    [Inject]
    ISnackbar Snackbar { get; set; }

    public IEnumerable<QuestionTypeDto> QuestionTypes = new List<QuestionTypeDto>();
    public IEnumerable<RatingTypeDto> RatingTypes;
    public IEnumerable<RatingTypeQuestionTypeDto> RatingTypeQuestionTypes;
    public IEnumerable<SurveyTypeDto> SurveyTypes;
    public IEnumerable<SurveyDto> Surveys;
    public IEnumerable<SurveyQuestionnaireDto> SurveyQuestionnaires;
    public IEnumerable<SurveyQuestionDto> SurveyQuestions;
    public string QuestionTypeTableName = EntityUtility.GetTableName<QuestionType>();
    public string RatingTypeQestionTypeTableName = EntityUtility.GetTableName<RatingTypeQuestionType>();
    public string RatingTypeTableName = EntityUtility.GetTableName<RatingType>();
    public string SurveyTypeTableName = EntityUtility.GetTableName<SurveyType>();
    public string SurveyTableName = EntityUtility.GetTableName<Survey>();
    public string SurveyQuestionnaireTableName = EntityUtility.GetTableName<SurveyQuestionnaire>();
    public string SurveyQuestionTableName = EntityUtility.GetTableName<SurveyQuestion>();

    public Dictionary<(string, int), string> RowHighlight = new();
    private Dictionary<string, (Func<dynamic,Task<dynamic>>, Func<dynamic, Task<dynamic>>, Func<dynamic, Task<dynamic>>, Func<Task>)> _callbacks = new();

    protected override async Task OnInitializedAsync()
    {
        QuestionTypes = await QuestionTypeService.GetAllAsync();
        RatingTypes = await RatingTypeService.GetAllAsync();
        RatingTypeQuestionTypes = await RatingTypeQuestionTypeService.GetAllAsync();
        SurveyTypes = await SurveyTypeService.GetAllAsync();
        Surveys = await SurveyService.GetAllAsync();
        SurveyQuestionnaires = await SurveyQuestionnaireService.GetAllAsync();
        SurveyQuestions = await SurveyQuestionService.GetAllAsync();

        AddCallbackServices();
    }

    private void AddCallbackServices()
    {
        _callbacks.Add(nameof(QuestionTypeDto),
            (async (dto) => await QuestionTypeService.InsertAsync(dto),
                async (dto) => await QuestionTypeService.UpdateAsync(dto),
                async (dto) => await QuestionTypeService.DeleteAsync(dto), GetAllQuestionTypesAsync));
        _callbacks.Add(nameof(RatingTypeDto),
            (async (dto) => await RatingTypeService.InsertAsync(dto),
                async (dto) => await RatingTypeService.UpdateAsync(dto),
                async (dto) => await RatingTypeService.DeleteAsync(dto), GetAllRatingTypesAsync));
        _callbacks.Add(nameof(RatingTypeQuestionTypeDto),
            (async (dto) => await RatingTypeQuestionTypeService.InsertAsync(dto),
                async (dto) => await RatingTypeQuestionTypeService.UpdateAsync(dto),
                async (dto) => await RatingTypeQuestionTypeService.DeleteAsync(dto), GetAllRatingTypeQuestionTypesAsync));
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
        return RowHighlight.TryGetValue((dto.GetType().Name, dto.GetHashCode()), out string bgColor) ? $"background-color: { bgColor };" : string.Empty;
    }
    protected void ResetRowHighlight(object dto)
    {
        RowHighlight.Remove((dto.GetType().Name, dto.GetHashCode()));
    }

    void ShowSnackBar(string message)
    {
        Snackbar.Configuration.SnackbarVariant = Variant.Outlined;
        Snackbar.Configuration.VisibleStateDuration = 4000;
        Snackbar.Configuration.HideTransitionDuration = 200;
        Snackbar.Configuration.ShowTransitionDuration = 200;
        Snackbar.Clear();
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        Snackbar.Add(message, Severity.Success);
    }

    public async Task DeleteAsync(object dto, int id)
    {
        SetRowHighlight(dto);
        var options = new DialogOptions { Position = DialogPosition.Center };
        var dialog = await DialogService.ShowAsync<DeleteDbRecordDialog>(null, options);
        var result = await dialog.Result;
        var dtoName = dto.GetType().Name;
        var tableName = dtoName.Replace("Dto", string.Empty);
        if (!result.Canceled)
        {
            var (insert, update, delete, getAll) = _callbacks[dtoName];
            await delete.Invoke(dto);
            await getAll.Invoke();
            ShowSnackBar($"A record from { tableName } has been deleted.");
        }

        ResetRowHighlight(dto);
        StateHasChanged();
    }

    public async Task CreateAsync<T,T1>() where T1 : ComponentBase
    {
        var options = new DialogOptions { Position = DialogPosition.Center };
        var tableName = typeof(T).Name.Replace("Dto", string.Empty);
        var dialog = await DialogService.ShowAsync<T1>($"New { tableName }", options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var dto = (T)result.Data;
            var dtoName = dto.GetType().Name;
            var (insert, update, delete, getAll) = _callbacks[dtoName];
            await insert.Invoke(dto);
            await getAll.Invoke();
            StateHasChanged();
            ShowSnackBar($"A new record was created in {tableName}.");
        }
    }

    public async Task UpdateRecordAsync(object dto)
    {
        SetRowHighlight(dto);
        var dtoName = dto.GetType().Name;
        var tableName = dtoName.Replace("Dto", string.Empty);
        var (insert, update, delete, getAll) = _callbacks[dtoName];
        await update.Invoke(dto);
        ResetRowHighlight(dto);
        ShowSnackBar($"A record was updated in {tableName}.");
    }

    public async Task EditContextAsync<T>(CellContext<T> context)
    {
        var dto = context.Item;
        SetRowHighlight(dto);
        await context.Actions.StartEditingItemAsync();
        
    }

    public async Task RefreshAsync(EntityType entityType)
    {
        switch (entityType)
        {
            case EntityType.QuestionType:
                await GetAllQuestionTypesAsync();
                break;
            case EntityType.RatingType:
                await GetAllRatingTypesAsync();
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
            case EntityType.RatingTypeQuestionType:
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

    public async Task GetAllRatingTypesAsync()
    {
        RatingTypes = await RatingTypeService.GetAllAsync();
        MudGridRatingType.Items = RatingTypes;
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

    public async Task GetAllRatingTypeQuestionTypesAsync()
    {
        RatingTypeQuestionTypes = await RatingTypeQuestionTypeService.GetAllAsync();
        MudGridSurvey.Items = Surveys;
    }
}