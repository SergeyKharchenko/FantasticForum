using System.Data.Entity;
using System.Web;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Concrete
{
    public class UserUnitOfWork : AbstractUserUnitOfWork
    {
        private readonly IRepository<Image> imageMongoRepository;
        private readonly IFileAssistant fileAssistant;

        public UserUnitOfWork(DbContext context, IRepository<User> repository,
                                 IRepository<Image> imageMongoRepository,
                                 IFileAssistant fileAssistant)
            : base(context, repository)
        {
            this.imageMongoRepository = imageMongoRepository;
            this.fileAssistant = fileAssistant;
        }

        public override CrudUtilityModel<Section> CreateOrUpdateUser(User user, HttpPostedFileBase avatar)
        {
            throw new System.NotImplementedException();
        }
    }
}