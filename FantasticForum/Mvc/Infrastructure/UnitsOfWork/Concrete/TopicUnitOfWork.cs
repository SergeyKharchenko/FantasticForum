using Models;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using System.Data.Entity;

namespace Mvc.Infrastructure.UnitsOfWork.Concrete
{
    public class TopicUnitOfWork : AbstractTopicUnitOfWork
    {
        private readonly IRepository<Record> recordRepository;

        public TopicUnitOfWork(DbContext context, IRepository<Topic> repository, IRepository<Record> recordRepository)
            : base(context, repository)
        {
            this.recordRepository = recordRepository;
        }


    }
}