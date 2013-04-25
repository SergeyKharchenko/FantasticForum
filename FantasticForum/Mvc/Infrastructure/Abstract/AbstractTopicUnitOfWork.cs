using System.Data.Entity;
using Models;
using Mvc.Infrastructure.Concrete;

namespace Mvc.Infrastructure.Abstract
{
    public abstract class AbstractTopicUnitOfWork : SqlCrudUnitOfWork<Topic>
    {
        protected AbstractTopicUnitOfWork(IRepository<Topic> repository)
            : base(repository)
        {
        }

    }
}