using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services.Admin;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Views.Dialogs;

public partial class SelectSurveyDialog : ComponentBase
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }
    [Inject] IAdminService<SurveyDto> SurveyService { get; set; }

    protected SurveyDto Model = new();
    protected IEnumerable<SurveyDto> _surveys;

    protected override async Task OnInitializedAsync()
    {
        var items = await SurveyService.GetAllAsync();
        _surveys = items.Where(i => i.IsActive);
    }

    private void SelectSurvey()
    {
        if (Model.SurveyId == 0) return;
        MudDialog.Close(DialogResult.Ok(Model.SurveyId));
    }

    void Cancel() => MudDialog.Cancel();
}