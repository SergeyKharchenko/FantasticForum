using System.Web;
using Models;
using Models.Abstract;
using Mvc.Infrastructure.Abstract;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Concrete
{
    public class EntityWithImageAssistant<TEntity>
        : IEntityWithImageAssistant<TEntity> where TEntity : SqlEntityWithImage
    {
        private readonly IRepository<TEntity> repository;
        private readonly IRepository<Image> imageMongoRepository;
        private readonly IFileAssistant fileAssistant;

        public EntityWithImageAssistant(IRepository<TEntity> repository,
                                        IRepository<Image> imageMongoRepository,
                                        IFileAssistant fileAssistant)
        {
            this.repository = repository;
            this.imageMongoRepository = imageMongoRepository;
            this.fileAssistant = fileAssistant;
        }

        public string CreateImage(HttpPostedFileBase avatar)
        {
            var imageData = fileAssistant.FileBaseToByteArray(avatar);
            var image = new Image { Data = imageData, ImageMimeType = avatar.ContentType };
            image = imageMongoRepository.Create(image);
            return image.Id.ToString();
        }

        public void RemoveImageFrom(TEntity entity)
        {
            if (entity.Id != 0 && !string.IsNullOrEmpty(entity.ImageId))
                imageMongoRepository.Remove(entity.ImageId);
        }

        public string GetImageId(int entityId)
        {
            var section = repository.GetById(entityId);
            return section == null ? null : section.ImageId;
        }

        public ImageUtilityModel GetImageFromEntityWithId(int entityId)
        {
            var section = repository.GetById(entityId);
            if (string.IsNullOrEmpty(section.ImageId))
                return new ImageUtilityModel(false);
            var image = imageMongoRepository.GetById(section.ImageId);
            return new ImageUtilityModel(true, image.Data, image.ImageMimeType);
        }
    }
}