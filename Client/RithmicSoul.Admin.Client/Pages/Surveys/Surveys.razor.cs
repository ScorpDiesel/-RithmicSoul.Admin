using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Application.Interfaces.Services.Survey;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class Surveys : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IDatabaseService<AuthoredSurveyDto> AuthoredSurveyService { get; set; }
    [Inject] private IService<SurveyResponseDto> SurveyResponseService { get; set; }
    [Inject] private IDraftSurveyService DraftSurveyService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [CascadingParameter] public EventCallback<bool> HideMenus { get; set; }

    private IEnumerable<AuthoredSurveyDto> _authoredSurveys;
    private IEnumerable<SurveyResponseDto> _surveyResponses;

    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await HideMenus.InvokeAsync(false);
        _authoredSurveys = await AuthoredSurveyService!.GetAllFromViewAsync();
        _surveyResponses = await SurveyResponseService.GetAllAsync();
    }

    private void NewSurvey() => Navigation!.NavigateTo("/surveys/new");

    private async Task SelectEditSurveyDialogAsync()
    {
        var parameters = new DialogParameters { { "IsShowDialog", false } };
        var dialog = await DialogService?.ShowAsync<SelectSurveyDialog>("Select a survey to edit", parameters)!;
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var id = (int)result.Data;
            Navigation!.NavigateTo($"/surveys/{id}");
        }
    }

    private async Task SelectShowSurveyDialogAsync()
    {
        var parameters = new DialogParameters { { "IsShowDialog", true } };
        var dialog = await DialogService?.ShowAsync<SelectSurveyDialog>("Select a survey to show", parameters)!;
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var id = (int)result.Data;
            var survey = await DraftSurveyService!.GetByIdAsync(id);
            if (survey is null)
            {
                ShowSnackBar(false, "Survey not found.");
                return;
            }

            Navigation!.NavigateTo($"/surveys/{survey.ActiveSurveyName}");
        }
    }

    private void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar!.Clear();
        Snackbar.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    //private RenderFragment CreateChildContent<T>(IList<T> collection) where T : class
    //{
    //    return builder =>
    //    {
    //        builder.OpenComponent<MudDialog>(0);
    //        builder.AddAttribute(1, "Class", "pa-5");

    //        builder.AddAttribute(2, "ChildContent", (RenderFragment)((subBuilder) =>
    //        {
    //            subBuilder.OpenComponent<DialogContent>(0);

    //            subBuilder.AddAttribute(1, "ChildContent", (RenderFragment)((selectBuilder) =>
    //            {
    //                selectBuilder.OpenComponent<MudSelect<int>>(0);
    //                selectBuilder.AddAttribute(1, "Required", true);
    //                selectBuilder.AddAttribute(2, "Margin", Margin.Dense);
    //                selectBuilder.AddAttribute(3, "Variant", Variant.Outlined);
    //                selectBuilder.AddAttribute(4, "Label", "Your Label");
    //                selectBuilder.AddAttribute(5, "AnchorOrigin", Origin.BottomCenter);
    //                selectBuilder.AddAttribute(6, "Value", new EventCallback<int>(this, (int)Model.SurveyId));

    //                selectBuilder.AddAttribute(7, "ChildContent", (RenderFragment)((itemsBuilder) =>
    //                {
    //                    itemsBuilder.OpenComponent<MudSelectItem<int>>(0);
    //                    itemsBuilder.AddAttribute(1, "Value", 0);
    //                    itemsBuilder.AddAttribute(2, "Disabled", true);
    //                    itemsBuilder.AddContent(3, "Select...");
    //                    itemsBuilder.CloseComponent();

    //                    foreach (var st in collection)
    //                    {
    //                        itemsBuilder.OpenComponent<MudSelectItem<int>>(0);
    //                        itemsBuilder.AddAttribute(1, "Value", st.SurveyId);
    //                        itemsBuilder.AddContent(2, st.SurveyName);
    //                        itemsBuilder.CloseComponent();
    //                    }
    //                }));

    //                selectBuilder.CloseComponent();
    //            }));

    //            subBuilder.CloseComponent();
    //        }));

    //        builder.AddAttribute(3, "DialogActions", (RenderFragment)((actionsBuilder) =>
    //        {
    //            actionsBuilder.OpenComponent<MudButton>(0);
    //            actionsBuilder.AddAttribute(1, "OnClick", new EventCallback(this, Cancel));
    //            actionsBuilder.AddContent(2, "Cancel");
    //            actionsBuilder.CloseComponent();

    //            actionsBuilder.OpenComponent<MudButton>(1);
    //            actionsBuilder.AddAttribute(1, "Color", Color.Primary);
    //            actionsBuilder.AddAttribute(2, "OnClick", new EventCallback(this, SelectSurvey));
    //            actionsBuilder.AddContent(3, "Select");
    //            actionsBuilder.CloseComponent();
    //        }));

    //        builder.CloseComponent();
    //    };

    //}
}