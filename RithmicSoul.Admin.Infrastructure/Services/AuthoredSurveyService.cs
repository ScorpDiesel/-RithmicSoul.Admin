using System.Linq.Expressions;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using System.Net.Http.Json;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AuthoredSurveyService : IService<AuthoredSurveyDto>
{
    private readonly HttpClient _httpClient;

    public AuthoredSurveyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AuthoredSurveyDto>>("v1/AuthoredSurveys");
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetAsync(Expression<Func<AuthoredSurveyDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var content = new StringContent(serializedExpression);
        var response = await _httpClient.PostAsync("v1/AuthoredSurveys", content);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<IEnumerable<AuthoredSurveyDto>>();
    }

    public async Task<AuthoredSurveyDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Expression<Func<AuthoredSurveyDto, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetAllFromViewAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetFromViewAsync(Expression<Func<AuthoredSurveyDto, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> InsertAsync(AuthoredSurveyDto item)
    {
        throw new NotImplementedException();
    }

    public async Task<object> InsertForIdAsync(AuthoredSurveyDto item)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> BulkInsertAsync(List<AuthoredSurveyDto> items)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(AuthoredSurveyDto item)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> BulkUpdateAsync(List<AuthoredSurveyDto> items)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(AuthoredSurveyDto item)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> BulkDeleteAsync(List<AuthoredSurveyDto> items)
    {
        throw new NotImplementedException();
    }
}