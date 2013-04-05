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
        Section GetSectionById(int sectionId);
        void CreateSection(Section section, HttpPostedFileBase avatar);
        void RemoveSection(int sectionId);
        GetAvatarSM GetAvatar(int sectionId);
    }
}