using System.Data.Entity;
using System.Web;
using Models;
using Mvc.Infrastructure.Concrete;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Abstract
{
    public abstract class AbstractUserUnitOfWork : SqlCrudUnitOfWork<User>
    {
        protected AbstractUserUnitOfWork(DbContext context, IRepository<User> repository) 
            : base(context, repository)
        {
        }

        public abstract string RegisterUser(User user, HttpPostedFileBase avatar);
    }
}