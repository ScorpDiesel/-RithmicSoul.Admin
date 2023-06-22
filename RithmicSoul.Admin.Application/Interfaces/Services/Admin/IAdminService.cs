using System.Linq.Expressions;

namespace RithmicSoul.Admin.Application.Interfaces.Services.Admin;

public interface IAdminService<T> where T : class
{
    Task<bool> InsertAsync(T dto);
    Task<object> InsertForIdAsync(T dto);
    Task<bool> UpdateAsync(T dto);
    Task<bool> DeleteAsync(T dto);
    Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);
}