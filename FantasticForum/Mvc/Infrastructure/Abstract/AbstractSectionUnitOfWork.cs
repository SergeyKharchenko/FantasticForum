using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Concrete;
using Mvc.StructModels;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Abstract
{
    public abstract class AbstractSectionUnitOfWork : SqlCrudUnitOfWork<Section>
    {
        protected AbstractSectionUnitOfWork(IRepository<Section> repository)
            : base(repository)
        {
        }

        public abstract CrudResult<Section> CreateOrUpdateSection(Section section, HttpPostedFileBase avatar);
        public abstract GetAvatarSM GetAvatar(int sectionId);
    }
}