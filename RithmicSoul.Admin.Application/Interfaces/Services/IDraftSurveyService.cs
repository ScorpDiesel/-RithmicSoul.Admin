using RithmicSoul.Admin.Application.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces.Services;

public interface IDraftSurveyService
{
    Task<bool> SaveAsync(DraftSurveyDto draftSurveyDto);
    Task<List<DraftSurveyDto>> GetAllAsync();
    Task<DraftSurveyDto?> GetByIdAsync(int id);
}