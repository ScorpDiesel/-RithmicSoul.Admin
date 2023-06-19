using Microsoft.AspNetCore.Components;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Pages.Tables;

public partial class Tables : ComponentBase
{
    [Inject] IAdminService<QuestionTypeDto> QuestionTypeService { get; set; }
    [Inject] IAdminService<SurveyDto> SurveyService { get; set; }
    [Inject] IAdminService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IAdminService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }
    [Inject] IAdminService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] IAdminService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] IAdminService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }

    private IEnumerable<QuestionTypeDto> _questionTypes;
    private IEnumerable<SurveyTypeDto> _surveyTypes;
    private IEnumerable<SurveyDto> _surveys;
    private IEnumerable<SurveyQuestionnaireDto> _surveyQuestionnaires;
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private IEnumerable<QuestionChoiceDto> _questionChoices;
    private IEnumerable<AdinkraSymbolDto> _adinkraSymbols;
    
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
    }
}