using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;
using System.Globalization;
using RithmicSoulSharedLibrary.Extensions;

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
        var response = await _httpClient.PostAsJsonAsync("v1/AdinkraSymbols", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(AdinkraSymbolDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/AdinkraSymbols", dto);
        response = response.EnsureSuccessStatusCode();
        var responseObj = response.Content.ReadAsStringAsync();
        return responseObj.Result;
    }

    public async Task<bool> BulkInsertAsync(List<AdinkraSymbolDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("v2/AdinkraSymbols", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(AdinkraSymbolDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("v1/AdinkraSymbols", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<AdinkraSymbolDto> dtos)
    {
        var response = await _httpClient.PutAsJsonAsync("v2/AdinkraSymbols", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(AdinkraSymbolDto dto)
    {
        return await DeleteAsync(dto.SymbolId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"v1/AdinkraSymbols/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<AdinkraSymbolDto> dtos)
    {
        var response = await _httpClient.DeleteAsJsonAsync("v2/AdinkraSymbols", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<AdinkraSymbolDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<AdinkraSymbolDto>($"v1/AdinkraSymbols/{id}");
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAllAsync()
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        return await _httpClient.GetFromJsonAsync<IEnumerable<AdinkraSymbolDto>>("v1/AdinkraSymbols");
    }

    public async Task<IEnumerable<AdinkraSymbolDto>> GetAsync(Expression<Func<AdinkraSymbolDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v2/AdinkraSymbols", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");

        return await response.Content.ReadFromJsonAsync<IEnumerable<AdinkraSymbolDto>>();
        return null;
    }

    public async Task<bool> DeleteAsync(Expression<Func<AdinkraSymbolDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.DeleteAsJsonAsync("v2/AdinkraSymbols", content);
        if (!response.IsSuccessStatusCode) throw new Exception("Delete failed");
        return response.IsSuccessStatusCode;
        return false;
    }
}