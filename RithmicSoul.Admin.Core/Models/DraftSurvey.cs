using System.ComponentModel.DataAnnotations;

namespace RithmicSoul.Admin.Core.Models;

public class DraftSurvey
{
    public int SurveyId { get; set; }

    [Required]
    public string SurveyName { get; set; }

    [Required]
    public string SurveyDescription { get; set; }

    [Required]
    public int SurveyTypeId { get; set; }

    public string SurveyTypeName { get; set; }

    public DateTime DateCreated { get; set; }

    [Required]
    public List<int?> SurveyQuestions { get; set; } = new();

    public List<string> SurveyQuestionTextList { get; set; } = new();
}