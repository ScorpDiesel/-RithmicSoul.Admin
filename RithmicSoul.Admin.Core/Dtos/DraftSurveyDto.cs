using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Core.Dtos;

public class DraftSurveyDto
{
    public int SurveyId { get; set; }
    public string SurveyName { get; set; }

    public string SurveyDescription { get; set; }

    public int SurveyTypeId { get; set; }

    public string SurveyTypeName { get; set; }

    public string SurveyQuestionText { get; set; }

    public int SurveyQuestionId { get; set; }

    public DateTime DateCreated { get; set; }

    public List<int> SurveyQuestionIds { get; set; } = new();

    public List<string> SurveyQuestionTextList { get; set; } = new();

    public IEnumerable<SurveyQuestionnaireDto> SurveyQuestions { get; set; }
}