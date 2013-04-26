using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;
using System.Linq;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Concrete
{
    public class SectionUnitOfWork : AbstractSectionUnitOfWork
    {
        private readonly IEntityWithImageAssistant<Section> imageAssistant;

        public SectionUnitOfWork(DbContext context,
                                 IRepository<Section> sectionRepository,
                                 IEntityWithImageAssistant<Section> imageAssistant)
            : base(context, sectionRepository)
        {
            this.imageAssistant = imageAssistant;
        }

        public override CrudUtilityModel<Section> CreateOrUpdateSection(Section section, HttpPostedFileBase avatar)
        {
            var crudResult = new CrudUtilityModel<Section>(true, section);            

            string newAvatarId = null;
            if (avatar != null)
                newAvatarId = imageAssistant.CreateImage(avatar);

            if (section.Id == 0)
            {
                section.ImageId = newAvatarId;
                Create(section);
            }
            else
            {
                section.ImageId = imageAssistant.GetImageId(section.Id);
                if (avatar != null)
                {
                    imageAssistant.RemoveImageFrom(section);
                    section.ImageId = newAvatarId;
                }
                crudResult = Update(section);
            }

            return crudResult;
        }

        public override void RemoveSection(Section section)
        {
            var oldSection = repository.GetById(section.Id);
            imageAssistant.RemoveImageFrom(oldSection);
            repository.Remove(section);
        }

        public override ImageUtilityModel GetAvatar(int sectionId)
        {
            return imageAssistant.GetImageFromEntityWithId(sectionId);
        }
    }
}