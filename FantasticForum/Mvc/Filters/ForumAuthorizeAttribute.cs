using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.DAL.Cocnrete;
using Ninject;

namespace Mvc.Filters
{
    public class ForumAuthorizeAttribute : AuthorizeAttribute
    {
        [Inject]
        public IAuthorizationAssistant Assistant { get; set; }

        [Inject]
        public IRepository<User> Repository { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        public ForumAuthorizeAttribute()
        {
            var logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            logger.WriteToLog("ctor");
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            Logger.WriteToLog("OnAuthorization");
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Logger.WriteToLog("AuthorizeCore");
            var userIndentity = new UserIndentity();
            httpContext.User = userIndentity;
            var authorizeUtilityModel = Assistant.ReadAuthInfoFromSession(httpContext.Session);
            if (!authorizeUtilityModel.IsAuthorized)
                return false;

            userIndentity.User = Repository.GetById(authorizeUtilityModel.UserId);
            return true;
        }
    }
}