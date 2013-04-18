using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Models.Abstract;
using Mvc.Infrastructure.Abstract;
using System.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class SqlCrudUnitOfWork<TEntity> : ISqlCrudUnitOfWork<TEntity> where TEntity : SqlEntity
    {
        private readonly IRepository<TEntity> repository;

        public SqlCrudUnitOfWork(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public TEntity Create(TEntity entity)
        {
            return repository.CreateOrUpdate(entity);
        }

        public IEnumerable<TEntity> Entities
        {
            get { return repository.Entities; }
        }

        public TEntity Read(object id)
        {
            return repository.GetById(id);
        }

        public CrudResult<TEntity> Update(TEntity entity)
        {
            try
            {
                var updatedEntity = repository.CreateOrUpdate(entity);
                return new CrudResult<TEntity>(true, updatedEntity);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return CreateFailCrudResult(exception);
            }
        }

        public CrudResult<TEntity> Delete(TEntity entity)
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

        public CrudResult<TEntity> Delete(object id)
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
    }
}