using System.Xml;
using AutoMapper;
using DAL.Models;
using DTOs;

namespace BLL.Infrastracture
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<TestModel, TestDto>()
                .ForMember(x => x.Count, opt => opt.MapFrom(y => y.Total))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Title))
                .ReverseMap();

            CreateMap<PartnersDto.Add, Partners>()
                .ForMember(model => model.Total, opt => opt.MapFrom(dto => dto.Count))
                .ReverseMap();
        }
    }
}
