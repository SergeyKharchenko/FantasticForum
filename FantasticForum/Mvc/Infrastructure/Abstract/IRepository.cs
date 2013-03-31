using System.Collections.Generic;
using Models;

namespace Mvc.Infrastructure.Abstract
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> Entities { get; }
        void Create(TEntity entity);

        void SaveChanges();
    }
}