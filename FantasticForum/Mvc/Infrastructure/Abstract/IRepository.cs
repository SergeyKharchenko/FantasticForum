using System.Collections.Generic;
using Models;
using Models.Abstract;

namespace Mvc.Infrastructure.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Entities { get; }
        TEntity GetById(object id);
        void CreateOrUpdate(TEntity entity);
        void Remove(TEntity entity);
        void Remove(object id);
    }
}