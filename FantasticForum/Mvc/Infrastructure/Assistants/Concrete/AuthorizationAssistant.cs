using Mvc.Infrastructure.Assistants.Abstract;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Security;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Assistants.Concrete
{
    public class AuthorizationAssistant : IAuthorizationAssistant
    {
        public void WriteAuthInfoInCookie(HttpResponseBase httpResponse, int userId)
        {
            var formsAuthenticationTicket = new FormsAuthenticationTicket(userId.ToString(CultureInfo.InvariantCulture),
                                                                          false,
                                                                          (int)FormsAuthentication.Timeout.TotalMinutes);
            var authTicket = FormsAuthentication.Encrypt(formsAuthenticationTicket);
            var authCookie = new HttpCookie(ConfigurationManager.AppSettings.Get("Auth")) { Value = authTicket };
            httpResponse.Cookies.Set(authCookie);
        }

        public AuthorizeUtilityModel ReadAuthInfoFromCookie(HttpRequestBase httpRequest)
        {
            var authorizeUtilityModel = new AuthorizeUtilityModel {IsAuthorized = false};
            var authCookie = httpRequest.Cookies.Get(ConfigurationManager.AppSettings.Get("Auth"));
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null)
                {
                    int userId;
                    authorizeUtilityModel.IsAuthorized = int.TryParse(ticket.Name, out userId);
                    authorizeUtilityModel.UserId = userId;
                }
            }
            return authorizeUtilityModel;
        }
    }
}