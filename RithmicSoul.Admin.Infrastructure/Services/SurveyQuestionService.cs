using System.Linq.Expressions;
using System.Net.Http.Json;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyQuestionService : IAdminService<SurveyQuestionDto>
{
    public async Task<bool> InsertAsync(SurveyQuestionDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v1/SurveyQuestions");
        return await api.InsertAsync(dto);
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v1/SurveyQuestions");
        return await api.InsertForIdAsync(dto);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v2/SurveyQuestions");
        return await api.BulkInsertAsync(dtos);
    }

    public async Task<bool> UpdateAsync(SurveyQuestionDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v1/SurveyQuestions");
        return await api.UpdateAsync(dto);
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v2/SurveyQuestions");
        return await api.BulkUpdateAsync(dtos);
    }

    public async Task<bool> DeleteAsync(SurveyQuestionDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v1/SurveyQuestions");
        return await api.DeleteAsync(dto);
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v2/SurveyQuestions");
        return await api.BulkDeleteAsync(dtos);
    }

    public async Task<SurveyQuestionDto> GetByIdAsync(int id)
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v1/SurveyQuestions");
        return await api.GetByIdAsync(id);
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAllAsync()
    {
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v1/SurveyQuestions");
        return await api.GetAllAsync();
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAsync(Expression<Func<SurveyQuestionDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdminRepository<SurveyQuestionDto>>("v3/SurveyQuestions");
        return await api.GetAsync(content);
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