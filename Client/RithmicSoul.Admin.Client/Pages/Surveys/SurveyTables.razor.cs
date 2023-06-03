using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Enums;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class SurveyTables : ComponentBase
{
    [Inject] IService<QuestionTypeDto> QuestionTypeService { get; set; }
    [Inject] IService<SurveyDto> SurveyService { get; set; }
    [Inject] IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IService<SurveyQuestionnaireDto> SurveyQuestionnaireService { get; set; }
    [Inject] IService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] IService<QuestionChoiceDto> QuestionChoiceService { get; set; }
    [Inject] IAuthoredSurveyService AuthoredSurveyService { get; set; }
    [Inject] NavigationManager Navigation { get; set; }

    //[Inject]
    //protected PeriodicTimerService TimerService { get; set; }
    protected string ConsoleOutput;

    protected void OpenSurveyForm()
    {
        Navigation.NavigateTo("/SurveyForm");
    }
}