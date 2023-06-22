using Refit;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;

public interface IAdminRepository<T> where T : class
{
    [Post("/v1/{routeSuffix}")] Task<ApiResponse<object>> InsertAsync(T dto, string routeSuffix);
    [Post("/v1/{routeSuffix}")] Task<object> InsertForIdAsync(T dto, string routeSuffix);
    [Post("/v2/{routeSuffix}")] Task<ApiResponse<object>> BulkInsertAsync(List<T> dtos, string routeSuffix);
    [Put("/v1/{routeSuffix}")] Task<ApiResponse<object>> UpdateAsync(T dto, string routeSuffix);
    [Put("/v2/{routeSuffix}")] Task<ApiResponse<object>> BulkUpdateAsync(List<T> dtos, string routeSuffix);
    [Post("/v4/{routeSuffix}")] Task<ApiResponse<object>> DeleteAsync(T dto, string routeSuffix);
    [Post("/v4/{routeSuffix}")] Task<ApiResponse<object>> DeleteAsync([Body] StringContent content, string routeSuffix);
    [Post("/v5/{routeSuffix}")] Task<ApiResponse<object>> BulkDeleteAsync(List<T> dtos, string routeSuffix);
    [Get("/v1/{routeSuffix}/{id}")] Task<T> GetByIdAsync(int id, string routeSuffix);
    [Get("/v1/{routeSuffix}")] Task<IEnumerable<T>> GetAllAsync(string routeSuffix);
    [Post("/v3/{routeSuffix}")] Task<IEnumerable<T>> GetAsync([Body] StringContent content, string routeSuffix);
}