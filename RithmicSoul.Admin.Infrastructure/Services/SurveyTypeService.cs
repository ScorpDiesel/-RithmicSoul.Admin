using System.Linq.Expressions;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyTypeService : IService<SurveyTypeDto>
{
    private readonly AppSetting _appSetting;
    private readonly IAdminRepository<SurveyTypeDto> _adminRepository;
    private const string RouteSuffix = "SurveyTypes";


    public SurveyTypeService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdminRepository<SurveyTypeDto>>(_appSetting.BaseAddress);
    }

    public async Task<bool> InsertAsync(SurveyTypeDto dto)
    {
        var response = await _adminRepository.InsertAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyTypeDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _adminRepository.BulkInsertAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyTypeDto dto)
    {
        var response = await _adminRepository.UpdateAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _adminRepository.BulkUpdateAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyTypeDto dto)
    {
        var response = await _adminRepository.DeleteAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _adminRepository.BulkDeleteAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetByIdAsync(object id)
    {
        var item = await _adminRepository.GetByIdAsync((int)id, RouteSuffix);
        return new List<SurveyTypeDto> { item };
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAllAsync()
    {
        return await _adminRepository.GetAllAsync(RouteSuffix);
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAsync(Expression<Func<SurveyTypeDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetAsync(content, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyTypeDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v1/SurveyTypes");
        //return await api.DeleteAsync(content);
    }
}