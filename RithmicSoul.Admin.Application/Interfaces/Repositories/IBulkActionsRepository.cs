using Refit;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories;

public interface IBulkActionsRepository<T> where T : class
{
    [Post("/v2/{routeSuffix}")] Task<ApiResponse<object>> BulkInsertAsync(List<T> dtos, string routeSuffix);
    [Post("/v2/{routeSuffix}")] Task<ApiResponse<object>> BulkUpdateAsync(List<T> dtos, string routeSuffix);
    [Post("/v5/{routeSuffix}")] Task<ApiResponse<object>> BulkDeleteAsync(List<T> dtos, string routeSuffix);
}