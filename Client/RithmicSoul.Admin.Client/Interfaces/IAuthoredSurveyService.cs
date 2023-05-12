using RithmicSoul.Admin.Client.Dtos;
using RithmicSoul.Admin.Client.Models;

namespace RithmicSoul.Admin.Client.Interfaces;

public interface IAuthoredSurveyService
{
    Task<bool> SaveAsync(AuthoredSurvey survey);
    Task<List<AuthoredSurveyDto>> GetAllAsync();
}