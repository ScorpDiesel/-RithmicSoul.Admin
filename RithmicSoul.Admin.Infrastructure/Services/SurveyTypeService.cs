using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyTypeService : IService<SurveyTypeDto>
{
    private readonly HttpClient _httpClient;

    public SurveyTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(SurveyTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/SurveyTypes", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/SurveyTypes", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("v2/SurveyTypes", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyTypeDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("v1/SurveyTypes", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _httpClient.PutAsJsonAsync("v2/SurveyTypes", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyTypeDto dto)
    {
        return await DeleteAsync(dto.SurveyTypeId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"v1/SurveyTypes/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _httpClient.DeleteAsJsonAsync("v2/SurveyTypes", dtos);
        return response.IsSuccessStatusCode;
        return false;
    }

    public async Task<SurveyTypeDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyTypeDto>($"v1/SurveyTypes/{id}");
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SurveyTypeDto>>("v1/SurveyTypes");
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAsync(Expression<Func<SurveyTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("v2/SurveyTypes", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyTypeDto>>();
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v1/SurveyTypes", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Delete failed");
        return response.IsSuccessStatusCode;
        return false;
    }
}