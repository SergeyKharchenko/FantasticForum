using System.Collections.Generic;
using Models;
using Models.Abstract;

namespace Mvc.Infrastructure.Abstract
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> Entities { get; }
        TEntity GetById(int id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void Remove(object id);
        void SaveChanges();
    }
}