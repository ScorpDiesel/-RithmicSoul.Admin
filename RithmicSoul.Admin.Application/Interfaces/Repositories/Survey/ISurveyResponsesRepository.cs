using Refit;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;

public interface ISurveyResponseRepository
{
    [Get("/v1/SurveyResponse")] Task<IEnumerable<SurveyResponseDto>> GetAllResponsesAsync();
    [Get("/v1/SurveyResponse/{id}")] Task<IEnumerable<SurveyResponseDto>> GetResponsesByIdAsync(Guid id);
    [Post("/v2/SurveyResponse")] Task<IEnumerable<SurveyResponseDto>> GetResponsesAsync([Body] StringContent content);
    [Post("/v1/SurveyResponse")] Task<ApiResponse<object>> InsertResponsesAsync(SurveyResponseDto item);
    [Put("/v1/SurveyResponse")] Task<ApiResponse<object>> UpdateResponsesAsync(SurveyResponseDto item);
    [Delete("/v4/SurveyResponse")] Task<ApiResponse<object>> DeleteResponsesAsync(SurveyResponseDto item);
    [Delete("/v4/SurveyResponse")] Task<ApiResponse<object>> DeleteResponsesAsync([Body] StringContent content);
}