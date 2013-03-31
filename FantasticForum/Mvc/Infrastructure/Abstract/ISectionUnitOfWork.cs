using System.Collections.Generic;
using System.Web;
using Models;

namespace Mvc.Infrastructure.Abstract
{
    public interface ISectionUnitOfWork : IUnitOfWork
    {
        IEnumerable<Section> Section { get; }
        void Create(Section section, HttpPostedFileBase avatar, string path, string virtualPath);
    }
}