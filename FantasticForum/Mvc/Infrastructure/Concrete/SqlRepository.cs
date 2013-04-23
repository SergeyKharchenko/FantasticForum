using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using Models.Abstract;
using Mvc.Infrastructure.Abstract;
using System.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class SqlRepository<TEntity> : IRepository<TEntity> where TEntity : SqlEntity
    {
        private readonly DbContext context;
        private readonly DbSet<TEntity> dbSet;

        public SqlRepository(DbContext context)
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
            return entity;
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.AsEnumerable();
        }

        public TEntity Create(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                var oldEntity = dbSet.Find(entity.Id);
                context.Entry(oldEntity).State = EntityState.Detached;
            }
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return entity;
        }

        public TEntity Remove(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                var oldEntity = dbSet.Find(entity.Id);
                context.Entry(oldEntity).State = EntityState.Detached;
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            context.SaveChanges();
            return entity;
        }

        public TEntity Remove(object id)
        {
            var entity = dbSet.Find(id);
            return Remove(entity);
        }
    }
}