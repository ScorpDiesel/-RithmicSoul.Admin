using Refit;
using System.Linq.Expressions;

namespace RithmicSoul.Admin.Application.Interfaces;

public interface IAdminRepository<T> where T : class
{
    [Post("/v1/{routeSuffix}")] Task<bool> InsertAsync(T dto, string routeSuffix);
    [Post("/v1/{routeSuffix}")] Task<object> InsertForIdAsync(T dto, string routeSuffix);
    [Post("/v2/{routeSuffix}")] Task<bool> BulkInsertAsync(List<T> dtos, string routeSuffix);
    [Put("/v1/{routeSuffix}")] Task<bool> UpdateAsync(T dto, string routeSuffix);
    [Put("/v2/{routeSuffix}")] Task<bool> BulkUpdateAsync(List<T> dtos, string routeSuffix);
    [Delete("/v1/{routeSuffix}")] Task<bool> DeleteAsync(T dto, string routeSuffix);
    [Post("/v2/{routeSuffix}")] Task<bool> DeleteAsync([Body] StringContent content, string routeSuffix);
    [Delete("/v2/{routeSuffix}")] Task<bool> BulkDeleteAsync(List<T> dtos, string routeSuffix);
    [Get("/v1/{routeSuffix}")] Task<T> GetByIdAsync(int id, string routeSuffix);
    [Get("/v1/{routeSuffix}")] Task<IEnumerable<T>> GetAllAsync(string routeSuffix);
    [Post("/v3/{routeSuffix}")] Task<IEnumerable<T>> GetAsync([Body] StringContent content, string routeSuffix);
}