using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AuthoredSurveyService : IAuthoredSurveyService
{
    private readonly IService<SurveyDto> _surveyService;
    private readonly IService<SurveyQuestionnaireDto> _surveyQuestionnaireService;

    public AuthoredSurveyService(IService<SurveyDto> surveyService, IService<SurveyQuestionnaireDto> surveyQuestionnaireService)
    {
        _surveyService = surveyService;
        _surveyQuestionnaireService = surveyQuestionnaireService;
    }

    public async Task<bool> SaveAsync(AuthoredSurvey survey)
    {
        var surveyDto = new SurveyDto
        {
            SurveyName = survey.SurveyName,
            SurveyTypeId = survey.SurveyTypeId,
            SurveyDescription = survey.SurveyDescription
        };

        var response = await _surveyService.InsertForIdAsync(surveyDto) ?? throw new Exception($"The id for the new {nameof(AuthoredSurvey)} was null");
        var surveyId = Convert.ToInt32(response);
        var questionnaires = survey.SurveyQuestions
            .Select(v => (int)v).Select(questionId => new SurveyQuestionnaireDto { SurveyId = surveyId, SurveyQuestionId = questionId }).ToList();

        return await _surveyQuestionnaireService.BulkInsertAsync(questionnaires);
    }

    public async Task<List<AuthoredSurveyDto>> GetAllAsync()
    {
        List<AuthoredSurveyDto> dtos = new();
        var surveys = await _surveyService.GetAllAsync();
        var questionnaires = await _surveyQuestionnaireService.GetAllAsync();

        foreach (var questionnaire in questionnaires)
        {
            var survey = surveys.FirstOrDefault(s => s.SurveyId == questionnaire.SurveyId);
            if (survey is null) continue;

            var dto = new AuthoredSurveyDto
            {
                DateCreated = (DateTime)survey.DateCreated,
                SurveyDescription = survey.SurveyDescription,
                SurveyName = survey.SurveyName,
                SurveyTypeId = (int)survey.SurveyTypeId,
                SurveyTypeName = survey.SurveyTypeName,
                SurveyQuestionId = questionnaire.SurveyQuestionId,
                SurveyQuestionText = questionnaire.SurveyQuestionText
            };

            dtos.Add(dto);
        }

        return dtos;
    }
}