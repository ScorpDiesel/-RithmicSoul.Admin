using AutoMapper;
using RithmicSoul.Admin.Application.Dtos;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Infrastructure.Mapper;

public class SurveyResponseProfile : Profile
{
    public SurveyResponseProfile()
    {
        CreateMap<SurveyResponse, SurveyResponseDto>()
            .ForMember(d => d.QuestionText, opt => opt.MapFrom(src => src.SurveyQuestion.QuestionText))
            .ForMember(d => d.SurveyName, opt => opt.MapFrom(src => src.SurveyMetaData.SurveyName))
            .ReverseMap();
        CreateMap<DraftSurveyDto, SurveyResponseDto>().ReverseMap();
    }
}