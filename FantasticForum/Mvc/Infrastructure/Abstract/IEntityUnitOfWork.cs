using System.Collections.Generic;
using Models;

namespace Mvc.Infrastructure.Abstract
{
    public interface IEntityUnitOfWork : IUnitOfWork
    {
        IEnumerable<Entity> Topics { get; }
    }
}