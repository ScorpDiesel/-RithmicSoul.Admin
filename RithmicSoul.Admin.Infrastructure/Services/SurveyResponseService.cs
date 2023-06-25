using System.Linq.Expressions;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using Serialize.Linq.Serializers;
using AutoMapper;
using RithmicSoul.Admin.Application.Interfaces.Services.Survey;
using RithmicSoul.Admin.Core.Models;
using Refit;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyResponseService : ISurveyResponseService
{
    private readonly ISurveyResponseRepository _surveyResponseRepository;
    private readonly AppSetting _appSetting;
    private readonly IMapper _mapper;

    public SurveyResponseService(AppSetting appSetting, IMapper _mapper)
    {
        _appSetting = appSetting;
        _surveyResponseRepository = RestService.For<ISurveyResponseRepository>(_appSetting.BaseAddress);
        this._mapper = _mapper;
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetAllResponsesAsync()
    {
        var items = await _surveyResponseRepository.GetAllResponsesAsync();
        return _mapper.Map<IEnumerable<SurveyResponseDto>>(items);
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetResponsesByIdAsync(Guid id)
    {
        var item = await _surveyResponseRepository.GetResponsesByIdAsync(id);
        return _mapper.Map<IEnumerable<SurveyResponseDto>>(item);
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetResponsesAsync(Expression<Func<SurveyResponseDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var items = await _surveyResponseRepository.GetResponsesAsync(content);
        return _mapper.Map<IEnumerable<SurveyResponseDto>>(items);
    }

    public async Task<bool> InsertResponsesAsync(SurveyResponseDto dto)
    {
        var item = _mapper.Map<SurveyResponse>(dto);
        var response = await _surveyResponseRepository.InsertResponsesAsync(item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateResponsesAsync(SurveyResponseDto dto)
    {
        var item = _mapper.Map<SurveyResponse>(dto);
        var response = await _surveyResponseRepository.UpdateResponsesAsync(item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteResponsesAsync(SurveyResponseDto dto)
    {
        var item = _mapper.Map<SurveyResponse>(dto);
        var response = await _surveyResponseRepository.DeleteResponsesAsync(item);
        return response.IsSuccessStatusCode;
    }
}