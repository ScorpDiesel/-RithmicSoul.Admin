using System.Linq.Expressions;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;
using RithmicSoul.Admin.Application.Interfaces.Services.Admin;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
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
        var response = await _adminRepository.InsertAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionnaireDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _adminRepository.BulkInsertAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyQuestionnaireDto dto)
    {
        var response = await _adminRepository.UpdateAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _adminRepository.BulkUpdateAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyQuestionnaireDto dto)
    {
        var response = await _adminRepository.DeleteAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _adminRepository.BulkDeleteAsync(dtos, RouteSuffix);
        return response.IsSuccessStatusCode;
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