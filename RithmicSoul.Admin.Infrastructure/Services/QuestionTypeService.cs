using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class QuestionTypeService : IService<QuestionTypeDto>
{
    private readonly HttpClient _httpClient;

    public QuestionTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(QuestionTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/QuestionTypes", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(QuestionTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/QuestionTypes", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<QuestionTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("v2/QuestionTypes", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(QuestionTypeDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("v1/QuestionTypes", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<QuestionTypeDto> dtos)
    {
        var response = await _httpClient.PutAsJsonAsync("v2/QuestionTypes", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(QuestionTypeDto dto)
    {
        return await DeleteAsync(dto.QuestionTypeId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"v1/QuestionTypes/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<QuestionTypeDto> dtos)
    {
        var response = await _httpClient.DeleteAsJsonAsync("QuestionType/BulkDelete", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<QuestionTypeDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<QuestionTypeDto>($"v1/QuestionTypes/{id}");
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<QuestionTypeDto>>("v1/QuestionTypes");
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAsync(Expression<Func<QuestionTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("v3/QuestionTypes", content);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<IEnumerable<QuestionTypeDto>>();
    }

    public async Task<bool> DeleteAsync(Expression<Func<QuestionTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v2/QuestionTypes", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAllFromViewAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetFromViewAsync(Expression<Func<QuestionTypeDto, bool>> expression)
    {
        throw new NotImplementedException();
    }
}