using System.Collections.Generic;
using Models;
using Models.Abstract;

namespace Mvc.Infrastructure.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Entities { get; }
        TEntity GetById(object id);
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Remove(TEntity entity);
        TEntity Remove(object id);
    }
}