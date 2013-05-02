using System;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Mvc.Infrastructure.Assistants.Abstract;

namespace Mvc.Filters
{
    public class AuthModule : IHttpModule
    {
        private readonly IAuthorizationAssistant assistant =
            (IAuthorizationAssistant) DependencyResolver.Current.GetService(typeof (IAuthorizationAssistant));

        public void Init(HttpApplication application)
        {
            application.PostAcquireRequestState += Application_PostAcquireRequestState;
            application.PostMapRequestHandler += Application_PostMapRequestHandler;
        }

        public void Dispose()
        {
        }

        private void Application_PostMapRequestHandler(object source, EventArgs e)
        {
            var app = (HttpApplication) source;

            if (app.Context.Handler is IRequiresSessionState)
            {
                // no need to replace the current handler
                return;
            }

            // swap the current handler
            app.Context.Handler = new MyHttpHandler(app.Context.Handler);
        }

        private void Application_PostAcquireRequestState(object source, EventArgs e)
        {
            var app = (HttpApplication) source;

            var resourceHttpHandler = HttpContext.Current.Handler as MyHttpHandler;

            if (resourceHttpHandler != null)
            {
                // set the original handler back
                HttpContext.Current.Handler = resourceHttpHandler.originalHandler;
            }
            var userIndentity = new UserIndentity {User = assistant.ReadAuthInfoFromSession(app.Session)};
            HttpContext.Current.User = userIndentity;
        }

        // a temp handler used to force the SessionStateModule to load session state
        public class MyHttpHandler : IHttpHandler, IRequiresSessionState
        {
            internal readonly IHttpHandler originalHandler;

            public MyHttpHandler(IHttpHandler originalHandler)
            {
                this.originalHandler = originalHandler;
            }

            public void ProcessRequest(HttpContext context)
            {
                // do not worry, ProcessRequest() will not be called, but let's be safe
                throw new InvalidOperationException("MyHttpHandler cannot process requests.");
            }

            public bool IsReusable
            {
                // IsReusable must be set to false since class has a member!
                get { return false; }
            }
        }
    }
}