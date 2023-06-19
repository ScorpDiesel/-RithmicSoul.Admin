using System.Linq.Expressions;
using System.Net.Http.Json;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyQuestionService : IAdminService<SurveyQuestionDto>
{
    private readonly AppSetting _appSetting;
    private readonly IAdminRepository<SurveyQuestionDto> _adminRepository;
    private const string RouteSuffix = "SurveyQuestions";

    public SurveyQuestionService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdminRepository<SurveyQuestionDto>>(_appSetting.BaseAddress);
    }

    public async Task<bool> InsertAsync(SurveyQuestionDto dto)
    {
        return await _adminRepository.InsertAsync(dto, RouteSuffix);
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionDto> dtos)
    {
        return await _adminRepository.BulkInsertAsync(dtos, RouteSuffix);
    }

    public async Task<bool> UpdateAsync(SurveyQuestionDto dto)
    {
        return await _adminRepository.UpdateAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionDto> dtos)
    {
        return await _adminRepository.BulkUpdateAsync(dtos, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(SurveyQuestionDto dto)
    {
        return await _adminRepository.DeleteAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionDto> dtos)
    {
        return await _adminRepository.BulkDeleteAsync(dtos, RouteSuffix);
    }

    public async Task<SurveyQuestionDto> GetByIdAsync(int id)
    {
        return await _adminRepository.GetByIdAsync(id, RouteSuffix);
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAllAsync()
    {
        return await _adminRepository.GetAllAsync(RouteSuffix);
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAsync(Expression<Func<SurveyQuestionDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetAsync(content, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyQuestionDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v1/SurveyQuestions");
        //return await api.DeleteAsync(content);
    }
}