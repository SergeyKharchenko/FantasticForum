using Mvc.App_Start;
using Mvc.Infrastructure;
using Mvc.Infrastructure.DAL;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            
            #if DEBUG
            Database.SetInitializer(new ForumContextInitializer());
            #else
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ForumContext, Models.Migrations.Configuration>());
            #endif

        }
    }
}