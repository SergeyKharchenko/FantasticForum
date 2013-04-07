using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Models.Abstract;
using Mvc.Infrastructure.Abstract;
using System.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext context;
        private readonly DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Entities
        {
            get { return dbSet.AsEnumerable(); }
        }

        public TEntity GetById(object id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
                context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public void CreateOrUpdate(TEntity entity)
        {
            var sqlEntity = entity as Entity;
            if (sqlEntity == null)
                return;
            if (sqlEntity.Id == 0)
                dbSet.Add(entity);
            else
                UpdateValues(entity);
            context.SaveChanges();
        }

        private void UpdateValues(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Remove(object id)
        {
            var entity = dbSet.Find(id);
            Remove(entity);
            context.SaveChanges();
        }
    }
}