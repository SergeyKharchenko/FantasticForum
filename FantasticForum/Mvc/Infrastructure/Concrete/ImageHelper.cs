using System;
using System.IO;
using System.Web;
using Models;
using Mvc.Infrastructure.Abstract;
using System.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class ImageHelper : IImageHelper
    {
        public string Save(HttpPostedFileBase fileBase, string path, int newId)
        {
            var fileName = Path.Combine(path,
                                        String.Concat(newId, Path.GetExtension(fileBase.FileName)));
            fileBase.SaveAs(fileName);

            return fileName;
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }
    }
}