using System.Linq;
using Models;
using Models.Abstract;

namespace Mvc.Infrastructure.Abstract
{
    public interface IMongoRepository<TEntity> where TEntity : MongoEntity
    {
        IQueryable<TEntity> Entities { get; }
        void Create(TEntity entity);
        TEntity Get(string id);
        void Update(TEntity entity);
        void Remove(string id);
        void DropCollection();
    }
}