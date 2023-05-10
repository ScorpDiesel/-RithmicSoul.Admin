using Microsoft.AspNetCore.Components;
using MudBlazor;
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


    protected override async Task OnInitializedAsync()
    {
        _surveyTypes = await SurveyTypeService.GetAllAsync();
        _surveyQuestions = await SurveyQuestionService.GetAllAsync();
        _questionTypes = await QuestionTypeService.GetAllAsync();
        _questionChoices = await QuestionChoiceService.GetAllAsync();
    }

    void AddQuestion()
    {
        if (Model.SurveyQuestions.Count < 10) Model.SurveyQuestions.Add(new int());
    }

    void RemoveQuestion()
    {
        if (Model.SurveyQuestions.Count > 0) Model.SurveyQuestions.RemoveAt(Model.SurveyQuestions.Count - 1);
    }

    //void RemoveQuestion(int index)
    //{
    //    //Console.WriteLine($"index {index} was deleted");
    //    //for (var s = 0; s < _surveyQuestionList.Count; s++)
    //    //{
    //    //    Console.WriteLine($"{s} = {index}");
    //    //}

    //    if (_surveyQuestionList.Count > 1)
    //    {
    //        var s = _surveyQuestionList.ElementAt(index);
    //        _surveyQuestionList.Remove(s);
    //    }
    //    //Console.WriteLine("----------------------------------");
    //    //for (var s = 0; s < _surveyQuestionList.Count; s++)
    //    //{
    //    //    Console.WriteLine($"{s} = {index}");
    //    //}
    //}


    protected void Cancel()
    {
        Navigation.NavigateTo("/Survey");
    }

    protected void Save()
    {
        Navigation.NavigateTo("/Survey");
    }
}