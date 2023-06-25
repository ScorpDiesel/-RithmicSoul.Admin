using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Dtos;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Survey;
using RithmicSoul.Models.Survey.Models;
using Serialize.Linq.Serializers;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Admin.Application.Interfaces.Services.Survey;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AuthoredSurveyService : IDatabaseService<AuthoredSurveyDto>
{
    private readonly AppSetting _appSetting;
    private readonly ISurveyRepository<AuthoredSurveyDto> _adminRepository;

    public AuthoredSurveyService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<ISurveyRepository<AuthoredSurveyDto>>(_appSetting.BaseAddress);
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetAllFromViewAsync()
    {
        return await _adminRepository.GetAllFromViewAsync();
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetFromViewAsync(Expression<Func<AuthoredSurveyDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetFromViewAsync(content);
    }

    public async Task<dynamic> ExecuteQueryStoredProcedureAsync(StoredProcedureRequest spRequest)
    {
        return await _adminRepository.ExecuteQueryStoredProcedureAsync(spRequest);
    }
}