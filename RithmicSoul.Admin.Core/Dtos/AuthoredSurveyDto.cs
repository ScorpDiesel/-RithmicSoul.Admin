namespace RithmicSoul.Admin.Core.Dtos;

public class AuthoredSurveyDto
{
    public string SurveyName { get; set; }

    public string SurveyDescription { get; set; }

    public int SurveyTypeId { get; set; }

    public string SurveyTypeName { get; set; }

    public string SurveyQuestionText { get; set; }

    public int SurveyQuestionId { get; set; }

    public DateTime DateCreated { get; set; }
}