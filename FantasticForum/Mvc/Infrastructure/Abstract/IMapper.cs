using System;

namespace Mvc.Infrastructure.Abstract
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}