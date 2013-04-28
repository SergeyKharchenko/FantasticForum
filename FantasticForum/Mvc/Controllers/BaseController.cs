using System.Web.Mvc;
using Models;
using Models.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;

namespace Mvc.Controllers
{
    public abstract class BaseController<TEntity> : Controller where TEntity : SqlEntity
    {
        protected SqlCrudUnitOfWork<TEntity> unitOfWork;

        protected BaseController(SqlCrudUnitOfWork<TEntity> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}