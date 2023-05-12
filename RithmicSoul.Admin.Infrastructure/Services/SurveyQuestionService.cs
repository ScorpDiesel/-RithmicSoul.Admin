using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
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
        throw new NotImplementedException();
    }

    public async Task<object> InsertForIdAsync(SurveyQuestionDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestion/Insert", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkInsertAsync(List<SurveyQuestionDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestion/BulkInsert", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyQuestionDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestion/Update", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyQuestionDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestion/BulkUpdate", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyQuestionDto dto)
    {
        return await DeleteAsync(dto.QuestionId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetAsync($"SurveyQuestion/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyQuestionDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestion/BulkDelete", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<SurveyQuestionDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyQuestionDto>($"SurveyQuestion/GetById/{id}");
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ObservableCollection<SurveyQuestionDto>>("SurveyQuestion/GetAll");
    }

    public async Task<IEnumerable<SurveyQuestionDto>> GetAsync(Expression<Func<SurveyQuestionDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestion/Get", serializedExpression);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyQuestionDto>>();
    }
}