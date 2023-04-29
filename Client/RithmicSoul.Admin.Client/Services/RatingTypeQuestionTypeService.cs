using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using RithmicSoul.Models.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Client.Services;

public class RatingTypeQuestionTypeService : IService<RatingTypeQuestionTypeDto>
{
    private readonly HttpClient _httpClient;

    public RatingTypeQuestionTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(RatingTypeQuestionTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("RatingTypeQuestionType/Insert", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(RatingTypeQuestionTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("RatingTypeQuestionType/Update", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(RatingTypeQuestionTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("RatingTypeQuestionType/Delete", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<HttpResponseMessage>($"RatingTypeQuestionType/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<RatingTypeQuestionTypeDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<RatingTypeQuestionTypeDto>($"RatingTypeQuestionType/GetById/{id}");
    }

    public async Task<IEnumerable<RatingTypeQuestionTypeDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ObservableCollection<RatingTypeQuestionTypeDto>>("RatingTypeQuestionType/GetAll");
    }

    public async Task<IEnumerable<RatingTypeQuestionTypeDto>> GetAsync(Expression<Func<RatingTypeQuestionTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var response = await _httpClient.PostAsJsonAsync("RatingTypeQuestionType/Get", serializedExpression);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<RatingTypeQuestionTypeDto>>();
    }
}