using System.Linq.Expressions;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;
using RithmicSoul.Models.Survey.Dtos;
using Serialize.Linq.Serializers;
using RithmicSoul.Admin.Core.Models;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Services;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyResponseService : IService<SurveyResponseDto>
{
    private readonly ISurveyResponseRepository _surveyResponseRepository;
    private readonly AppSetting _appSetting;

    public SurveyResponseService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _surveyResponseRepository = RestService.For<ISurveyResponseRepository>(_appSetting.BaseAddress);
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetAllAsync()
    {
        return await _surveyResponseRepository.GetAllResponsesAsync();
    }

    public async Task<bool> DeleteAsync(Expression<Func<SurveyResponseDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var response = await _surveyResponseRepository.DeleteResponsesAsync(content);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetByIdAsync(object id)
    {
        return await _surveyResponseRepository.GetResponsesByIdAsync((Guid)id);
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetAsync(Expression<Func<SurveyResponseDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _surveyResponseRepository.GetResponsesAsync(content);
    }

    public async Task<bool> InsertAsync(SurveyResponseDto dto)
    {
        var response = await _surveyResponseRepository.InsertResponsesAsync(dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyResponseDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(SurveyResponseDto dto)
    {
        var response = await _surveyResponseRepository.UpdateResponsesAsync(dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyResponseDto dto)
    {
        var response = await _surveyResponseRepository.DeleteResponsesAsync(dto);
        return response.IsSuccessStatusCode;
    }
}