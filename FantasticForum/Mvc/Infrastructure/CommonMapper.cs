using AutoMapper;
using Models;
using Mvc.ViewModels;
using System;

namespace Mvc.Infrastructure
{
    public class CommonMapper : IMapper
    {
        public CommonMapper()
        {
            Mapper.CreateMap<Section, SectionViewModel>();
            Mapper.CreateMap<Topic, TopicViewModel>()
                  .ForMember(dest => dest.RecordCount, opt => opt.MapFrom(src => src.Records.Count));
            Mapper.CreateMap<RegisterViewModel, User>();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}