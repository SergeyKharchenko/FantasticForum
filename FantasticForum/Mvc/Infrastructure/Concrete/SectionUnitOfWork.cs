using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;
using System.Linq;
using Mvc.StructModels;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Concrete
{
    public class SectionUnitOfWork : AbstractSectionUnitOfWork
    {
        private readonly IRepository<Image> imageMongoRepository;
        private readonly IFileHelper fileHelper;

        public SectionUnitOfWork(DbContext context,
                                 IRepository<Section> sectionRepository,
                                 IRepository<Image> imageMongoRepository,
                                 IFileHelper fileHelper)
            : base(context, sectionRepository)
        {
            this.imageMongoRepository = imageMongoRepository;
            this.fileHelper = fileHelper;
        }

        public override CrudResult<Section> CreateOrUpdateSection(Section section, HttpPostedFileBase avatar)
        {
            var crudResult = new CrudResult<Section>(true, section);            

            string newAvatarId = null;
            if (avatar != null)
                newAvatarId = CreateAvatar(avatar);

            if (section.Id == 0)
            {
                section.ImageId = newAvatarId;
                Create(section);
            }
            else
            {
                section.ImageId = GetOldAvatarId(section.Id);
                if (avatar != null)
                {
                    RemoveOldAvatar(section);
                    section.ImageId = newAvatarId;
                }
                crudResult = Update(section);
            }

            return crudResult;
        }

        private void RemoveOldAvatar(Section section)
        {
            if (section.Id != 0 && !string.IsNullOrEmpty(section.ImageId))
                imageMongoRepository.Remove(section.ImageId);
        }

        private string CreateAvatar(HttpPostedFileBase avatar)
        {
            var imageData = fileHelper.FileBaseToByteArray(avatar);
            var image = new Image {Data = imageData, ImageMimeType = avatar.ContentType};
            image = imageMongoRepository.Create(image);
            return image.Id.ToString();
        }

        private string GetOldAvatarId(int sectionId)
        {
            var section = repository.GetById(sectionId);
            return section == null ? null : section.ImageId;
        }

        public void RemoveSection(Section section)
        {
            var oldSection = repository.GetById(section.Id);
            if (!string.IsNullOrEmpty(oldSection.ImageId))
                imageMongoRepository.Remove(oldSection.ImageId);
            repository.Remove(section);
        }

        public override GetAvatarSM GetAvatar(int sectionId)
        {
            var section = repository.GetById(sectionId);
            if (string.IsNullOrEmpty(section.ImageId))
                return new GetAvatarSM(false);
            var image = imageMongoRepository.GetById(section.ImageId);
            return new GetAvatarSM(true, image.Data, image.ImageMimeType);
        }
    }
}