using System.Linq.Expressions;
using System.Net.Http.Json;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyService : IAdminService<SurveyDto>
{
    public async Task<bool> InsertAsync(SurveyDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v1/Surveys");
        return await api.InsertAsync(dto);
    }

    public async Task<object> InsertForIdAsync(SurveyDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v1/Surveys");
        return await api.InsertForIdAsync(dto);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v2/Surveys");
        return await api.BulkInsertAsync(dtos);
    }

    public async Task<bool> UpdateAsync(SurveyDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v1/Surveys");
        return await api.UpdateAsync(dto);
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v2/Surveys");
        return await api.BulkUpdateAsync(dtos);
    }

    public async Task<bool> DeleteAsync(SurveyDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v1/Surveys");
        return await api.DeleteAsync(dto);
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v2/Surveys");
        return await api.BulkDeleteAsync(dtos);
    }

    public async Task<SurveyDto> GetByIdAsync(int id)
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v1/Surveys");
        return await api.GetByIdAsync(id);
    }

    public async Task<IEnumerable<SurveyDto>> GetAllAsync()
    {
        var api = RestService.For<IAdminRepository<SurveyDto>>("v1/Surveys");
        return await api.GetAllAsync();
    }

    public async Task<IEnumerable<SurveyDto>> GetAsync(Expression<Func<SurveyDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdminRepository<SurveyDto>>("v3/Surveys");
        return await api.GetAsync(content);
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