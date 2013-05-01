using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.DAL.Cocnrete;

namespace Mvc.Filters
{
    public class ForumAuthorizeAttribute : AuthorizeAttribute
    {
        public IAuthorizationAssistant assistant =
            (IAuthorizationAssistant) DependencyResolver.Current.GetService(typeof (IAuthorizationAssistant));

        public IRepository<User> repository;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userIndentity = new UserIndentity();
            httpContext.User = userIndentity;
            var authorizeUtilityModel = assistant.ReadAuthInfoFromSession(httpContext.Session);
            if (!authorizeUtilityModel.IsAuthorized)
                return false;

            var usedNewContext = false;
            ForumContext context = null;

            if (repository == null)
            {
                usedNewContext = true;
                context = new ForumContext();
                repository = new SqlRepository<User>(context);
            }
                userIndentity.User = repository.GetById(authorizeUtilityModel.UserId);
            if (usedNewContext)
                context.Dispose();
            repository = null;
            return true;
        }
    }
}