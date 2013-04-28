using System;

namespace Mvc.Infrastructure
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType); 
    }
}