using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services.Admin;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Views.Dialogs;

public partial class SelectSurveyDialog : ComponentBase
{
    [Inject] private IAdminService<SurveyMetaDataDto> SurveyMetaDataService { get; set; }
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public bool IsShowDialog { get; set; }

    private SurveyMetaDataDto _model = new();
    private IEnumerable<SurveyMetaDataDto> _surveys;

    protected override async Task OnInitializedAsync()
    {
        _surveys = await SurveyMetaDataService.GetAllAsync();
        //_surveys = IsShowDialog ? items.Where(i => i.IsActive) : items;
    }

    private void SelectSurvey()
    {
        if (_model.SurveyId == 0) return;
        MudDialog.Close(DialogResult.Ok(_model.SurveyId));
    }

    private void Cancel() => MudDialog.Cancel();
}