using Refit;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories;

public interface IBulkActionsRepository<T> where T : class
{
    [Post("/v2/{routeSuffix}")] Task<bool> BulkInsertAsync(List<T> dtos, string routeSuffix);
    [Post("/v2/{routeSuffix}")] Task<bool> BulkUpdateAsync(List<T> dtos, string routeSuffix);
    [Post("/v2/{routeSuffix}")] Task<bool> BulkDeleteAsync(List<T> dtos, string routeSuffix);
}