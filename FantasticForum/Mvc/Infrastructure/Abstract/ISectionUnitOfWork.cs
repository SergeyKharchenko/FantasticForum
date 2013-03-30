using System.Collections.Generic;
using Models;

namespace Mvc.Infrastructure.Abstract
{
    public interface ISectionUnitOfWork : IUnitOfWork
    {
        IEnumerable<Section> Section { get; }
    }
}