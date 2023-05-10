using System.ComponentModel.DataAnnotations;

namespace RithmicSoul.Admin.Client.Models;

public class AuthoredSurvey
{
    [Required]
    public string SurveyName { get; set; }

    [Required]
    public string SurveyDescription { get; set; }

    [Required]
    public int SurveyTypeId { get; set; }

    [Required] public List<int> SurveyQuestions { get; set; } = new();
}