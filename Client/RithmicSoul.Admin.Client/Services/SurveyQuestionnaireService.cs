using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Client.Services;

public class SurveyQuestionnaireService : IService<SurveyQuestionnaireDto>
{
    private readonly HttpClient _httpClient;

    public SurveyQuestionnaireService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InsertAsync(SurveyQuestionnaireDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/Insert", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(SurveyQuestionnaireDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/Update", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyQuestionnaireDto item)
    {
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/Delete", item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<HttpResponseMessage>($"SurveyQuestionnaire/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<SurveyQuestionnaireDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<SurveyQuestionnaireDto>($"SurveyQuestionnaire/GetById/{id}");
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ObservableCollection<SurveyQuestionnaireDto>>("SurveyQuestionnaire/GetAll");
    }

    public async Task<IEnumerable<SurveyQuestionnaireDto>> GetAsync(Expression<Func<SurveyQuestionnaireDto, bool>> expression)
    {
        // Serialize the expression
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);

        // Send the serialized expression as an HTTP request
        var response = await _httpClient.PostAsJsonAsync("SurveyQuestionnaire/Get", serializedExpression);
        if (!response.IsSuccessStatusCode) throw new Exception("Failed to get response");
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<SurveyQuestionnaireDto>>();
    }
}