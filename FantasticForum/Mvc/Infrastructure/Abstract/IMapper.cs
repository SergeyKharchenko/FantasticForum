using System;

namespace Mvc.Infrastructure.Abstract
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType); 
    }
}