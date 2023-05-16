using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Enums;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Admin.Infrastructure.Logging;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulDatabaseLibrary.Utilities;
using RithmicSoulSharedLibrary.Extensions;

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

    public string QuestionTypeTableName = EntityUtility.GetTableName<QuestionType>();
    public string QuestionChoiceTableName = EntityUtility.GetTableName<QuestionChoice>();
    public string SurveyTypeTableName = EntityUtility.GetTableName<SurveyType>();
    public string SurveyTableName = EntityUtility.GetTableName<Survey>();
    public string SurveyQuestionnaireTableName = EntityUtility.GetTableName<SurveyQuestionnaire>();
    public string SurveyQuestionTableName = EntityUtility.GetTableName<SurveyQuestion>();
    protected string ConsoleOutput;

    protected override async Task OnInitializedAsync()
    {
        //await TimerService.StartExecutingAsync();
        //TimerService.JobExecuted += (_, _) => UpdateConsoleOutput();
    }

    protected void OpenSurveyForm()
    {
        Navigation.NavigateTo("/SurveyForm");
    }
}