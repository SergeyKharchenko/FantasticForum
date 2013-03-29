using System.Collections.Generic;
using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Infrastructure.Concrete
{
    public class TopicUnitOfWork : IEntityUnitOfWork
    {
        private readonly IRepository<Topic> topicRepository;

        public TopicUnitOfWork(IRepository<Topic> topicRepository)
        {
            this.topicRepository = topicRepository;
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Entity> Topics
        {
            get { return topicRepository.Entities; }
        }
    }
}