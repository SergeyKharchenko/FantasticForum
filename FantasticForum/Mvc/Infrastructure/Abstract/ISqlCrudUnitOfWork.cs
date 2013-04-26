﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Models.Abstract;
using Mvc.Infrastructure.Concrete;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Abstract
{
    public interface ISqlCrudUnitOfWork<TEntity> where TEntity : SqlEntity
    {
        TEntity Create(TEntity entity);
        IEnumerable<TEntity> Entities { get; }
        TEntity Read(object id);
        IEnumerable<TEntity> Read(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        CrudUtilityModel<TEntity> Update(TEntity entity);
        CrudUtilityModel<TEntity> Delete(TEntity entity);
        CrudUtilityModel<TEntity> Delete(object id);
    }
}