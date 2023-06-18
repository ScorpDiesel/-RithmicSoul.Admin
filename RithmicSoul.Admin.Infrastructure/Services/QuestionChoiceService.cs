using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class QuestionChoiceService : IService<QuestionChoiceDto>
{
    private readonly HttpClient _httpClient;

    public QuestionChoiceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(QuestionChoiceDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/QuestionChoices", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(QuestionChoiceDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/QuestionChoices", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<QuestionChoiceDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("v2/QuestionChoices", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(QuestionChoiceDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("v1/QuestionChoices", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<QuestionChoiceDto> dtos)
    {
        var response = await _httpClient.PutAsJsonAsync("v2/QuestionChoices", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(QuestionChoiceDto dto)
    {
        var response = await _httpClient.DeleteAsJsonAsync($"v1/QuestionChoices", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"v1/QuestionChoices/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<QuestionChoiceDto> dtos)
    {
        var response = await _httpClient.DeleteAsJsonAsync("v2/QuestionChoices", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<QuestionChoiceDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<QuestionChoiceDto>($"v1/QuestionChoices/{id}");
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<QuestionChoiceDto>>("v1/QuestionChoices");
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAsync(Expression<Func<QuestionChoiceDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("v3/QuestionChoices", content);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<IEnumerable<QuestionChoiceDto>>();
    }

    public async Task<bool> DeleteAsync(Expression<Func<QuestionChoiceDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v2/QuestionChoices", content);
        return response.IsSuccessStatusCode;
    }
}