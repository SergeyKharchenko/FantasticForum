using System.Collections.Generic;
using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Infrastructure.Concrete
{
    public class SectionUnitOfWork : IEntityUnitOfWork
    {
        private readonly IRepository<Section> topicRepository;

        public SectionUnitOfWork(IRepository<Section> topicRepository)
        {
            this.topicRepository = topicRepository;
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Entity> Sections
        {
            get { return topicRepository.Entities; }
        }
    }
}