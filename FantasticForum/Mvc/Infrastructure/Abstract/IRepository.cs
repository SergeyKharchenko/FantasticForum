using System.Collections.Generic;
using Models;

namespace Mvc.Infrastructure.Abstract
{
    public interface IRepository<out TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> Entities { get; }
    }
}