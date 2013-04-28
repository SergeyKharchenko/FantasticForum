using Models;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;
using Mvc.UtilityModels;
using System.Data.Entity;
using System.Web;

namespace Mvc.Infrastructure.UnitsOfWork.Abstract
{
    public abstract class AbstractSectionUnitOfWork : SqlCrudUnitOfWork<Section>
    {
        protected AbstractSectionUnitOfWork(DbContext context, IRepository<Section> repository)
            : base(context, repository)
        {
        }

        public abstract CrudUtilityModel<Section> CreateOrUpdateSection(Section section, HttpPostedFileBase avatar);
        public abstract void RemoveSection(Section section);
        public abstract ImageUtilityModel GetAvatar(int sectionId);
    }
}