using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.StructModels;

namespace Mvc.Infrastructure.Abstract
{
    public interface ISectionUnitOfWork
    {
        IEnumerable<Section> Section { get; }
        Section GetSectionById(int sectionId);
        void CreateOrUpdateSection(Section section, HttpPostedFileBase avatar);
        void RemoveSection(Section section);
        GetAvatarSM GetAvatar(int sectionId);
    }
}