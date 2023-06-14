namespace RithmicSoul.Admin.Core.Models;

public class QuestionResponseObject
{
    public string QuestionType { get; set; }
    public string QuestionText { get; set; }
    public object Response { get; set; }
    public bool isSelected { get; set; }
}