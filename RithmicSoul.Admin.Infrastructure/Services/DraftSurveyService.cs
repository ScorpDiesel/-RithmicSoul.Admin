using System.Collections.ObjectModel;
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

    public async Task<bool> SaveAsync(DraftSurveyDto draftSurveyDto)
    {
        if (draftSurveyDto is null) throw new ArgumentNullException(nameof(draftSurveyDto));

        bool isSurveySaved;

        if (draftSurveyDto.SurveyId == 0)
        {
            isSurveySaved = await InsertSurvey(draftSurveyDto);
        }
        else
        {
            isSurveySaved = await UpdateSurvey(draftSurveyDto);
        }

        return isSurveySaved;
    }

    private async Task<bool> UpdateSurvey(DraftSurveyDto draftSurveyDto)
    {
        var result = await _surveyQuestionnaireService.GetAsync(q => q.SurveyId == draftSurveyDto.SurveyId);

        var dto = new SurveyDto
        {
            SurveyId = draftSurveyDto.SurveyId,
            SurveyName = draftSurveyDto.SurveyName,
            SurveyTypeId = draftSurveyDto.SurveyTypeId,
            SurveyDescription = draftSurveyDto.SurveyDescription
        };

        var surveyId = draftSurveyDto.SurveyId;
        var isSurveyUpdated = await _surveyService.UpdateAsync(dto);
        if (!isSurveyUpdated) return false;

        if (result is not null && result.Any())
        {
            var isDeleted = await _surveyQuestionnaireService.BulkDeleteAsync(result.ToList());
            if (!isDeleted) return false;
        }

        var questionnaires = draftSurveyDto.SurveyQuestionIds.Select(questionId => new SurveyQuestionnaireDto { SurveyId = surveyId, QuestionId = questionId }).ToList();
        var isQuestionnairesSaved = await _surveyQuestionnaireService.BulkInsertAsync(questionnaires);

        return isQuestionnairesSaved;
    }

    private async Task<bool> InsertSurvey(DraftSurveyDto draftSurveyDto)
    {
        var dto = new SurveyDto
        {
            SurveyName = draftSurveyDto.SurveyName,
            SurveyTypeId = draftSurveyDto.SurveyTypeId,
            SurveyDescription = draftSurveyDto.SurveyDescription
        };

        var responseId = await _surveyService.InsertForIdAsync(dto) ??
                       throw new Exception($"The id for the new {nameof(DraftSurvey)} was null");
        var surveyId = Convert.ToInt32(responseId);
        var questionnaires = draftSurveyDto.SurveyQuestionIds.Select(questionId => new SurveyQuestionnaireDto { SurveyId = surveyId, QuestionId = questionId }).ToList();
        return await _surveyQuestionnaireService.BulkInsertAsync(questionnaires);
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
                DateCreated = survey.DateCreated,
                SurveyDescription = survey.SurveyDescription,
                SurveyName = survey.SurveyName,
                SurveyTypeId = (int)survey.SurveyTypeId,
                SurveyTypeName = survey.SurveyTypeName,
                SurveyQuestionId = questionnaire.QuestionId,
                SurveyQuestionText = questionnaire.QuestionText
            };

            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<DraftSurveyDto?> GetByIdAsync(int id)
    {
        var survey = await _surveyService.GetByIdAsync(id);
        var items = await _surveyQuestionnaireService.GetAsync(q => q.SurveyId == id);
        IEnumerable<DraftSurveyQuestionnaireDto> questionnaires = null;

        if (items is not null)
        {
            questionnaires = items.Select(q => new DraftSurveyQuestionnaireDto
            {
                DateCreated = q.DateCreated,
                QuestionnaireId = q.QuestionnaireId,
                SurveyQuestionId = q.QuestionId,
                SurveyId = q.SurveyId,
                SurveyQuestionText = q.QuestionText,
                SurveyName = q.SurveyName
            });
        }

        if (survey is null) return null;

        var collection = questionnaires is null ? null : new ObservableCollection<DraftSurveyQuestionnaireDto>(questionnaires);
        return new DraftSurveyDto
        {
            DateCreated = survey.DateCreated,
            SurveyDescription = survey.SurveyDescription,
            SurveyName = survey.SurveyName,
            SurveyTypeId = (int)survey.SurveyTypeId,
            SurveyTypeName = survey.SurveyTypeName,
            SurveyQuestions = collection,
            SurveyId = id,
            SurveyQuestionIds = questionnaires is null ? new List<int>() : questionnaires.Select(q => q.SurveyQuestionId).ToList()
        };
    }
}