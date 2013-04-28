using System.Web.Mvc;

namespace Mvc.Filters
{
    public class ForumAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }
    }
}