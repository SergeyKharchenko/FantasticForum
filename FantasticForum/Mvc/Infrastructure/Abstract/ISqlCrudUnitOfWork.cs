using Models.Abstract;
using Mvc.Infrastructure.Concrete;

namespace Mvc.Infrastructure.Abstract
{
    public interface ICrudUnitOfWork<TEntity> where TEntity : Entity
    {
        TEntity Create(TEntity entity);
        TEntity Read(object id);
        CrudResult Read(TEntity entity);
        CrudResult Delete(TEntity entity);
        CrudResult Delete(object id);
    }
}