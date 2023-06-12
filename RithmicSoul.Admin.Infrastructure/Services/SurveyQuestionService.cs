using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyQuestionService : IService<SurveyQuestionDto>
{
    private readonly HttpClient _httpClient;

    public SurveyQuestionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(SurveyQuestionDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/SurveyQuestions", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/SurveyQuestions", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("v2/SurveyQuestions", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyQuestionDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("v1/SurveyQuestions", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionDto> dtos)
    {
        var response = await _httpClient.PutAsJsonAsync("v2/SurveyQuestions", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyQuestionDto dto)
    {
        return await DeleteAsync(dto.QuestionId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"v1/SurveyQuestions/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionDto> dtos)
    {
        var response = await _httpClient.DeleteAsJsonAsync("v2/SurveyQuestions", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<SurveyQuestionDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyQuestionDto>($"v1/SurveyQuestions/{id}");
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SurveyQuestionDto>>("v1/SurveyQuestions");
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAsync(Expression<Func<SurveyQuestionDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("v2/SurveyQuestions", content);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyQuestionDto>>();
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyQuestionDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v2/SurveyQuestions", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAllFromViewAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetFromViewAsync(Expression<Func<SurveyQuestionDto, bool>> expression)
    {
        throw new NotImplementedException();
    }
}