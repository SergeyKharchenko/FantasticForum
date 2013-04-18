using System.IO;
using System.Web;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Infrastructure.Concrete
{
    public class FileHelper : IFileHelper
    {
        public byte[] FileBaseToByteArray(HttpPostedFileBase fileBase)
        {
            var data = new byte[fileBase.ContentLength];
            fileBase.InputStream.Read(data, 0, fileBase.ContentLength);
            return data;
        }

        public byte[] FileToByteArray(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}