using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.DAL.Abstract;

namespace Mvc.Filters
{
    public class ForumAuthorizeAttribute : AuthorizeAttribute
    {
        public IAuthorizationAssistant assistant =
            (IAuthorizationAssistant) DependencyResolver.Current.GetService(typeof (IAuthorizationAssistant));

        public IRepository<User> repository =
            (IRepository<User>) DependencyResolver.Current.GetService(typeof (IRepository<User>));
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userIndentity = new UserIndentity();
            httpContext.User = userIndentity;
            var authorizeUtilityModel = assistant.ReadAuthInfoFromCookie(httpContext.Request);
            if (!authorizeUtilityModel.IsAuthorized)
                return false;
            userIndentity.User = repository.GetById(authorizeUtilityModel.UserId);
            return true;
        }
    }
}