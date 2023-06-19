using System.Linq.Expressions;
using System.Net.Http.Json;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class QuestionChoiceService : IAdminService<QuestionChoiceDto>
{
    public async Task<bool> InsertAsync(QuestionChoiceDto dto)
    {
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        return await api.InsertAsync(dto);
    }

    public async Task<object> InsertForIdAsync(QuestionChoiceDto dto)
    {

        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        return await api.InsertForIdAsync(dto);
    }

    public async Task<bool> BulkInsertAsync(List<QuestionChoiceDto> dtos)
    {
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v2/QuestionChoices");
        return await api.BulkInsertAsync(dtos);
    }

    public async Task<bool> UpdateAsync(QuestionChoiceDto dto)
    {

        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        return await api.UpdateAsync(dto);
    }

    public async Task<bool> BulkUpdateAsync(List<QuestionChoiceDto> dtos)
    {
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v2/QuestionChoices");
        return await api.BulkUpdateAsync(dtos);
    }

    public async Task<bool> DeleteAsync(QuestionChoiceDto dto)
    {
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        return await api.DeleteAsync(dto);
    }

    public async Task<bool> BulkDeleteAsync(List<QuestionChoiceDto> dtos)
    {
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v2/QuestionChoices");
        return await api.BulkDeleteAsync(dtos);
    }

    public async Task<QuestionChoiceDto> GetByIdAsync(int id)
    {
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        return await api.GetByIdAsync(id);
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAllAsync()
    {
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        return await api.GetAllAsync();
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAsync(Expression<Func<QuestionChoiceDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v3/QuestionChoices");
        return await api.GetAsync(content);
    }

    public async Task<bool> DeleteAsync(Expression<Func<QuestionChoiceDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        //return await api.DeleteAsync(content);
    }
}