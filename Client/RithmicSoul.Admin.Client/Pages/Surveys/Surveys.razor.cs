using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Enums;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class Surveys : ComponentBase
{
    [Inject] IService<QuestionTypeDto> QuestionTypeService { get; set; }
    [Inject] IService<SurveyDto> SurveyService { get; set; }
    [Inject] IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }
    [Inject] IService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] IService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] IService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }
    [Inject] IAuthoredSurveyService AuthoredSurveyService { get; set; }
    [Inject] NavigationManager Navigation { get; set; }

    private IEnumerable<QuestionTypeDto> _questionTypes;
    private IEnumerable<SurveyTypeDto> _surveyTypes;
    private IEnumerable<SurveyDto> _surveys;
    private IEnumerable<SurveyQuestionnaireDto> _surveyQuestionnaires;
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private IEnumerable<QuestionChoiceDto> _questionChoices;
    private IEnumerable<AdinkraSymbolDto> _adinkraSymbols;

    //[Inject]
    //protected PeriodicTimerService TimerService { get; set; }
    protected string ConsoleOutput;

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

    protected void OpenSurveyForm()
    {
        Navigation.NavigateTo("/SurveyForm");
    }

    public async Task RefreshAsync() => StateHasChanged();
}