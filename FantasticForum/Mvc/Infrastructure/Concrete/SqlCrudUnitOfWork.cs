using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Models.Abstract;
using Mvc.Infrastructure.Abstract;
using System.Linq;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Concrete
{
    public class SqlCrudUnitOfWork<TEntity> : IDisposable, ISqlCrudUnitOfWork<TEntity> where TEntity : SqlEntity
    {
        protected readonly DbContext context;
        protected readonly IRepository<TEntity> repository;

        public SqlCrudUnitOfWork(DbContext context, IRepository<TEntity> repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public virtual TEntity Create(TEntity entity)
        {
            return repository.Create(entity);
        }

        public virtual IEnumerable<TEntity> Entities
        {
            get { return repository.Entities; }
        }

        public virtual TEntity Read(object id)
        {
            return repository.GetById(id);
        }

        public IEnumerable<TEntity> Read(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return repository.Get(filter, includeProperties);
        }

        public virtual CrudResult<TEntity> Update(TEntity entity)
        {
            try
            {
                var updatedEntity = repository.Update(entity);
                return new CrudResult<TEntity>(true, updatedEntity);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return CreateFailCrudResult(exception);
            }
        }

        public virtual CrudResult<TEntity> Delete(TEntity entity)
        {
            try
            {
                var deletedEntity = repository.Remove(entity);
                return new CrudResult<TEntity>(true, deletedEntity);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return CreateFailCrudResult(exception);
            }
        }

        public virtual CrudResult<TEntity> Delete(object id)
        {
            try
            {
                var deletedEntity = repository.Remove(id);
                return new CrudResult<TEntity>(true, deletedEntity);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return CreateFailCrudResult(exception);
            }
        }

        private static CrudResult<TEntity> CreateFailCrudResult(DbUpdateConcurrencyException exception)
        {
            var entry = exception.Entries.FirstOrDefault();
            return new CrudResult<TEntity>(false, entry == null ? null : entry.Entity as TEntity);
        }


        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                //if (disposing)
                //    context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}