using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using RithmicSoul.Models.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Client.Services;

public class QuestionTypeService : IService<QuestionTypeDto>
{
    private readonly HttpClient _httpClient;

    public QuestionTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(QuestionTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/Insert", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(QuestionTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/Update", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(QuestionTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/Delete", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<HttpResponseMessage>($"QuestionType/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<QuestionTypeDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<QuestionTypeDto>($"QuestionType/GetById/{id}");
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ObservableCollection<QuestionTypeDto>>("QuestionType/GetAll");
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAsync(Expression<Func<QuestionTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var response = await _httpClient.PostAsJsonAsync("QuestionType/Get", serializedExpression);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<QuestionTypeDto>>();
    }
}