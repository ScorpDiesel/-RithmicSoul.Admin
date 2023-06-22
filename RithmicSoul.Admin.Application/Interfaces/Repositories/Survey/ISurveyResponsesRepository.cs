using Refit;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;

public interface ISurveyResponsesRepository
{
    [Get("/v1/SurveyResponses")] Task<IEnumerable<SurveyResponses>> GetAllResponsesAsync();
    [Get("/v1/SurveyResponses/{id}")] Task<SurveyResponses> GetResponsesByIdAsync(Guid id);
    [Post("/v2/SurveyResponses")] Task<IEnumerable<SurveyResponses>> GetResponsesAsync([Body] StringContent content);
    [Post("/v1/SurveyResponses")] Task<ApiResponse<object>> InsertResponsesAsync(SurveyResponses item);
    [Put("/v1/SurveyResponses")] Task<ApiResponse<object>> UpdateResponsesAsync(SurveyResponses item);
    [Delete("/v4/SurveyResponses")] Task<ApiResponse<object>> DeleteResponsesAsync(SurveyResponses item);
}