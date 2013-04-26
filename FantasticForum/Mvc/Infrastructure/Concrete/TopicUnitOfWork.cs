using System.Data.Entity;
using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Infrastructure.Concrete
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