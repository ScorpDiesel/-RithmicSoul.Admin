using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Views.Dialogs;

public partial class SelectSurveyDialog : ComponentBase
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }
    [Inject] IService<SurveyDto> SurveyService { get; set; }

    protected SurveyDto Model = new();
    protected IEnumerable<SurveyDto> _surveys;

    protected override async Task OnInitializedAsync()
    {
        _surveys = await SurveyService.GetAllAsync();
    }

    private void SelectSurvey()
    {
        if (Model.SurveyId == 0) return;
        MudDialog.Close(DialogResult.Ok(Model.SurveyId));
    }

    void Cancel() => MudDialog.Cancel();
}