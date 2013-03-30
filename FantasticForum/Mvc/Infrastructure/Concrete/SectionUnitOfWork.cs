using System.Collections.Generic;
using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Infrastructure.Concrete
{
    public class SectionUnitOfWork : ISectionUnitOfWork
    {
        private readonly IRepository<Section> sectionRepository;

        public SectionUnitOfWork(IRepository<Section> sectionRepository)
        {
            this.sectionRepository = sectionRepository;
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Section> Section
        {
            get { return sectionRepository.Entities; }
        }
    }
}