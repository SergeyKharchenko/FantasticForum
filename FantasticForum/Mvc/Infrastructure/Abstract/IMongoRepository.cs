using Models.Abstract;

namespace Mvc.Infrastructure.Abstract
{
    public interface IMongoRepository<TEntity> where TEntity : MongoEntity
    {
        void Create(TEntity entity);
        TEntity Get(string id);
        void Remove(string id);
        void DropCollection();
    }
}