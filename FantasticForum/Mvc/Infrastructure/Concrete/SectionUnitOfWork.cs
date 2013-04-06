﻿using System.Collections.Generic;
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
            var oldSsection = sectionRepository.GetById(section.Id);
            if (oldSsection != null)
                section.ImageId = oldSsection.ImageId;

            if (avatar != null)
            {
                if (section.Id != 0 && !string.IsNullOrEmpty(oldSsection.ImageId))
                    imageMongoRepository.Remove(oldSsection.ImageId);

                var imageData = fileHelper.FileBaseToByteArray(avatar);
                var image = new Image {Data = imageData, ImageMimeType = avatar.ContentType};
                imageMongoRepository.CreateOrUpdate(image);
                section.ImageId = image.Id.ToString();
            }
            sectionRepository.CreateOrUpdate(section);
        }

        public void RemoveSection(int sectionId)
        {
            var section = sectionRepository.GetById(sectionId);
            if (!string.IsNullOrEmpty(section.ImageId))
                imageMongoRepository.Remove(section.ImageId);
            sectionRepository.Remove(sectionId);
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