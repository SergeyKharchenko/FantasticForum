using System.Web;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IAuthorizationAssistant
    {
        void PlaceAuthInfoInCookie(HttpResponseBase httpResponse, int userId);
    }
}