using System.Linq.Expressions;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyService : IAdminService<SurveyDto>
{
    private readonly AppSetting _appSetting;
    private readonly IAdminRepository<SurveyDto> _adminRepository;
    private const string RouteSuffix = "Surveys";

    public SurveyService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdminRepository<SurveyDto>>(_appSetting.BaseAddress);
    }

    public async Task<bool> InsertAsync(SurveyDto dto)
    {
        return await _adminRepository.InsertAsync(dto, RouteSuffix);
    }

    public async Task<object> InsertForIdAsync(SurveyDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyDto> dtos)
    {
        return await _adminRepository.BulkInsertAsync(dtos, RouteSuffix);
    }

    public async Task<bool> UpdateAsync(SurveyDto dto)
    {
        return await _adminRepository.UpdateAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyDto> dtos)
    {
        return await _adminRepository.BulkUpdateAsync(dtos, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(SurveyDto dto)
    {
        return await _adminRepository.DeleteAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyDto> dtos)
    {
        return await _adminRepository.BulkDeleteAsync(dtos, RouteSuffix);
    }

    public async Task<SurveyDto> GetByIdAsync(int id)
    {
        return await _adminRepository.GetByIdAsync(id, RouteSuffix);
    }

    public async Task<IEnumerable<SurveyDto>> GetAllAsync()
    {
        return await _adminRepository.GetAllAsync(RouteSuffix);
    }

    public async Task<IEnumerable<SurveyDto>> GetAsync(Expression<Func<SurveyDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetAsync(content, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<SurveyDto>>("v1/Surveys");
        //return await api.DeleteAsync(content);
    }
}