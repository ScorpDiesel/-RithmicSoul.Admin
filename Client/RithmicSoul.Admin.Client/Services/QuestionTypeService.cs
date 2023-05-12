using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using RithmicSoul.Admin.Client.Logging;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Client.Services;

public class QuestionTypeService : IService<QuestionTypeDto>
{
    private readonly HttpClient _httpClient;
    //private readonly ConsoleRedirectLogger<QuestionTypeService> _logger;

    public QuestionTypeService(HttpClient httpClient /*ConsoleRedirectLogger<QuestionTypeService> logger*/)
    {
        _httpClient = httpClient;
        //_logger = logger;
    }

    public async Task<bool> InsertAsync(QuestionTypeDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<object> InsertForIdAsync(QuestionTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/Insert", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkInsertAsync(List<QuestionTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/BulkInsert", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(QuestionTypeDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/Update", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkUpdateAsync(List<QuestionTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/BulkUpdate", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(QuestionTypeDto dto)
    {
        return await DeleteAsync(dto.QuestionTypeId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.GetAsync($"QuestionType/DeleteById/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<QuestionTypeDto> dtos)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/BulkDelete", dtos);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkDeleteAsync(List<int> ids)
    {
        var response = await _httpClient.PostAsJsonAsync("QuestionType/BulkDelete", ids);
        return response.IsSuccessStatusCode;
    }

    public async Task<QuestionTypeDto> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<QuestionTypeDto>($"QuestionType/GetById/{id}");
    }

    public async Task<IEnumerable<QuestionTypeDto>> GetAllAsync()
    {
        IEnumerable<QuestionTypeDto> result = null;
        try
        {
            result = await _httpClient.GetFromJsonAsync<IEnumerable<QuestionTypeDto>>("QuestionType/GetAll");
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, ex.Message, ex.StackTrace);
        }

        return result;
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