using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
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
        throw new NotImplementedException();
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionnaireDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/Insert", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/BulkInsert", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyQuestionnaireDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/Update", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/BulkUpdate", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyQuestionnaireDto dto)
    {
        return await DeleteAsync(dto.QuestionnaireId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetAsync($"SurveyQuestionnaire/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionnaireDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/BulkDelete", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<SurveyQuestionnaireDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyQuestionnaireDto>($"SurveyQuestionnaire/GetById/{id}");
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SurveyQuestionnaireDto>>("SurveyQuestionnaire/GetAll");
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/Get", serializedExpression);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyQuestionnaireDto>>();
    }
}