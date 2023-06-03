using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
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
        var response = await _httpClient.PostAsJsonAsync("SurveyType/Insert", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyType/Insert", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyType/BulkInsert", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyType/Update", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyType/BulkUpdate", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyTypeDto dto)
    {
        return await DeleteAsync(dto.SurveyTypeId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetAsync($"SurveyType/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<SurveyTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyType/BulkDelete", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<SurveyTypeDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyTypeDto>($"SurveyType/GetById/{id}");
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SurveyTypeDto>>("SurveyType/GetAll");
    }

    public async Task<IEnumerable<SurveyTypeDto>> GetAsync(Expression<Func<SurveyTypeDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var response = await _httpClient.PostAsJsonAsync("SurveyType/Get", serializedExpression);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyTypeDto>>();
    }
}