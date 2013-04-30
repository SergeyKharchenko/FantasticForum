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
        public void WriteAuthInfoInSession(HttpSessionStateBase httpSession, int userId)
        {
            httpSession.Add(ConfigurationManager.AppSettings.Get("Auth"), userId);
        }

        public AuthorizeUtilityModel ReadAuthInfoFromSession(HttpSessionStateBase httpSession)
        {
            var authorizeUtilityModel = new AuthorizeUtilityModel {IsAuthorized = false};
            var authData = httpSession[ConfigurationManager.AppSettings.Get("Auth")];
            if (authData is int)
            {
                authorizeUtilityModel.UserId = (int) authData;
                authorizeUtilityModel.IsAuthorized = true;
            }
            return authorizeUtilityModel;
        }
    }
}