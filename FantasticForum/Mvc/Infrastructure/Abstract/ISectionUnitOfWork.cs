using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.StructModels;

namespace Mvc.Infrastructure.Abstract
{
    public interface ISectionUnitOfWork : IUnitOfWork
    {
        IEnumerable<Section> Section { get; }
        void Create(Section section, HttpPostedFileBase avatar);
        GetAvatarSM GetAvatar(int sectionId);
    }
}