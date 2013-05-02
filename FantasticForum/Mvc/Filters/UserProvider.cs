using System.Web.Mvc;
using Mvc.Infrastructure.Assistants.Abstract;
using Ninject;

namespace Mvc.Filters
{
    public class UserProvider : ActionFilterAttribute
    {
        [Inject]
        public IAuthorizationAssistant Assistant { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userIndentity = new UserIndentity();
            filterContext.HttpContext.User = userIndentity;
            userIndentity.User = Assistant.ReadAuthInfoFromSession(filterContext.HttpContext.Session);
        }
    }
}