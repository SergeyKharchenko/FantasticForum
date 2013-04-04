using System.Linq;
using Models.Abstract;

namespace Mvc.Infrastructure.Abstract
{
    public interface IMongoRepository<TEntity> where TEntity : MongoEntity
    {
        IQueryable<TEntity> Entities { get; }
        void Create(TEntity entity);
        TEntity Get(string id);
        void Remove(string id);
        void DropCollection();
    }
}