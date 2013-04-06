using System.Collections.Generic;
using System.Linq;
using Models.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Mvc.Infrastructure.Abstract;
using MongoDB.Driver.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MongoCollection<TEntity> collection;

        public MongoRepository(MongoDatabase database, string collectionName = "")
        {
            if (string.IsNullOrEmpty(collectionName))
                collectionName = string.Format("{0}s", typeof (TEntity).Name);
            collection = database.GetCollection<TEntity>(collectionName);   
        }

        public IEnumerable<TEntity> Entities
        {
            get { return collection.AsQueryable().AsEnumerable(); }
        }

        public TEntity GetById(object id)
        {
            var objectId = new ObjectId(id.ToString());
            var query = Query<MongoEntity>.EQ(entity => entity.Id, objectId);
            return collection.FindOne(query);
        }

        public void Create(TEntity entity)
        {
            collection.Insert(entity);
        }

        public void Update(TEntity entity)
        {
            collection.Save(entity);
        }

        public void Remove(TEntity entity)
        {
            var mongoEntity = entity as MongoEntity;
            if (mongoEntity == null)
                return;
            var query = Query.EQ("Id", mongoEntity.Id);
            collection.Remove(query);
        }

        public void Remove(object id)
        {
            var objectId = new ObjectId(id.ToString());
            var query = Query<MongoEntity>.EQ(entity => entity.Id, objectId);
            collection.Remove(query);
        }

        public void DropCollection()
        {
            collection.Drop();
        }
    }
}