using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Mvc.Infrastructure.DAL.Abstract;

namespace Mvc.Infrastructure.DAL.Cocnrete
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

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public TEntity Create(TEntity entity)
        {
            collection.Insert(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            collection.Save(entity);
            return entity;
        }

        public TEntity Remove(TEntity entity)
        {
            var mongoEntity = entity as MongoEntity;
            if (mongoEntity != null)
            {
                var query = Query.EQ("_id", mongoEntity.Id);
                collection.Remove(query);
            }
            return entity;
        }

        public TEntity Remove(object id)
        {
            var entity = GetById(id);
            return Remove(entity);
        }

        public void DropCollection()
        {
            collection.Drop();
        }
    }
}