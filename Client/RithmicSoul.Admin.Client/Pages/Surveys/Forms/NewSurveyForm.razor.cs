using Azure;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RithmicSoul.Admin.Client.Interfaces;
using RithmicSoul.Admin.Client.Models;
using RithmicSoul.Admin.Client.Services;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys.Forms;

public partial class NewSurveyForm : ComponentBase
{
    protected IEnumerable<SurveyTypeDto> _surveyTypes;
    protected IEnumerable<SurveyQuestionDto> _surveyQuestions;
    protected IEnumerable<QuestionTypeDto> _questionTypes;
    protected IEnumerable<QuestionChoiceDto> _questionChoices;
    protected AuthoredSurvey Model = new();
    protected string? _selectedQuestionTypeName;

    [Inject]
    NavigationManager Navigation { get; set; }

    [Inject]
    IService<SurveyTypeDto> SurveyTypeService { get; set; }

    [Inject]
    IService<SurveyQuestionDto> SurveyQuestionService { get; set; }

    [Inject]
    IService<QuestionTypeDto> QuestionTypeService { get; set; }

    [Inject]
    IService<QuestionChoiceDto> QuestionChoiceService { get; set; }

    [Inject]
    IAuthoredSurveyService AuthoredSurveyService { get; set; }

    [Inject]
    ISnackbar Snackbar { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _surveyTypes = await SurveyTypeService.GetAllAsync();
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _questionTypes = await QuestionTypeService.GetAllAsync();
        _questionChoices = await QuestionChoiceService.GetAllAsync();
    }

    void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar.Clear();
        Snackbar.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    private void AddQuestion()
    {
        if (Model.SurveyQuestions.Count < 10) Model.SurveyQuestions.Add(new int());
    }

    private void QuestionChanged(int id)
    {
        if (_surveyQuestions is null) return;
        var dto = _surveyQuestions.FirstOrDefault(q => q.QuestionId == id);
        _selectedQuestionTypeName = dto?.QuestionTypeName;
    }

    private void RemoveQuestion(int index)
    {
        if (Model.SurveyQuestions.Count > 1) Model.SurveyQuestions.RemoveAt(index);
    }


    protected void Cancel() => Navigation.NavigateTo("/surveys");

    protected async Task Save()
    {
        var response = await AuthoredSurveyService.SaveAsync(Model);
        string message;

        if (response)
        {
            message = "The new authored survey was created.";
        }
        else
        {
            message = "There was an error creating the new authored survey.";
        }

        ShowSnackBar(response, message);
        //Navigation.NavigateTo("/surveys");
    }
}