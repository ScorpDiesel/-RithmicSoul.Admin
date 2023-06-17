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
    [Inject] IDatabaseService<AuthoredSurveyDto> AuthoredSurveyService { get; set; }
    [Inject] NavigationManager Navigation { get; set; }
    [CascadingParameter] public EventCallback<bool> HideMenus { get; set; }

    private IEnumerable<AuthoredSurveyDto> _authoredSurveys;

    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await HideMenus.InvokeAsync(false);
        _authoredSurveys = await AuthoredSurveyService.GetAllFromViewAsync();
    }

    private void NewSurvey() => Navigation.NavigateTo("/surveys/new");

    private async Task SelectEditSurveyDialogAsync()
    {
        var dialog = await DialogService?.ShowAsync<SelectSurveyDialog>("Select a survey to edit")!;
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var id = (int)result.Data;
            Navigation.NavigateTo($"/surveys/{id}");
        }
    }

    private async Task SelectShowSurveyDialogAsync()
    {
        var dialog = await DialogService?.ShowAsync<SelectSurveyDialog>("Select a survey to show")!;
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var id = (int)result.Data;

            var survey = id switch
            {
                16 => "product",
                21 => "adinkra",
                _ => null
            };
            Navigation.NavigateTo($"/surveys/{survey}");
        }
    }
}