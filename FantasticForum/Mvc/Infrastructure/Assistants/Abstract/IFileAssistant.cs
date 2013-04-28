using System.Web;

namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IFileAssistant
    {
        byte[] FileBaseToByteArray(HttpPostedFileBase fileBase);
        byte[] FileToByteArray(string path);
    }
}