using System.Linq.Expressions;
using Refit;
using RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;
using RithmicSoul.Admin.Application.Interfaces.Services.Admin;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Models.Survey.Dtos;
using Serialize.Linq.Serializers;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class QuestionChoiceService : IAdminService<QuestionChoiceDto>
{
    private readonly AppSetting _appSetting;
    private readonly IAdminRepository<QuestionChoiceDto> _adminRepository;
    private const string RouteSuffix = "QuestionChoices";

    public QuestionChoiceService(AppSetting appSetting)
    {
        _appSetting = appSetting;
        _adminRepository = RestService.For<IAdminRepository<QuestionChoiceDto>>(_appSetting.BaseAddress);
    }

    public async Task<bool> InsertAsync(QuestionChoiceDto dto)
    {
        var response = await _adminRepository.InsertAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<object> InsertForIdAsync(QuestionChoiceDto dto)
    {
        return await _adminRepository.InsertForIdAsync(dto, RouteSuffix);
    }
    public async Task<bool> UpdateAsync(QuestionChoiceDto dto)
    {
        var response = await _adminRepository.UpdateAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(QuestionChoiceDto dto)
    {
        var response = await _adminRepository.DeleteAsync(dto, RouteSuffix);
        return response.IsSuccessStatusCode;
    }

    public async Task<QuestionChoiceDto> GetByIdAsync(int id)
    {
        return await _adminRepository.GetByIdAsync(id, RouteSuffix);
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAllAsync()
    {
        return await _adminRepository.GetAllAsync(RouteSuffix);
    }

    public async Task<IEnumerable<QuestionChoiceDto>> GetAsync(Expression<Func<QuestionChoiceDto, bool>> expression)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var serializedExpression = serializer.SerializeText(expression);
        var content = new StringContent(serializedExpression);
        return await _adminRepository.GetAsync(content, RouteSuffix);
    }

    public async Task<bool> DeleteAsync(Expression<Func<QuestionChoiceDto, bool>> expression)
    {
        throw new NotImplementedException();
        //var serializer = new ExpressionSerializer(new JsonSerializer());
        //var serializedExpression = serializer.SerializeText(expression);
        //var content = new StringContent(serializedExpression);
        //var api = RestService.For<IAdminRepository<QuestionChoiceDto>>("v1/QuestionChoices");
        //return await api.DeleteAsync(content);
    }
}