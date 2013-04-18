using System.Collections.Generic;
using Models.Abstract;
using Mvc.Infrastructure.Concrete;

namespace Mvc.Infrastructure.Abstract
{
    public interface ISqlCrudUnitOfWork<TEntity> where TEntity : SqlEntity
    {
        TEntity Create(TEntity entity);
        IEnumerable<TEntity> Entities { get; }
        TEntity Read(object id);
        CrudResult<TEntity> Update(TEntity entity);
        CrudResult<TEntity> Delete(TEntity entity);
        CrudResult<TEntity> Delete(object id);
    }
}