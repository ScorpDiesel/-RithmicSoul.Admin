using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Dtos;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;
using RithmicSoul.Admin.Core.Models;
using Serialize.Linq.Serializers;
using RithmicSoul.Admin.Application.Interfaces.Services;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AdinkraSymbolService : IService<AdinkraSymbolDto>
{
    private readonly IAdinkraSymbolRepository _adinkraSymbolRepository;
    private readonly AppSetting _appSetting;
    private readonly IAdminRepository<AdinkraSymbolDto> _adminRepository;
    private const string RouteSuffix = "AdinkraSymbols";

    public AdinkraSymbolService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdminRepository<AdinkraSymbolDto>>(_appSetting.BaseAddress);
        _adinkraSymbolRepository = RestService.For<IAdinkraSymbolRepository>(_appSetting.BaseAddress);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null)
    {
        return await _adinkraSymbolRepository.GetImageDataByIdAsync(id, w, h);
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id)
    {
        return await _adinkraSymbolRepository.GetAudioDataByIdAsync(id);
    }

    public async Task<bool> InsertAsync(AdinkraSymbolDto dto)
    {
        var response = await _adminRepository.InsertAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(AdinkraSymbolDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }
    public async Task<bool> UpdateAsync(AdinkraSymbolDto dto)
    {
        var response = await _adminRepository.UpdateAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(AdinkraSymbolDto dto)
    {
        var response = await _adminRepository.DeleteAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetByIdAsync(object id)
    {
        var item = await _adminRepository.GetByIdAsync((int)id, RouteSuffix);
        return new List<AdinkraSymbolDto> { item };
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
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<AdinkraSymbolDto>>("v1/QuestionChoices");
        //return await api.DeleteAsync(content);
    }
}