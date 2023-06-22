using Refit;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;

public interface ISurveyResponsesRepository
{
    [Get("/v1/SurveyResponses")] Task<IEnumerable<SurveyResponses>> GetAllResponsesAsync();
    [Get("/v1/SurveyResponses/{id}")] Task<SurveyResponses> GetResponsesByIdAsync(object id);
    [Post("/v2/SurveyResponses")] Task<IEnumerable<SurveyResponses>> GetResponsesAsync([Body] StringContent content);
    [Post("/v1/SurveyResponses")] Task<bool> InsertResponsesAsync(SurveyResponses item);
    [Put("/v1/SurveyResponses")] Task<bool> UpdateResponsesAsync(SurveyResponses item);
    [Delete("/v1/SurveyResponses")] Task<bool> DeleteResponsesAsync(SurveyResponses item);
}