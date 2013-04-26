using System.Web;

namespace Mvc.Infrastructure.Abstract
{
    public interface IFileAssistant
    {
        byte[] FileBaseToByteArray(HttpPostedFileBase fileBase);
        byte[] FileToByteArray(string path);
    }
}