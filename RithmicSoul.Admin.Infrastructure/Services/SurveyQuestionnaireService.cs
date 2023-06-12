using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyQuestionnaireService : IService<SurveyQuestionnaireDto>
{
    private readonly HttpClient _httpClient;

    public SurveyQuestionnaireService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(SurveyQuestionnaireDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/SurveyQuestionnaires", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionnaireDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/SurveyQuestionnaires", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("v2/SurveyQuestionnaires", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyQuestionnaireDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("v1/SurveyQuestionnaires", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _httpClient.PutAsJsonAsync("v2/SurveyQuestionnaires", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyQuestionnaireDto dto)
    {
        return await DeleteAsync(dto.QuestionnaireId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"v1/SurveyQuestionnaires/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _httpClient.DeleteAsJsonAsync("v2/SurveyQuestionnaires", dtos);
        return response.IsSuccessStatusCode;
        return false;
    }

    public async Task<SurveyQuestionnaireDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyQuestionnaireDto>($"v1/SurveyQuestionnaires/{id}");
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SurveyQuestionnaireDto>>("v1/SurveyQuestionnaires");
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("v3/SurveyQuestionnaires", content);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyQuestionnaireDto>>();
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v2/SurveyQuestionnaires", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAllFromViewAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetFromViewAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        throw new NotImplementedException();
    }
}