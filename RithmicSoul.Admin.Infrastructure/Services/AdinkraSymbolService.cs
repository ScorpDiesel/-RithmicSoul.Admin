using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Dtos;
using Serialize.Linq.Serializers;
using RithmicSoul.Admin.Application.Interfaces;
using Refit;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AdinkraSymbolService : IAdinkraSymbolService<AdinkraSymbolDto>
{
    public async Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v2/AdinkraSymbols");
        return await api.GetImageDataByIdAsync(id, w, h);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v3/AdinkraSymbols");
        return await api.GetAudioDataByIdAsync(id);
    }

    public async Task<bool> InsertAsync(AdinkraSymbolDto dto)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v1/AdinkraSymbols");
        return await api.InsertAsync(dto);
    }

    public async Task<object> InsertForIdAsync(AdinkraSymbolDto dto)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v1/AdinkraSymbols");
        return await api.InsertForIdAsync(dto);
    }

    public async Task<bool> BulkInsertAsync(List<AdinkraSymbolDto> dtos)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v2/AdinkraSymbols");
        return await api.BulkInsertAsync(dtos);
    }

    public async Task<bool> UpdateAsync(AdinkraSymbolDto dto)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v1/AdinkraSymbols");
        return await api.UpdateAsync(dto);
    }

    public async Task<bool> BulkUpdateAsync(List<AdinkraSymbolDto> dtos)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v2/AdinkraSymbols");
        return await api.BulkUpdateAsync(dtos);
    }

    public async Task<bool> DeleteAsync(AdinkraSymbolDto dto)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v1/AdinkraSymbols");
        return await api.DeleteAsync(dto);
    }

    public async Task<bool> BulkDeleteAsync(List<AdinkraSymbolDto> dtos)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v2/AdinkraSymbols");
        return await api.BulkDeleteAsync(dtos);
    }

    public async Task<AdinkraSymbolDto> GetByIdAsync(int id)
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v1/AdinkraSymbols");
        return await api.GetByIdAsync(id);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAllAsync()
    {
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v1/AdinkraSymbols");
        return await api.GetAllAsync();
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAsync(Expression<Func<AdinkraSymbolDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v3/AdinkraSymbols");
        return await api.GetAsync(content);
    }

    public async Task<bool> DeleteAsync(Expression<Func<AdinkraSymbolDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdinkraSymbolRepository<AdinkraSymbolDto>>("v2/AdinkraSymbols");
        return await api.DeleteAsync(content);
    }
}