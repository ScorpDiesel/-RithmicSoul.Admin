using RithmicSoul.Admin.Core.Dtos;
using RithmicSoul.Admin.Core.Models;

namespace RithmicSoul.Admin.Core.Interfaces;

public interface IAuthoredSurveyService
{
    Task<bool> SaveAsync(AuthoredSurvey survey);
    Task<List<AuthoredSurveyDto>> GetAllAsync();
}