using System.Web;
using System.Web.Mvc;
using Mvc.Infrastructure.Assistants.Abstract;
using Ninject;

namespace Mvc.Filters
{
    public class ForumAuthorizeAttribute : AuthorizeAttribute
    {
        [Inject]
        public IAuthorizationAssistant Assistant { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Assistant.ReadAuthInfoFromSession(httpContext.Session) != null;
        }
    }
}