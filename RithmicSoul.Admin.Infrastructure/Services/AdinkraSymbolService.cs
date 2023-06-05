using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;
using System.Globalization;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AdinkraSymbolService : IService<AdinkraSymbolDto>
{
    private readonly HttpClient _httpClient;

    public AdinkraSymbolService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(AdinkraSymbolDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("AdinkraSymbol/Insert", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(AdinkraSymbolDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("AdinkraSymbol/Insert", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<AdinkraSymbolDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("AdinkraSymbol/BulkInsert", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(AdinkraSymbolDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("AdinkraSymbol/Update", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<AdinkraSymbolDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("AdinkraSymbol/BulkUpdate", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(AdinkraSymbolDto dto)
    {
        return await DeleteAsync(dto.SymbolId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetAsync($"AdinkraSymbol/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<AdinkraSymbolDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("AdinkraSymbol/BulkDelete", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<AdinkraSymbolDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<AdinkraSymbolDto>($"AdinkraSymbol/GetById/{id}");
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAllAsync()
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        return await _httpClient.GetFromJsonAsync<IEnumerable<AdinkraSymbolDto>>("AdinkraSymbol/GetAll");
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAsync(Expression<Func<AdinkraSymbolDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("AdinkraSymbol/Get", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<AdinkraSymbolDto>>();
    }
}