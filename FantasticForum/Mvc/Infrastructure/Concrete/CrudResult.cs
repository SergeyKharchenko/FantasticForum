using Models.Abstract;

namespace Mvc.Infrastructure.Concrete
{
    public class CrudResult<TEntity> where TEntity : SqlEntity
    {
        public bool Success { get; private set; }
        public TEntity Entity { get; private set; }

        public CrudResult(bool success, TEntity entity)
        {
            Success = success;
            Entity = entity;
        }
    }
}