﻿using System;
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
        protected readonly IRepository<TEntity> repository;
        private readonly DbContext context;

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

        public virtual IEnumerable<TEntity> Read(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return repository.Get(filter, includeProperties);
        }

        public virtual CrudUtilityModel<TEntity> Update(TEntity entity)
        {
            try
            {
                var updatedEntity = repository.Update(entity);
                return new CrudUtilityModel<TEntity>(true, updatedEntity);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return CreateFailCrudResult(exception);
            }
        }

        public virtual CrudUtilityModel<TEntity> Delete(TEntity entity)
        {
            try
            {
                var deletedEntity = repository.Remove(entity);
                return new CrudUtilityModel<TEntity>(true, deletedEntity);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return CreateFailCrudResult(exception);
            }
        }

        public virtual CrudUtilityModel<TEntity> Delete(object id)
        {
            try
            {
                var deletedEntity = repository.Remove(id);
                return new CrudUtilityModel<TEntity>(true, deletedEntity);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return CreateFailCrudResult(exception);
            }
        }

        private static CrudUtilityModel<TEntity> CreateFailCrudResult(DbUpdateConcurrencyException exception)
        {
            var entry = exception.Entries.FirstOrDefault();
            return new CrudUtilityModel<TEntity>(false, entry == null ? null : entry.Entity as TEntity);
        }

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
                context.Dispose();
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}