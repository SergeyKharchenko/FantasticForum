using System.Web;
using System.Web.Mvc;
using Mvc.Infrastructure.Assistants.Abstract;
using Ninject;

namespace Mvc.Filters
{
    public class ForumAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }
    }
}