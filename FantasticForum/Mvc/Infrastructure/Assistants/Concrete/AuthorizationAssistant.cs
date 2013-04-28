using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Security;
using Mvc.Infrastructure.Assistants.Abstract;

namespace Mvc.Infrastructure.Assistants.Concrete
{
    public class AuthorizationAssistant : IAuthorizationAssistant
    {
        public void PlaceAuthInfoInCookie(HttpResponseBase httpResponse, int userId)
        {
            var formsAuthenticationTicket = new FormsAuthenticationTicket(userId.ToString(CultureInfo.InvariantCulture),
                                                                          false,
                                                                          (int)FormsAuthentication.Timeout.TotalMinutes);
            var authTicket = FormsAuthentication.Encrypt(formsAuthenticationTicket);
            var authCookie = new HttpCookie(ConfigurationManager.AppSettings.Get("Auth")) { Value = authTicket };
            httpResponse.Cookies.Set(authCookie);
        }
    }
}