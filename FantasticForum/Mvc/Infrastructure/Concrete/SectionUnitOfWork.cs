using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;
using System.Linq;

namespace Mvc.Infrastructure.Concrete
{
    public class SectionUnitOfWork : ISectionUnitOfWork
    {
        private readonly IImageHelper imageHelper;
        private readonly IRepository<Section> sectionRepository;
        private readonly IRepository<Image> imageRepository;

        public SectionUnitOfWork(IImageHelper imageHelper,
                                 IRepository<Section> sectionRepository,
                                 IRepository<Image> imageRepository)
        {
            this.imageHelper = imageHelper;
            this.sectionRepository = sectionRepository;
            this.imageRepository = imageRepository;
        }

        public void Commit()
        {
            sectionRepository.SaveChanges();
            imageRepository.SaveChanges();
        }

        public IEnumerable<Section> Section
        {
            get { return sectionRepository.Entities; }
        }

        public void Create(Section section, HttpPostedFileBase avatar, string path, string virtualPath)
        {
            lock (ControllerBuilder.Current)
            {
                if (avatar != null)
                {
                    var newId = 1;
                    if (imageRepository.Entities.Any())
                        newId = imageRepository.Entities.Max(image => image.Id) + 1;
                    var fullPath = imageHelper.Save(avatar, path, newId);
                    var fileName = Path.Combine(virtualPath, Path.GetFileName(fullPath));
                    section.Image = new Image {FileName = fileName, ImageMimeType = avatar.ContentType};
                }
                sectionRepository.Create(section);
                Commit();
            }
        }
    }
}