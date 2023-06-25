using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Application.Dtos;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Forms;

public partial class SurveyForm : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private IService<SurveyTypeDto> SurveyTypeService { get; set; }
    [Inject] private IService<SurveyQuestionDto> SurveyQuestionService { get; set; }
    [Inject] private IDraftSurveyService DraftSurveyService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public int? Id { get; set; }

    private static IEnumerable<SurveyTypeDto> _surveyTypes;
    private IEnumerable<SurveyQuestionDto> _surveyQuestions;
    private DraftSurveyDto Model = new();
    private AppSettings _appSettings;
    private string _scrollToBottom;
    private bool _surveyTypeDisabled;
    private bool _isActiveDefault;
    protected bool _showContent;
    protected string _contentStyle;

    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
        ShowContent();
    }

    private async Task InitializeAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _contentStyle = "display:none !important;";
        _surveyTypes = await SurveyTypeService.GetAllAsync();
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();

        if (Id is not null)
        {
            _surveyTypeDisabled = true;
            Model = await DraftSurveyService.GetByIdAsync((int)Id) ?? new();
            Model.SetDirty(false);
            _isActiveDefault = Model.IsActive;
        }
    }


    private MudBlazor.Converter<int, string> ConvertIdToName = new()
    {
        GetFunc = str => int.TryParse(str, out var id) ? id : 0,
        SetFunc = id => _surveyTypes.FirstOrDefault(qt => qt.SurveyTypeId == id)?.SurveyTypeName ?? "Select..."
    };

    private string GetSelectedQuestionTypeName(int id)
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
            Model.SetDirty(true);
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
            var options = new DialogOptions { CloseButton = true };
            var parameters = new DialogParameters
            {
                { "ContentText", "Save this record?" },
                { "CloseButtonText", "Yes" },
                { "CancelButtonText", "No" },
                { "Style", "min-width:300px" },
                { "Color", Color.Success }
            };
            var dialog = await DialogService?.ShowAsync<ActionDialog>("Confirm", parameters, options)!;
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

    private void QuestionIdChanged(int id, int index)
    {
        var oldId = Model.SurveyQuestionIds[index];
        Model.SetDirty(oldId != id);
        Model.SurveyQuestionIds[index] = id;
    }

    private void ToggleActive(bool? isChecked)
    {
        if (!isChecked.HasValue) return;
        var isActive = isChecked.Value;
        if (!Model.IsDirty && isActive && _isActiveDefault != isActive)
        {
            Model.SetDirty(isActive);
        }
        else if (!Model.IsDirty && _isActiveDefault == isActive)
        {
            Model.SetDirty(_isActiveDefault);
        }

        Model.IsActive = isActive;
    }

    private void ShowContent()
    {
        _contentStyle = "";
        _showContent = true;
    }
}