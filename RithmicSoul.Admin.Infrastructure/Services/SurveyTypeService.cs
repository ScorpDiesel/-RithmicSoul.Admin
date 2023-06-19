using System.Linq.Expressions;
using System.Net.Http.Json;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyTypeService : IAdminService<SurveyTypeDto>
{
    public async Task<bool> InsertAsync(SurveyTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v1/SurveyTypes");
        return await api.InsertAsync(dto);
    }

    public async Task<object> InsertForIdAsync(SurveyTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v1/SurveyTypes");
        return await api.InsertForIdAsync(dto);
    }

    public async Task<bool> BulkInsertAsync(List<SurveyTypeDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v2/SurveyTypes");
        return await api.BulkInsertAsync(dtos);
    }

    public async Task<bool> UpdateAsync(SurveyTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v1/SurveyTypes");
        return await api.UpdateAsync(dto);
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyTypeDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v2/SurveyTypes");
        return await api.BulkUpdateAsync(dtos);
    }

    public async Task<bool> DeleteAsync(SurveyTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v1/SurveyTypes");
        return await api.DeleteAsync(dto);
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyTypeDto> dtos)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v2/SurveyTypes");
        return await api.BulkDeleteAsync(dtos);
    }

    public async Task<SurveyTypeDto> GetByIdAsync(int id)
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v1/SurveyTypes");
        return await api.GetByIdAsync(id);
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAllAsync()
    {
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v1/SurveyTypes");
        return await api.GetAllAsync();
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAsync(Expression<Func<SurveyTypeDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdminRepository<SurveyTypeDto>>("v3/SurveyTypes");
        return await api.GetAsync(content);
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