namespace RithmicSoul.Admin.Core.Models;

public class SurveyQuestionChoices
{
    public int ChoiceId { get; set; }

    public int? QuestionId { get; set; }

    public string QuestionText { get; set; }

    public List<string> QuestionChoices { get; set; } = new();

}