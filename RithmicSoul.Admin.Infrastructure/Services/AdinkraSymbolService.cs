using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Dtos;
using Serialize.Linq.Serializers;
using RithmicSoul.Admin.Application.Interfaces;
using Refit;
using Microsoft.Extensions.Options;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Admin.Application.Interfaces.Services;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AdinkraSymbolService : IAdinkraSymbolService<AdinkraSymbolDto>
{
    private readonly AppSetting _appSetting;
    private readonly IAdinkraSymbolRepository<AdinkraSymbolDto> _adminRepository;
    private const string RouteSuffix = "AdinkraSymbols";

    public AdinkraSymbolService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>(_appSetting.BaseAddress);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null)
    {
        return await _adminRepository.GetImageDataByIdAsync(id, w, h);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id)
    {
        return await _adminRepository.GetAudioDataByIdAsync(id);
    }

    public async Task<bool> InsertAsync(AdinkraSymbolDto dto)
    {
        return await _adminRepository.InsertAsync(dto, RouteSuffix);
    }

    public async Task<object> InsertForIdAsync(AdinkraSymbolDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkInsertAsync(List<AdinkraSymbolDto> dtos)
    {
        return await _adminRepository.BulkInsertAsync(dtos, RouteSuffix);
    }

    public async Task<bool> UpdateAsync(AdinkraSymbolDto dto)
    {
        return await _adminRepository.UpdateAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkUpdateAsync(List<AdinkraSymbolDto> dtos)
    {
        return await _adminRepository.BulkUpdateAsync(dtos, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(AdinkraSymbolDto dto)
    {
        return await _adminRepository.DeleteAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkDeleteAsync(List<AdinkraSymbolDto> dtos)
    {
        return await _adminRepository.BulkDeleteAsync(dtos, RouteSuffix);
    }

    public async Task<AdinkraSymbolDto> GetByIdAsync(int id)
    {
        return await _adminRepository.GetByIdAsync(id, RouteSuffix);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAllAsync()
    {
        return await _adminRepository.GetAllAsync(RouteSuffix);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAsync(Expression<Func<AdinkraSymbolDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetAsync(content, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(Expression<Func<AdinkraSymbolDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.DeleteAsync(content, RouteSuffix);
    }
}