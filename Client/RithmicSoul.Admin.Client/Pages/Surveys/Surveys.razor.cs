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
    [Inject] IDialogService? DialogService { get; set; }
    [Inject] IService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }
    [Inject] IDatabaseService<AuthoredSurveyDto> AuthoredSurveyService { get; set; }
    [Inject] NavigationManager Navigation { get; set; }

    private IEnumerable<AdinkraSymbolDto> _adinkraSymbols;
    private IEnumerable<AuthoredSurveyDto> _authoredSurveys;

    protected override async Task OnInitializedAsync()
    {
        _authoredSurveys = await AuthoredSurveyService.GetAllFromViewAsync();
    }

    private void NewSurvey() => Navigation.NavigateTo("/surveys/new");

    private async Task SelectSurveyDialogAsync()
    {
        var dialog = await DialogService?.ShowAsync<SelectSurveyDialog>("Select a survey to edit")!;
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var id = (int)result.Data;
            Navigation.NavigateTo($"/surveys/{id}");
        }
    }
}