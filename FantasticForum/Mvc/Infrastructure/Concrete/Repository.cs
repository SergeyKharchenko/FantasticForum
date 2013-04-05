using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Models;
using Models.Abstract;
using Mvc.Infrastructure.Abstract;
using System.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
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

        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public void Remove(object id)
        {
            var entity = dbSet.Find(id);
            Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}