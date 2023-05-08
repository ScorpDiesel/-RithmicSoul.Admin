using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Client.Services;

public class QuestionChoiceService : IService<QuestionChoiceDto>
{
    private readonly HttpClient _httpClient;

    public QuestionChoiceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(QuestionChoiceDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionChoice/Insert", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkInsertAsync(List<QuestionChoiceDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionChoice/BulkInsert", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(QuestionChoiceDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionChoice/Update", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<QuestionChoiceDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionChoice/BulkUpdate", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(QuestionChoiceDto dto)
    {
        return await DeleteAsync(dto.ChoiceId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetAsync($"QuestionChoice/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<QuestionChoiceDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionChoice/BulkDelete", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<QuestionChoiceDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<QuestionChoiceDto>($"QuestionChoice/GetById/{id}");
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ObservableCollection<QuestionChoiceDto>>("QuestionChoice/GetAll");
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAsync(Expression<Func<QuestionChoiceDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("QuestionChoice/Get", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<QuestionChoiceDto>>();
    }
}