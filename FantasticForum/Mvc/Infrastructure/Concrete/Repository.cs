using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using Models;
using Models.Abstract;
using Mvc.Infrastructure.Abstract;
using System.Linq;
using NUnit.Framework;

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
            return dbSet.Find(id);
        }

        public void CreateOrUpdate(TEntity entity)
        {
            var sqlEntity = entity as Entity;
            if (sqlEntity == null)
                return;
            if (sqlEntity.Id == 0)
            {
                dbSet.Add(entity);
            }
            else
            {
                var entry = context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {                    
                    var attachedEntity = GetById(sqlEntity.Id);
                    if (attachedEntity != null)
                    {
                        var attachedEntry = context.Entry(attachedEntity);
                        attachedEntry.CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                    }
                }
            }
            context.SaveChanges();
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