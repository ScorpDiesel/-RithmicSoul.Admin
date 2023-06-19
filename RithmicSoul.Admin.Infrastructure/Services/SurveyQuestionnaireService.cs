using System.Linq.Expressions;
using System.Net.Http.Json;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyQuestionnaireService : IAdminService<SurveyQuestionnaireDto>
{
    public async Task<bool> InsertAsync(SurveyQuestionnaireDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
        return await api.InsertAsync(dto);
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionnaireDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
        return await api.InsertForIdAsync(dto);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v2/SurveyQuestionnaires");
        return await api.BulkInsertAsync(dtos);
    }

    public async Task<bool> UpdateAsync(SurveyQuestionnaireDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
        return await api.UpdateAsync(dto);
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v2/SurveyQuestionnaires");
        return await api.BulkUpdateAsync(dtos);
    }

    public async Task<bool> DeleteAsync(SurveyQuestionnaireDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
        return await api.DeleteAsync(dto);
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v2/SurveyQuestionnaires");
        return await api.BulkDeleteAsync(dtos);
    }

    public async Task<SurveyQuestionnaireDto> GetByIdAsync(int id)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
        return await api.GetByIdAsync(id);
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAllAsync()
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
        return await api.GetAllAsync();
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdminRepository<SurveyQuestionnaireDto>>("v3/SurveyQuestionnaires");
        return await api.GetAsync(content);
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