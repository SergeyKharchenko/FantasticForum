using Models.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;
using System.Web.Mvc;

namespace Mvc.Controllers
{
    public abstract class BaseController<TEntity> : Controller where TEntity : SqlEntity
    {
        protected SqlCrudUnitOfWork<TEntity> unitOfWork;

        protected BaseController(SqlCrudUnitOfWork<TEntity> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}