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
                  .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));
            Mapper.CreateMap<Tuple<int, int, int, int, IEnumerable<Record>>, RecordsViewModel>()
                  .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.Item1))
                  .ForMember(dest => dest.TopicId, opt => opt.MapFrom(src => src.Item2))
                  .AfterMap((src, dest) => dest.Records = (from record in src.Item5
                                                           select Map<Record, RecordViewModel>(record)).ToPagedList
                                                              (src.Item3, src.Item4));
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