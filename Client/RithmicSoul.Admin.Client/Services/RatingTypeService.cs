using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Client.Services;

public class RatingTypeService : IService<RatingTypeDto>
{
    private readonly HttpClient _httpClient;

    public RatingTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(RatingTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("RatingType/Insert", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(RatingTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("RatingType/Update", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(RatingTypeDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("RatingType/Delete", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<HttpResponseMessage>($"RatingType/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<RatingTypeDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<RatingTypeDto>($"RatingType/GetById/{id}");
    }

    public async Task<IEnumerable<RatingTypeDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ObservableCollection<RatingTypeDto>>("RatingType/GetAll");
    }

    public async Task<IEnumerable<RatingTypeDto>> GetAsync(Expression<Func<RatingTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var response = await _httpClient.PostAsJsonAsync("RatingType/Get", serializedExpression);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<RatingTypeDto>>();
    }
}