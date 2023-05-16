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
    [Inject] NavigationManager Navigation { get; set; }

    //[Inject]
    //protected PeriodicTimerService TimerService { get; set; }

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