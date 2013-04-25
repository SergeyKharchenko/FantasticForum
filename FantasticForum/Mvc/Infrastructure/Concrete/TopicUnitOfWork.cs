using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Infrastructure.Concrete
{
    public class TopicUnitOfWork : AbstractTopicUnitOfWork
    {
        private readonly IRepository<Record> recordRepository;

        public TopicUnitOfWork(IRepository<Topic> repository, IRepository<Record> recordRepository)
            : base(repository)
        {
            this.recordRepository = recordRepository;
        }


    }
}