using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Models;

namespace RithmicSoul.Admin.Core.Interfaces;

public interface IDraftSurveyService
{
    Task<bool> SaveAsync(DraftSurveyDto draftSurveyDto);
    Task<List<DraftSurveyDto>> GetAllAsync();
    Task<DraftSurveyDto?> GetByIdAsync(int id);
}