using System.Web;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IAuthorizationAssistant
    {
        void WriteAuthInfoInSession(HttpSessionStateBase httpSession, int userId);
        AuthorizeUtilityModel ReadAuthInfoFromSession(HttpSessionStateBase httpSession);
    }
}