using System.Linq.Expressions;
using RithmicSoul.Models.Survey.Dtos;
using Refit;
using RithmicSoul.Admin.Application.Interfaces;
using RithmicSoul.Models.Survey.Models;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class AuthoredSurveyService : ISurveyService<AuthoredSurveyDto>
{
    public async Task<IEnumerable<AuthoredSurveyDto>> GetAllFromViewAsync()
    {
        var api = RestService.For<ISurveyRepository<AuthoredSurveyDto>>("v1/AuthoredSurveys");
        return await api.GetAllFromViewAsync();
    }

    public async Task<IEnumerable<AuthoredSurveyDto>> GetFromViewAsync(Expression<Func<AuthoredSurveyDto, bool>> expression)
    {
        var api = RestService.For<ISurveyRepository<AuthoredSurveyDto>>("v1/AuthoredSurveys");
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await api.GetFromViewAsync(content);
    }

    public async Task<dynamic> ExecuteQueryStoredProcedureAsync(StoredProcedureRequest spRequest)
    {
        var api = RestService.For<ISurveyRepository<AuthoredSurveyDto>>("v2/AuthoredSurveys");
        return await api.ExecuteQueryStoredProcedureAsync(spRequest);
    }
}