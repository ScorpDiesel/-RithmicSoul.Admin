using System.Linq.Expressions;
using System.Net.Http.Json;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class QuestionTypeService : IAdminService<QuestionTypeDto>
{
    public async Task<bool> InsertAsync(QuestionTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v1/QuestionTypes");
        return await api.InsertAsync(dto);
    }

    public async Task<object> InsertForIdAsync(QuestionTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v1/QuestionTypes");
        return await api.InsertForIdAsync(dto);
    }

    public async Task<bool> BulkInsertAsync(List<QuestionTypeDto> dtos)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v2/QuestionTypes");
        return await api.BulkInsertAsync(dtos);
    }

    public async Task<bool> UpdateAsync(QuestionTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v1/QuestionTypes");
        return await api.UpdateAsync(dto);
    }

    public async Task<bool> BulkUpdateAsync(List<QuestionTypeDto> dtos)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v2/QuestionTypes");
        return await api.BulkUpdateAsync(dtos);
    }

    public async Task<bool> DeleteAsync(QuestionTypeDto dto)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v1/QuestionTypes");
        return await api.DeleteAsync(dto);
    }

    public async Task<bool> BulkDeleteAsync(List<QuestionTypeDto> dtos)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v2/QuestionTypes");
        return await api.BulkDeleteAsync(dtos);
    }

    public async Task<QuestionTypeDto> GetByIdAsync(int id)
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v1/QuestionTypes");
        return await api.GetByIdAsync(id);
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAllAsync()
    {
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("http://localhost:7129/api/");
        return await api.GetAllAsync();
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAsync(Expression<Func<QuestionTypeDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v3/QuestionTypes");
        return await api.GetAsync(content);
    }

    public async Task<bool> DeleteAsync(Expression<Func<QuestionTypeDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<QuestionTypeDto>>("v2/QuestionTypes");
        //return await api.DeleteAsync(content);
    }
}