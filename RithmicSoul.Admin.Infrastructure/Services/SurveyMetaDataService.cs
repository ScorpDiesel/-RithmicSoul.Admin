using System.Linq.Expressions;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyMetaDataService : IService<SurveyMetaDataDto>
{
    private readonly AppSetting _appSetting;
    private readonly IAdminRepository<SurveyMetaDataDto> _adminRepository;
    private const string RouteSuffix = "SurveyMetaData";

    public SurveyMetaDataService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdminRepository<SurveyMetaDataDto>>(_appSetting.BaseAddress);
    }

    public async Task<bool> InsertAsync(SurveyMetaDataDto dto)
    {
        var response = await _adminRepository.InsertAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyMetaDataDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyMetaDataDto> dtos)
    {
        var response = await _adminRepository.BulkInsertAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyMetaDataDto dto)
    {
        var response = await _adminRepository.UpdateAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyMetaDataDto> dtos)
    {
        var response = await _adminRepository.BulkUpdateAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyMetaDataDto dto)
    {
        var response = await _adminRepository.DeleteAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyMetaDataDto> dtos)
    {
        var response = await _adminRepository.BulkDeleteAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<SurveyMetaDataDto>> GetByIdAsync(object id)
    {
        var item = await _adminRepository.GetByIdAsync((int)id, RouteSuffix);
        return new List<SurveyMetaDataDto> { item };
    }

    public async Task<IEnumerable<SurveyMetaDataDto>> GetAllAsync()
    {
        return await _adminRepository.GetAllAsync(RouteSuffix);
    }

    public async Task<IEnumerable<SurveyMetaDataDto>> GetAsync(Expression<Func<SurveyMetaDataDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetAsync(content, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyMetaDataDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<SurveyMetaDataDto>>("v1/Surveys");
        //return await api.DeleteAsync(content);
    }
}