using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Admin.Infrastructure.Services;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;

namespace RithmicSoul.Admin.Client.Pages.Tables;

public partial class Tables : ComponentBase
{
    [Inject] protected IDialogService DialogService { get; set; }
    [Inject] private IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Inject] private IService<QuestionTypeDto> QuestionTypeService { get; set; }
    [Inject] private IService<SurveyMetaDataDto> SurveyService { get; set; }
    [Inject] private IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] private IService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }
    [Inject] private IService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] private IService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] private IService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }

    private AppSettings _appSettings;
    private IEnumerable<QuestionTypeDto> _questionTypes;
    private IEnumerable<SurveyTypeDto> _surveyTypes;
    private IEnumerable<SurveyMetaDataDto> _surveys;
    private IEnumerable<SurveyQuestionnaireDto> _surveyQuestionnaires;
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private IEnumerable<QuestionChoiceDto> _questionChoices;
    private IEnumerable<AdinkraSymbolDto> _adinkraSymbols;
    private int[] _surveyColumnsToHide;
    private int[] _surveyQuestionnaireColumnsToHide;
    private int[] _questionChoicesColumnsToHide;
    private int[] _surveyQuestionColumnsToHide;
    private int[] _adinkraSymbolColumnsToHide;
    private Type _surveyQuestionDialogType;
    private Type _questionChoiceDialogType;
    private Type _questionChoiceEditDialogType;
    private Type _surveyTypeDialogType;
    private Type _questionTypeDialogType;
    private readonly TableColumnWidth _adinkraSymbolColumnWidths = new();
    private readonly TableColumnWidth _surveyQuestionnaireColumnWidths = new();
    private readonly TableColumnWidth _surveyMetaDataColumnWidths = new();
    private readonly TableColumnWidth _surveyQuestionColumnWidths = new();
    private readonly TableColumnWidth _questionChoiceColumnWidths = new();
    //private Type _adinkraSymbolEditDialogType;

    public delegate Task<bool> EditModel(Type editDialogType, object dto, string tableName);


    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _adinkraSymbolColumnWidths.TableName = nameof(AdinkraSymbolDto).Replace("Dto", "");
        _adinkraSymbolColumnWidths.ColumnWidths[1] = "350px";
        _adinkraSymbolColumnWidths.ColumnWidths[2] = "450px";
        _adinkraSymbolColumnWidths.ColumnWidths[3] = "580px";
        _surveyQuestionnaireColumnWidths.TableName = nameof(SurveyQuestionnaireDto).Replace("Dto", "");
        _surveyQuestionnaireColumnWidths.ColumnWidths[1] = "280px";
        _surveyQuestionnaireColumnWidths.ColumnWidths[2] = "700px";
        _surveyQuestionnaireColumnWidths.ColumnWidths[3] = "300px";
        _surveyMetaDataColumnWidths.TableName = nameof(SurveyMetaDataDto).Replace("Dto", "");
        _surveyMetaDataColumnWidths.ColumnWidths[1] = "280px";
        _surveyMetaDataColumnWidths.ColumnWidths[2] = "230px";
        _surveyMetaDataColumnWidths.ColumnWidths[3] = "100px";
        _surveyMetaDataColumnWidths.ColumnWidths[4] = "150px";
        _surveyMetaDataColumnWidths.ColumnWidths[5] = "600px";
        _surveyMetaDataColumnWidths.ColumnWidths[6] = "300px";
        _surveyQuestionColumnWidths.TableName = nameof(SurveyQuestionDto).Replace("Dto", "");
        _surveyMetaDataColumnWidths.ColumnWidths[1] = "150px";
        _surveyMetaDataColumnWidths.ColumnWidths[2] = "575px";
        _surveyMetaDataColumnWidths.ColumnWidths[3] = "575px";
        _questionChoiceColumnWidths.TableName = nameof(QuestionChoiceDto).Replace("Dto", "");
        _questionChoiceColumnWidths.ColumnWidths[1] = "600px";
        _questionChoiceColumnWidths.ColumnWidths[2] = "200px";
        _questionChoiceColumnWidths.ColumnWidths[3] = "600px";
        _questionTypes = await QuestionTypeService.GetAllAsync();
        _surveyTypes = await SurveyTypeService.GetAllAsync();
        _surveys = await SurveyService.GetAllAsync();
        _surveyQuestionnaires = await SurveyQuestionnaireService.GetAllAsync();
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _questionChoices = await QuestionChoiceService.GetAllAsync();
        _adinkraSymbols = await AdinkraSymbolService.GetAllAsync();
        _surveyColumnsToHide = new[] { 1, 2 };
        _surveyQuestionnaireColumnsToHide = new[] { 1, 2, 3 };
        _surveyQuestionColumnsToHide = new[] { 1, 2 };
        _questionChoicesColumnsToHide = new[] { 1, 2, };
        _adinkraSymbolColumnsToHide = new[] { 1, 5, 6 };
        _surveyQuestionDialogType = typeof(SurveyQuestionDialog);
        _questionChoiceDialogType = typeof(NewQuestionChoiceDialog);
        _questionChoiceEditDialogType = typeof(EditQuestionChoiceDialog);
        _surveyTypeDialogType = typeof(SurveyTypeDialog);
        _questionTypeDialogType = typeof(QuestionTypeDialog);
        //_adinkraSymbolEditDialogType = typeof(EditQuestionChoiceDialog);
    }
}