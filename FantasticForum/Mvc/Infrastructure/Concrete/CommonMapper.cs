using System;
using AutoMapper;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.ViewModels;

namespace Mvc.Infrastructure.Concrete
{
    public class CommonMapper : IMapper
    {
        public CommonMapper()
        {
            Mapper.CreateMap<Section, SectionViewModel>();
            Mapper.CreateMap<Topic, TopicViewModel>()
                  .ForMember(dest => dest.RecordCount, opt => opt.MapFrom(src => src.Records.Count));
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}