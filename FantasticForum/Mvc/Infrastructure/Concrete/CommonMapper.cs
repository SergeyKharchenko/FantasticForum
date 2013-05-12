using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.ViewModels;
using PagedList;

namespace Mvc.Infrastructure.Concrete
{
    public class CommonMapper : IMapper
    {
        public CommonMapper()
        {
            Mapper.CreateMap<Section, SectionViewModel>();
            Mapper.CreateMap<Topic, TopicViewModel>()
                  .ForMember(dest => dest.RecordCount, opt => opt.MapFrom(src => src.Records.Count));
            Mapper.CreateMap<RegisterViewModel, User>();

            Mapper.CreateMap<Record, RecordViewModel>()
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));
        }

        public class Shit
        {
            public IPagedList<RecordViewModel> P { get; set; }
            public IEnumerable<RecordViewModel> R { get; set; }
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }
}