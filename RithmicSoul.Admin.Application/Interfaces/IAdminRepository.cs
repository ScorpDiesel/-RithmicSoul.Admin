using Refit;
using System.Linq.Expressions;

namespace RithmicSoul.Admin.Application.Interfaces;

public interface IAdminRepository<T> where T : class
{
    [Post("")] Task<bool> InsertAsync(T dto);
    [Post("")] Task<object> InsertForIdAsync(T dto);
    [Post("")] Task<bool> BulkInsertAsync(List<T> dtos);
    [Put("")] Task<bool> UpdateAsync(T dto);
    [Put("")] Task<bool> BulkUpdateAsync(List<T> dtos);
    [Delete("")] Task<bool> DeleteAsync(T dto);
    [Post("")] Task<bool> DeleteAsync([Body] StringContent content);
    [Delete("")] Task<bool> BulkDeleteAsync(List<T> dtos);
    [Get("")] Task<T> GetByIdAsync(int id);
    [Get("")] Task<IEnumerable<T>> GetAllAsync();
    [Post("")] Task<IEnumerable<T>> GetAsync([Body] StringContent content);
}