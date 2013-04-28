using Models;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using System.Data.Entity;
using System.Web;

namespace Mvc.Infrastructure.UnitsOfWork.Concrete
{
    public class UserUnitOfWork : AbstractUserUnitOfWork
    {
        private readonly IEntityWithImageAssistant<User> imageAssistant;

        public UserUnitOfWork(DbContext context,
                              IRepository<User> repository,
                              IEntityWithImageAssistant<User> imageAssistant)
            : base(context, repository)
        {
            this.imageAssistant = imageAssistant;
        }

        public override User RegisterUser(User user, HttpPostedFileBase avatar)
        {
            string avatarId = null;
            if (avatar != null)
                avatarId = imageAssistant.CreateImage(avatar);
            user.ImageId = avatarId;
            return Create(user);
        }
    }
}