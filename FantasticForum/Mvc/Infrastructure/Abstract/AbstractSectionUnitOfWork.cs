using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Concrete;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Abstract
{
    public abstract class AbstractSectionUnitOfWork : SqlCrudUnitOfWork<Section>
    {
        protected AbstractSectionUnitOfWork(DbContext context, IRepository<Section> repository)
            : base(context, repository)
        {
        }

        public abstract CrudUtilityModel<Section> CreateOrUpdateSection(Section section, HttpPostedFileBase avatar);
        public abstract AvatarUtilityModel GetAvatar(int sectionId);
    }
}