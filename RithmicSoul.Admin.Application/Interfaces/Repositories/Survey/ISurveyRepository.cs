using Refit;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;

public interface ISurveyRepository<T> where T : class
{
    [Get("/v1/AuthoredSurveys")] Task<IEnumerable<T>> GetAllFromViewAsync();
    [Post("/v1/AuthoredSurveys")] Task<IEnumerable<T>> GetFromViewAsync([Body] StringContent content);
    [Post("/v2/AuthoredSurveys")] Task<dynamic> ExecuteQueryStoredProcedureAsync([Body] StoredProcedureRequest spRequest);
}