using Microsoft.AspNetCore.Components;
using RithmicSoul.Models.Survey.ValueObjects;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsQuestionResponse : ComponentBase
{
    [Parameter] public string Style { get; set; }
    [Parameter] public string SurveyQuestion { get; set; }
    [Parameter] public List<QuestionAnswers> QuestionAnswers { get; set; }

    private int _elevation = 1;
    private List<string> _answers;
    private bool _showNoAnswersMessage;

    private void CardElevationUp() => _elevation = 7;

    private void CardElevationDown() => _elevation = 1;

    protected override void OnInitialized()
    {
        var questionAnswer = QuestionAnswers.First();
        _answers = QuestionAnswers.SelectMany(q => q.Answers).Where(a => !string.IsNullOrEmpty(a)).ToList();
        _showNoAnswersMessage = !_answers.Any();
    }
}