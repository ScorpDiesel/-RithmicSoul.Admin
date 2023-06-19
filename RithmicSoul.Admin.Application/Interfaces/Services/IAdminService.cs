using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces.Services;

public interface IAdminService<T> where T : class
{
    Task<bool> InsertAsync(T dto);
    Task<object> InsertForIdAsync(T dto);
    Task<bool> BulkInsertAsync(List<T> dtos);
    Task<bool> UpdateAsync(T dto);
    Task<bool> BulkUpdateAsync(List<T> dtos);
    Task<bool> DeleteAsync(T dto);
    Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);
    Task<bool> BulkDeleteAsync(List<T> dtos);
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);
}