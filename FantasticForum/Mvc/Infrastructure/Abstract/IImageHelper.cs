using System.Web;
using Models;

namespace Mvc.Infrastructure.Abstract
{
    public interface IImageHelper
    {
        string Save(HttpPostedFileBase fileBase, string path, int newId);
        void Delete(string path);
    }
}