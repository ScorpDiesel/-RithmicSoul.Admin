using Microsoft.AspNetCore.Components;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Tables;

public partial class Tables : ComponentBase
{
    [Inject] IService<QuestionTypeDto> QuestionTypeService { get; set; }
    [Inject] IService<SurveyDto> SurveyService { get; set; }
    [Inject] IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }
    [Inject] IService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] IService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] IService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }

    private IEnumerable<QuestionTypeDto> _questionTypes;
    private IEnumerable<SurveyTypeDto> _surveyTypes;
    private IEnumerable<SurveyDto> _surveys;
    private IEnumerable<SurveyQuestionnaireDto> _surveyQuestionnaires;
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private IEnumerable<QuestionChoiceDto> _questionChoices;
    private IEnumerable<AdinkraSymbolDto> _adinkraSymbols;
    
    protected override async Task OnInitializedAsync()
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