using System.Linq.Expressions;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;
using Serialize.Linq.Serializers;
using AutoMapper;
using RithmicSoul.Admin.Application.Interfaces.Services.Survey;
using RithmicSoul.Admin.Core.Models;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Services;
using System;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class SurveyResponseService : IService<SurveyResponseDto>
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

    public async Task<IEnumerable<SurveyResponseDto>> GetAllAsync()
    {
        var items = await _surveyResponseRepository.GetAllResponsesAsync();
        return _mapper.Map<IEnumerable<SurveyResponseDto>>(items);
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
        var item = await _surveyResponseRepository.GetResponsesByIdAsync((Guid)id);
        return _mapper.Map<IEnumerable<SurveyResponseDto>>(item);
    }

    public async Task<IEnumerable<SurveyResponseDto>> GetAsync(Expression<Func<SurveyResponseDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        var items = await _surveyResponseRepository.GetResponsesAsync(content);
        return _mapper.Map<IEnumerable<SurveyResponseDto>>(items);
    }

    public async Task<bool> InsertAsync(SurveyResponseDto dto)
    {
        var item = _mapper.Map<SurveyResponse>(dto);
        var response = await _surveyResponseRepository.InsertResponsesAsync(item);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(SurveyResponseDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(SurveyResponseDto dto)
    {
        var item = _mapper.Map<SurveyResponse>(dto);
        var response = await _surveyResponseRepository.UpdateResponsesAsync(item);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(SurveyResponseDto dto)
    {
        var item = _mapper.Map<SurveyResponse>(dto);
        var response = await _surveyResponseRepository.DeleteResponsesAsync(item);
        return response.IsSuccessStatusCode;
    }
}