using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;
using System.Linq;
using Mvc.StructModels;

namespace Mvc.Infrastructure.Concrete
{
    public class SectionUnitOfWork : ISectionUnitOfWork
    {
        private readonly IRepository<Section> sectionRepository;
        private readonly IRepository<Image> imageMongoRepository;
        private readonly IFileHelper fileHelper;

        public SectionUnitOfWork(IRepository<Section> sectionRepository,
                                 IRepository<Image> imageMongoRepository,
                                 IFileHelper fileHelper)
        {
            this.sectionRepository = sectionRepository;
            this.imageMongoRepository = imageMongoRepository;
            this.fileHelper = fileHelper;
        }

        public IEnumerable<Section> Section
        {
            get { return sectionRepository.Entities; }
        }

        public Section GetSectionById(int sectionId)
        {
            return sectionRepository.GetById(sectionId);
        }

        public void CreateOrUpdateSection(Section section, HttpPostedFileBase avatar)
        {
            section.ImageId = GetOldAvatarId(section.Id);

            if (avatar != null)
            {
                RemoveOldAvatar(section);
                section.ImageId = CreateAvatar(avatar);
            }
            sectionRepository.CreateOrUpdate(section);
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
            imageMongoRepository.CreateOrUpdate(image);
            return image.Id.ToString();
        }

        private string GetOldAvatarId(int sectionId)
        {
            var section = sectionRepository.GetById(sectionId);
            return section == null ? null : section.ImageId;
        }

        public void RemoveSection(Section section)
        {
            var oldSection = sectionRepository.GetById(section.Id);
            if (!string.IsNullOrEmpty(oldSection.ImageId))
                imageMongoRepository.Remove(oldSection.ImageId);
            sectionRepository.Remove(section);
        }

        public GetAvatarSM GetAvatar(int sectionId)
        {
            var section = sectionRepository.GetById(sectionId);
            if (string.IsNullOrEmpty(section.ImageId))
                return new GetAvatarSM(false);
            var image = imageMongoRepository.GetById(section.ImageId);
            return new GetAvatarSM(true, image.Data, image.ImageMimeType);
        }
    }
}