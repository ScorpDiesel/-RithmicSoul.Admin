using Refit;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;

public interface ISurveyResponseRepository
{
    [Get("/v1/SurveyResponse")] Task<IEnumerable<SurveyResponse>> GetAllResponsesAsync();
    [Get("/v1/SurveyResponse/{id}")] Task<IEnumerable<SurveyResponse>> GetResponsesByIdAsync(Guid id);
    [Post("/v2/SurveyResponse")] Task<IEnumerable<SurveyResponse>> GetResponsesAsync([Body] StringContent content);
    [Post("/v1/SurveyResponse")] Task<ApiResponse<object>> InsertResponsesAsync(SurveyResponse item);
    [Put("/v1/SurveyResponse")] Task<ApiResponse<object>> UpdateResponsesAsync(SurveyResponse item);
    [Delete("/v4/SurveyResponse")] Task<ApiResponse<object>> DeleteResponsesAsync(SurveyResponse item);
}