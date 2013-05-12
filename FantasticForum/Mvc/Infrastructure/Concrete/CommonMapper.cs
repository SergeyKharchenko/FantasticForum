using System;
using System.Collections.Generic;
using System.Linq;
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
            Mapper.CreateMap<RegisterViewModel, User>();

            Mapper.CreateMap<Record, RecordViewModel>()
                  .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
            Mapper.CreateMap<Tuple<int, int, IEnumerable<Record>>, RecordsViewModel>()
                  .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.Item1))
                  .ForMember(dest => dest.TopicId, opt => opt.MapFrom(src => src.Item2))
                  .ForMember(dest => dest.Records, opt => opt.MapFrom(src => from record in src.Item3
                                                                             select Map<Record, RecordViewModel>(record)));
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }
}