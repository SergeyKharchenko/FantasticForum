using System.Linq;
using Models.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Mvc.Infrastructure.Abstract;
using MongoDB.Driver.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : MongoEntity
    {
        private readonly MongoCollection<TEntity> collection;

        public MongoRepository(MongoDatabase database, string collectionName = "")
        {
            if (string.IsNullOrEmpty(collectionName))
                collectionName = string.Format("{0}s", typeof (TEntity).Name);
            collection = database.GetCollection<TEntity>(collectionName);   
        }

        public IQueryable<TEntity> Entities
        {
            get { return collection.AsQueryable(); }
        }

        public void Create(TEntity entity)
        {
            collection.Insert(entity);
        }

        public TEntity Get(string id)
        {
            var objectId = new ObjectId(id);
            var query = Query<TEntity>.EQ(e => e.Id, objectId);
            return collection.FindOne(query);
        }

        public void Remove(string id)
        {
            var objectId = new ObjectId(id);
            var query = Query<TEntity>.EQ(e => e.Id, objectId);
            collection.Remove(query);
        }

        public void DropCollection()
        {
            collection.Drop();
        }
    }
}