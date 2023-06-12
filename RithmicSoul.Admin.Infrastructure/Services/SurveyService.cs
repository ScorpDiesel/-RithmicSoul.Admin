using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyService : IService<SurveyDto>
{
    private readonly HttpClient _httpClient;

    public SurveyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(SurveyDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/Surveys", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/Surveys", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<SurveyDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("v2/Surveys", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("v1/Surveys", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyDto> dtos)
    {
        var response = await _httpClient.PutAsJsonAsync("v2/Surveys", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyDto dto)
    {
        return await DeleteAsync(dto.SurveyId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"v1/Surveys/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyDto> dtos)
    {
        var response = await _httpClient.DeleteAsJsonAsync("v2/Surveys", dtos);
        return response.IsSuccessStatusCode;
        return false;
    }

    public async Task<SurveyDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyDto>($"v1/Surveys/{id}");
    }

    public async Task<IEnumerable<SurveyDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SurveyDto>>("v1/Surveys");
    }

    public async Task<IEnumerable<SurveyDto>> GetAsync(Expression<Func<SurveyDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("v1/Surveys", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyDto>>();
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v2/Surveys", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Delete failed");
        return response.IsSuccessStatusCode;
        return false;
    }
}