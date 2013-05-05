using Models.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using System.Web.Mvc;

namespace Mvc.Controllers
{
    public abstract class BaseController<TEntity> : Controller where TEntity : SqlEntity
    {
        protected ISqlCrudUnitOfWork<TEntity> unitOfWork;

        protected BaseController(ISqlCrudUnitOfWork<TEntity> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}