using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces.Services.Survey;

public interface ISurveyResponseService
{
    Task<IEnumerable<SurveyResponseDto>> GetAllResponsesAsync();
    Task<IEnumerable<SurveyResponseDto>> GetResponsesByIdAsync(Guid id);
    Task<IEnumerable<SurveyResponseDto>> GetResponsesAsync(Expression<Func<SurveyResponseDto, bool>> expression);
    Task<bool> InsertResponseAsync(SurveyResponseDto dto);
    Task<object> InsertResponseForIdAsync(SurveyResponseDto dto);
    Task<bool> UpdateResponseAsync(SurveyResponseDto dto);
    Task<bool> DeleteResponseAsync(SurveyResponseDto dto);
    Task<bool> DeleteResponsesAsync(Expression<Func<SurveyResponseDto, bool>> expression);
}