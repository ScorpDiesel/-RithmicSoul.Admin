using System.Linq.Expressions;
using RithmicSoul.Admin.Core.Interfaces;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using System.Net.Http.Json;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AuthoredSurveyService : IDatabaseService<AuthoredSurveyDto>
{
    private readonly HttpClient _httpClient;

    public AuthoredSurveyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetAllFromViewAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AuthoredSurveyDto>>("v1/AuthoredSurveys");
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetFromViewAsync(Expression<Func<AuthoredSurveyDto, bool>> expression)
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

    async Task<bool> IDatabaseService<AuthoredSurveyDto>.ExecuteStoredProcedure(string sprocName, object? sqlParams)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> ExecuteQueryStoredProcedureAsync(string sprocName, object sqlParams = null)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/ProductSurveys", sqlParams);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<IEnumerable<AuthoredSurveyDto>>();
    }
}