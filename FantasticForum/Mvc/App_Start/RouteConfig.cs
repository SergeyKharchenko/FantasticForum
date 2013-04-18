using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*
            #if !DEBUG
            routes.MapRoute(
                name: "Preview",
                url: "{*catchall}",
                defaults: new { controller = "Preview", action = "Index" }
            );
            #endif
             */

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Section", action = "List", id = UrlParameter.Optional}
                );
        }
    }
}