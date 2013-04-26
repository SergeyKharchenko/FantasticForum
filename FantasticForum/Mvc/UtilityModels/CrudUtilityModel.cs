using Models.Abstract;

namespace Mvc.UtilityModels
{
    public class CrudUtilityModel<TEntity> where TEntity : SqlEntity
    {
        public bool Success { get; private set; }
        public TEntity Entity { get; private set; }

        public CrudUtilityModel(bool success, TEntity entity)
        {
            Success = success;
            Entity = entity;
        }
    }
}