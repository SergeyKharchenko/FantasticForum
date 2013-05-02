using System.Web.Mvc;
using Mvc.Filters;

namespace Mvc.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserProvider());
        }
    }
}