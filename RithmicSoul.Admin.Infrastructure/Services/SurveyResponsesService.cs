using System.Linq.Expressions;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;
using RithmicSoul.Models.Survey.Models;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyResponsesService
{
    private readonly ISurveyResponsesRepository _surveyResponsesRepository;

    public SurveyResponsesService(ISurveyResponsesRepository surveyResponsesRepository)
    {
        _surveyResponsesRepository = surveyResponsesRepository;
    }

    public async Task<IEnumerable<SurveyResponses>> GetAllResponsesAsync()
    {
        return await _surveyResponsesRepository.GetAllResponsesAsync();
    }

    public async Task<SurveyResponses> GetResponsesByIdAsync(Guid id)
    {
        return await _surveyResponsesRepository.GetResponsesByIdAsync(id);
    }

    public async Task<IEnumerable<SurveyResponses>> GetResponsesAsync(Expression<Func<SurveyResponses, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _surveyResponsesRepository.GetResponsesAsync(content);
    }

    public async Task<bool> InsertResponsesAsync(SurveyResponses dto)
    {
        var response = await _surveyResponsesRepository.InsertResponsesAsync(dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateResponsesAsync(SurveyResponses dto)
    {
        var response = await _surveyResponsesRepository.UpdateResponsesAsync(dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteResponsesAsync(SurveyResponses dto)
    {
        var response = await _surveyResponsesRepository.DeleteResponsesAsync(dto);
        return response.IsSuccessStatusCode;
    }
}