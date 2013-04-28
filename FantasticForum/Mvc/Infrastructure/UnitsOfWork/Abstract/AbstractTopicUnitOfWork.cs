using System.Data.Entity;
using Models;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;

namespace Mvc.Infrastructure.UnitsOfWork.Abstract
{
    public abstract class AbstractTopicUnitOfWork : SqlCrudUnitOfWork<Topic>
    {
        protected AbstractTopicUnitOfWork(DbContext context, IRepository<Topic> repository)
            : base(context, repository)
        {
        }

    }
}