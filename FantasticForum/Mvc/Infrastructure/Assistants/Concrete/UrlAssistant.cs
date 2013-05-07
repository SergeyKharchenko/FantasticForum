using System.Web.Mvc;
using Mvc.Infrastructure.Assistants.Abstract;

namespace Mvc.Infrastructure.Assistants.Concrete
{
    public class UrlAssistant : IUrlAssistant
    {
        public string GenerateAbsoluteUrl(string action, string controller, object routeValues, UrlHelper urlHelper)
        {
            return urlHelper.Action("RegistrationConfirmation", "Account", routeValues, "http");
        }
    }
}