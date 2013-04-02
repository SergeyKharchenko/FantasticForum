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
        private readonly IMongoRepository<Image> imageMongoRepository;
        private readonly IFileHelper fileHelper;

        public SectionUnitOfWork(IRepository<Section> sectionRepository,
                                 IMongoRepository<Image> imageMongoRepository,
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

        public void Create(Section section, HttpPostedFileBase avatar)
        {
            if (avatar != null)
            {
                var imageData = fileHelper.FileBaseToByteArray(avatar);
                var image = new Image {Data = imageData, ImageMimeType = avatar.ContentType};
                imageMongoRepository.Create(image);
                section.ImageId = image.Id.ToString();
            }
            sectionRepository.Create(section);
            sectionRepository.SaveChanges();
        }

        public GetAvatarSM GetAvatar(int sectionId)
        {
            var section = sectionRepository.GetById(sectionId);
            if (string.IsNullOrEmpty(section.ImageId))
                return new GetAvatarSM(false);
            var image = imageMongoRepository.Get(section.ImageId);
            return new GetAvatarSM(true, image.Data, image.ImageMimeType);
        }
    }
}