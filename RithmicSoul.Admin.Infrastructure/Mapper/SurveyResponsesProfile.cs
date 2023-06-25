using AutoMapper;
using RithmicSoul.Admin.Application.Dtos;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Infrastructure.Mapper;

public class SurveyResponseProfile : Profile
{
    public SurveyResponseProfile()
    {
        CreateMap<SurveyResponse, SurveyResponseDto>().ReverseMap();
        CreateMap<DraftSurveyDto, SurveyResponseDto>().ReverseMap();
    }
}