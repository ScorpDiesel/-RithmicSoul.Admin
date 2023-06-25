namespace RithmicSoul.Admin.Core.Models;

public class QuestionResponseObject
{
    public int QuestionId { get; set; } 
    public string QuestionType { get; set; }
    public string QuestionText { get; set; }
    public List<string> Responses { get; set; }
    public bool IsSelected { get; set; }
    public override bool Equals(object obj)
    {
        if (obj is QuestionResponseObject other)
        {
            return QuestionId == other.QuestionId && QuestionType == other.QuestionType && QuestionText == other.QuestionText;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(QuestionId, QuestionText);
    }
}