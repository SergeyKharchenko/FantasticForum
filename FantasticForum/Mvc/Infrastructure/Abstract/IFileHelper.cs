using System.Web;

namespace Mvc.Infrastructure.Abstract
{
    public interface IFileHelper
    {
        byte[] FileBaseToByteArray(HttpPostedFileBase fileBase);
        byte[] FileToByteArray(string path);
    }
}