using Models;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;
using System.Data.Entity;
using System.Web;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.UnitsOfWork.Abstract
{
    public abstract class AbstractUserUnitOfWork : SqlCrudUnitOfWork<User>
    {
        protected AbstractUserUnitOfWork(DbContext context, IRepository<User> repository) 
            : base(context, repository)
        {
        }

        public abstract User RegisterUser(User user, HttpPostedFileBase avatar);
        public abstract ImageUtilityModel GetAvatar(int userId);
    }
}