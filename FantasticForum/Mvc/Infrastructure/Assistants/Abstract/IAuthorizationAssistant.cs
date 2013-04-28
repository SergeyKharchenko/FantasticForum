using System.Web;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IAuthorizationAssistant
    {
        void WriteAuthInfoInCookie(HttpResponseBase httpResponse, int userId);
        bool ReadAuthInfoFromCookie(HttpRequestBase httpRequest, ref int userId);
    }
}