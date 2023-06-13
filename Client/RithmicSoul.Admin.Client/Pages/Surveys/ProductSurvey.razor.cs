using Microsoft.AspNetCore.Components;
using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class ProductSurvey : ComponentBase
{
    [Inject] IService<AuthoredSurveyDto> AuthoredSurveyService { get; set; }


    private DraftSurveyDto? Model = new();
    private string _surveyDescription;
    private IEnumerable<AuthoredSurveyDto> _authoredSurveys;
    private IEnumerable<string> _questions;

    protected override async Task OnInitializedAsync()
    {
        _authoredSurveys = await AuthoredSurveyService.GetAllAsync();
        _surveyDescription = _authoredSurveys.First().SurveyDescription;
        _questions = _authoredSurveys.Select(q => q.QuestionText);

        //_choices = items.GroupBy(q => q.QuestionText)
        //    .Select(g => (Type: g.Key, Text: g.Select(x => x.ChoiceText).ToList()));
    }
}