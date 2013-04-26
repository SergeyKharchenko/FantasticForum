using System.Web;
using Models.Abstract;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Abstract
{
    public interface IEntityWithImageAssistant<in TEntity> where TEntity : SqlEntityWithImage
    {
        string CreateImage(HttpPostedFileBase avatar);
        void RemoveImageFrom(TEntity entity);
        string GetImageId(int entityId);
        ImageUtilityModel GetImageFromEntityWithId(int entityId);
    }
}