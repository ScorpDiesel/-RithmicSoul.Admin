using Refit;
using RithmicSoul.Admin.Application.Interfaces.Repositories;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class BulkActionsService<T> : IBulkActionsService<T> where T: class
{
    private readonly AppSetting _appSetting;
    private readonly IBulkActionsRepository<T> _bulkActionsRepository;
    private readonly string _routeSuffix = typeof(T).Name.Replace("Dto", "s");

    public BulkActionsService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _bulkActionsRepository = RestService.For<IBulkActionsRepository<T>>(_appSetting.BaseAddress);
    }

    public async Task<bool> BulkInsertAsync(List<T> dtos)
    {
        return await _bulkActionsRepository.BulkInsertAsync(dtos, _routeSuffix);
    }

    public async Task<bool> BulkUpdateAsync(List<T> dtos)
    {
        return await _bulkActionsRepository.BulkUpdateAsync(dtos, _routeSuffix);
    }

    public async Task<bool> BulkDeleteAsync(List<T> dtos)
    {
        return await _bulkActionsRepository.BulkDeleteAsync(dtos, _routeSuffix);
    }
}