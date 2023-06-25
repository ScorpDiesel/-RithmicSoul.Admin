using Microsoft.AspNetCore.Components;
using RithmicSoul.Admin.Application.Interfaces.Services.Admin;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Pages.Tables;

public partial class Tables : ComponentBase
{
    [Inject] private IAdminService<QuestionTypeDto> QuestionTypeService { get; set; }
    [Inject] private IAdminService<SurveyMetaDataDto> SurveyService { get; set; }
    [Inject] private IAdminService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] private IAdminService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }
    [Inject] private IAdminService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] private IAdminService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] private IAdminService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }

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
    //private Type _adinkraSymbolEditDialogType;


    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
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