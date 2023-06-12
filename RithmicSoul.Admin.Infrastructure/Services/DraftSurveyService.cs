using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulDatabaseLibrary.Services;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class DraftSurveyService : IDraftSurveyService
{
    private readonly IService<SurveyDto> _surveyService;
    private readonly IService<SurveyQuestionnaireDto> _surveyQuestionnaireService;

    public DraftSurveyService(IService<SurveyDto> surveyService, IService<SurveyQuestionnaireDto> surveyQuestionnaireService)
    {
        _surveyService = surveyService;
        _surveyQuestionnaireService = surveyQuestionnaireService;
    }

    public async Task<bool> SaveAsync(DraftSurvey survey)
    {
        bool isSurveySaved;

        if (survey.SurveyId == 0)
        {
            isSurveySaved = await InsertSurvey(survey);
        }
        else
        {
            isSurveySaved = await UpdateSurvey(survey);
        }

        return isSurveySaved;
    }

    private async Task<bool> UpdateSurvey(DraftSurvey survey)
    {
        var result = await _surveyQuestionnaireService.GetAsync(q => q.SurveyId == survey.SurveyId);

        var dto = new SurveyDto
        {
            SurveyId = survey.SurveyId,
            SurveyName = survey.SurveyName,
            SurveyTypeId = survey.SurveyTypeId,
            SurveyDescription = survey.SurveyDescription
        };

        var surveyId = survey.SurveyId;
        var isSurveyUpdated = await _surveyService.UpdateAsync(dto);
        var questionnaires = survey.SurveyQuestions.Select(questionId => new SurveyQuestionnaireDto { SurveyId = surveyId, SurveyQuestionId = questionId }).ToList();
        var isQuestionnairesUpdated = result.Any() 
            ? await _surveyQuestionnaireService.BulkUpdateAsync(questionnaires) 
            : await _surveyQuestionnaireService.BulkInsertAsync(questionnaires);
        return isSurveyUpdated && isQuestionnairesUpdated;
    }

    private async Task<bool> InsertSurvey(DraftSurvey survey)
    {
        var dto = new SurveyDto
        {
            SurveyName = survey.SurveyName,
            SurveyTypeId = survey.SurveyTypeId,
            SurveyDescription = survey.SurveyDescription
        };

        var response = await _surveyService.InsertForIdAsync(dto) ??
                       throw new Exception($"The id for the new {nameof(DraftSurvey)} was null");
        var surveyId = Convert.ToInt32(response);
        var questionnaires = survey.SurveyQuestions.Select(questionId => new SurveyQuestionnaireDto { SurveyId = surveyId, SurveyQuestionId = questionId }).ToList();
        return await _surveyQuestionnaireService.BulkInsertAsync(questionnaires);
    }

    public async Task<bool> SaveAsync(DraftSurveyDto? dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var survey = new DraftSurvey
        {
            SurveyId = dto.SurveyId,
            SurveyName = dto.SurveyName,
            SurveyDescription = dto.SurveyDescription,
            SurveyTypeId = dto.SurveyTypeId,
            SurveyTypeName = dto.SurveyTypeName,
            SurveyQuestions = dto.SurveyQuestionIds
        };

        return await SaveAsync(survey);
    }

    public async Task<List<DraftSurveyDto>> GetAllAsync()
    {
        List<DraftSurveyDto> dtos = new();
        var surveys = await _surveyService.GetAllAsync();
        var questionnaires = await _surveyQuestionnaireService.GetAllAsync();

        foreach (var questionnaire in questionnaires)
        {
            var survey = surveys.FirstOrDefault(s => s.SurveyId == questionnaire.SurveyId);
            if (survey is null) continue;

            var dto = new DraftSurveyDto
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

    public async Task<DraftSurveyDto?> GetByIdAsync(int id)
    {
        var survey = await _surveyService.GetByIdAsync(id);
        var questionnaires = await _surveyQuestionnaireService.GetAsync(q => q.SurveyId == id);
        if (survey is null) return null;

        return new DraftSurveyDto
        {
            DateCreated = (DateTime)survey.DateCreated,
            SurveyDescription = survey.SurveyDescription,
            SurveyName = survey.SurveyName,
            SurveyTypeId = (int)survey.SurveyTypeId,
            SurveyTypeName = survey.SurveyTypeName,
            SurveyQuestions = questionnaires,
            SurveyId = id,
            SurveyQuestionIds = questionnaires.Select(q => q.SurveyQuestionId).ToList()
        };
    }
}