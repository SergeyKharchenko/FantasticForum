using System.Web;
using Models;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IAuthorizationAssistant
    {
        void WriteAuthInfoInSession(HttpSessionStateBase httpSession, User user);
        User ReadAuthInfoFromSession(HttpSessionStateBase httpSession);
    }
}