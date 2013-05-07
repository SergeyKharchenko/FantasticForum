using System.Security.Policy;
using System.Web.Mvc;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IUrlAssistant
    {
        string GenerateAbsoluteUrl(string action, string controller, object routeValues, UrlHelper urlHelper);
    }
}