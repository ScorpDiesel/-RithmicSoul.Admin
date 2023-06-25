namespace RithmicSoul.Admin.Core.Models;

public class QuestionChoiceObject
{
    public int QuestionId { get; set; }
    public string QuestionType { get; set; }
    public string QuestionText { get; set; }
    public List<string> QuestionChoices { get; set; }
}