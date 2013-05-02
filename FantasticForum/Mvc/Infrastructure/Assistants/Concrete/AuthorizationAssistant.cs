using System.Web.SessionState;
using Models;
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
        public void WriteAuthInfoInSession(HttpSessionStateBase httpSession, User user)
        {
            httpSession.Add(ConfigurationManager.AppSettings.Get("Auth"), user);
        }

        public User ReadAuthInfoFromSession(HttpSessionState httpSession)
        {
            var authData = httpSession[ConfigurationManager.AppSettings.Get("Auth")];
            return authData as User;
        }

        public User ReadAuthInfoFromSession(HttpSessionStateBase httpSession)
        {
            var authData = httpSession[ConfigurationManager.AppSettings.Get("Auth")];
            return authData as User;
        }
    }
}