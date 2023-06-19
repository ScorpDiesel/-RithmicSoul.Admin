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

public class SurveyQuestionnaireService : IAdminService<SurveyQuestionnaireDto>
{
    private readonly AppSetting _appSetting;
    private readonly IAdminRepository<SurveyQuestionnaireDto> _adminRepository;
    private const string RouteSuffix = "SurveyQuestionnaires";

    public SurveyQuestionnaireService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>(_appSetting.BaseAddress);
    }

    public async Task<bool> InsertAsync(SurveyQuestionnaireDto dto)
    {
        return await _adminRepository.InsertAsync(dto, RouteSuffix);
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionnaireDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionnaireDto> dtos)
    {
        return await _adminRepository.BulkInsertAsync(dtos, RouteSuffix);
    }

    public async Task<bool> UpdateAsync(SurveyQuestionnaireDto dto)
    {
        return await _adminRepository.UpdateAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionnaireDto> dtos)
    {
        return await _adminRepository.BulkUpdateAsync(dtos, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(SurveyQuestionnaireDto dto)
    {
        return await _adminRepository.DeleteAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionnaireDto> dtos)
    {
        return await _adminRepository.BulkDeleteAsync(dtos, RouteSuffix);
    }

    public async Task<SurveyQuestionnaireDto> GetByIdAsync(int id)
    {
        return await _adminRepository.GetByIdAsync(id, RouteSuffix);
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAllAsync()
    {
        return await _adminRepository.GetAllAsync(RouteSuffix);
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetAsync(content, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
        //return await api.DeleteAsync(content);
    }
}