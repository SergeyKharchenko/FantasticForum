using System;
using AutoMapper;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.ViewModels;

namespace Mvc.Infrastructure.Concrete
{
    public class CommonMapper : IMapper
    {
        static CommonMapper()
        {
            Mapper.CreateMap<Section, SectionViewModel>();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}