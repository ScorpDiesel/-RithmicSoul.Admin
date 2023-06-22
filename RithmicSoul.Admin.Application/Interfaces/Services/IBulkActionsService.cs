namespace RithmicSoul.Admin.Application.Interfaces.Services;

public interface IBulkActionsService<T> where T : class
{
    Task<bool> BulkInsertAsync(List<T> dtos);
    Task<bool> BulkUpdateAsync(List<T> dtos);
    Task<bool> BulkDeleteAsync(List<T> dtos);
}