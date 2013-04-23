using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Concrete;
using Mvc.StructModels;

namespace Mvc.Infrastructure.Abstract
{
    public abstract class AbstractSectionUnitOfWork : SqlCrudUnitOfWork<Section>
    {
        protected AbstractSectionUnitOfWork(DbContext context, IRepository<Section> repository)
            : base(context, repository)
        {
        }

        public abstract CrudResult<Section> CreateOrUpdateSection(Section section, HttpPostedFileBase avatar);
        public abstract GetAvatarSM GetAvatar(int sectionId);
    }
}