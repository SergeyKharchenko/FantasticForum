﻿using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Models;
using Mvc.App_Start;
using Mvc.Infrastructure.Concrete;

namespace Mvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            #if DEBUG
            Database.SetInitializer(new ForumContextInitializer());
            #else
            var context = new ForumContext();
            context.Database.Initialize(true);
            #endif

        }
    }
}