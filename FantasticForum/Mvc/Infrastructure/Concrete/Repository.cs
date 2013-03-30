using System.Collections.Generic;
using System.Data.Entity;
using Models;
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
    }
}