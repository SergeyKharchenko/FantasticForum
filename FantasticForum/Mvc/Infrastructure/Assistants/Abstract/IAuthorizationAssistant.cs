using System.Web;
using System.Web.SessionState;
using Models;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IAuthorizationAssistant
    {
        void WriteAuthInfoInSession(HttpSessionStateBase httpSession, User user);
        User ReadAuthInfoFromSession(HttpSessionState httpSession);
        User ReadAuthInfoFromSession(HttpSessionStateBase httpSession); // temp
        void RemoveAuthInfoFromSession(HttpSessionStateBase httpSession);
    }
}