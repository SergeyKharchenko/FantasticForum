using System.Web;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IAuthorizationAssistant
    {
        void WriteAuthInfoInCookie(HttpResponseBase httpResponse, int userId);
        AuthorizeUtilityModel ReadAuthInfoFromCookie(HttpRequestBase httpRequest);
    }
}