using Refit;
using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Application.Interfaces;

public interface ISurveyService<T> where T : class
{
    Task<IEnumerable<T>> GetAllFromViewAsync();
    Task<IEnumerable<T>> GetFromViewAsync([Body] Expression<Func<T, bool>> expression);
    Task<dynamic> ExecuteQueryStoredProcedureAsync([Body] StoredProcedureRequest spRequest);
}