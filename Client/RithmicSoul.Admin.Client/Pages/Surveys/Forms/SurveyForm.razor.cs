using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Application.Dtos;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Application.Interfaces.Services.Admin;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Forms;

public partial class SurveyForm : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }
    [Inject] NavigationManager Navigation { get; set; }
    [Inject] IAdminService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] IAdminService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] IDraftSurveyService DraftSurveyService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public int? Id { get; set; }

    private static IEnumerable<SurveyTypeDto> _surveyTypes;
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private DraftSurveyDto? Model = new();
    private AppSettings _appSettings;
    private string _scrollToBottom;
    private bool _surveyTypeDisabled;

    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _surveyTypes = await SurveyTypeService.GetAllAsync();
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();

        if (Id is not null)
        {
            _surveyTypeDisabled = true;
            Model = await DraftSurveyService.GetByIdAsync((int)Id) ?? new();
            Model.SetDirty(false);
        }
    }


    private MudBlazor.Converter<int, string> ConvertIdToName = new()
    {
        GetFunc = str => int.TryParse(str, out var id) ? id : 0,
        SetFunc = id => _surveyTypes.FirstOrDefault(qt => qt.SurveyTypeId == id)?.SurveyTypeName ?? "Select..."
    };

    private string? GetSelectedQuestionTypeName(int id)
    {
        return _surveyQuestions.FirstOrDefault(s => s.QuestionId == id)?.QuestionTypeName;
    }

    private void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar.Clear();
        Snackbar.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    private void AddQuestion()
    {
        if (Model?.SurveyQuestionIds.Count < _appSettings.SurveyQuestionsMaxCount)
        {
            Model.SurveyQuestionIds.Add(new int());
            _scrollToBottom = Model?.SurveyQuestionIds.Count > 4 ? "display: flex;flex-direction: column-reverse;" : "";
        }
    }

    private void RemoveQuestion(int index)
    {
        if (Model?.SurveyQuestionIds.Count > _appSettings.SurveyQuestionsMinCount)
        {
            Model.SurveyQuestionIds.RemoveAt(index);
            _scrollToBottom = Model?.SurveyQuestionIds.Count > 4 ? "display: flex;flex-direction: column-reverse;" : "";
        }
    }


    private async Task CancelAsync()
    {
        await SaveAsync();
        Navigation.NavigateTo("/surveys");
    }

    private async Task SaveAsync()
    {
        if (Model.IsDirty)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Save this record?" },
                { "CloseButtonText", "Yes" },
                { "CancelButtonText", "No" },
                { "Style", "min-width:300px" },
                { "Color", Color.Success }
            };
            var dialog = await DialogService?.ShowAsync<ActionDialog>("Confirm", parameters)!;
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var isSaveSuccess = await DraftSurveyService.SaveAsync(Model);
                if (isSaveSuccess) Model.SetDirty(false);
                var message = isSaveSuccess
                    ? _appSettings.AuthoredSurveyCreationSuccessMessage
                    : _appSettings.AuthoredSurveyCreationFailureMessage;
                ShowSnackBar(isSaveSuccess, message);
            }
        }
    }
}