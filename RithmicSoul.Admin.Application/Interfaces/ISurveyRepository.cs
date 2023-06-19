using Refit;
using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Application.Interfaces;

public interface ISurveyRepository<T> where T : class
{
    [Get("")] Task<IEnumerable<T>> GetAllFromViewAsync();
    [Post("")] Task<IEnumerable<T>> GetFromViewAsync([Body] StringContent content);
    [Post("")] Task<dynamic> ExecuteQueryStoredProcedureAsync([Body] StoredProcedureRequest spRequest);
}