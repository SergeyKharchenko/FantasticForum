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
                name: "Record",
                url: "Section/{sectionId}/Topic/{topicId}/{controller}/{action}/{id}",
                defaults: new {controller = "Record", action = "List", id = UrlParameter.Optional},
                constraints: new { sectionId = @"\d+", topicId = @"\d+" }
                );

            routes.MapRoute(
                name: "Topic",
                url: "Section/{sectionId}/{controller}/{action}/{id}",
                defaults: new {controller = "Topic", action = "List", id = UrlParameter.Optional},
                constraints: new { sectionId = @"\d+" }
                );

            routes.MapRoute(
                name: "Section",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Section", action = "List", id = UrlParameter.Optional}
                );
        }
    }
}