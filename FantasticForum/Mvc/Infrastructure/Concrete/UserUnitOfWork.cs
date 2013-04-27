using System;
using System.Data.Entity;
using System.Globalization;
using System.Web;
using System.Web.Security;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.UtilityModels;

namespace Mvc.Infrastructure.Concrete
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

        public override string RegisterUser(User user, HttpPostedFileBase avatar)
        {
            string avatarId = null;
            if (avatar != null)
                avatarId = imageAssistant.CreateImage(avatar);
            user.ImageId = avatarId;
            user = Create(user);
            var formsAuthenticationTicket = new FormsAuthenticationTicket(user.Id.ToString(CultureInfo.InvariantCulture),
                                                                          false,
                                                                          (int) FormsAuthentication.Timeout.TotalMinutes);
            return FormsAuthentication.Encrypt(formsAuthenticationTicket);
        }
    }
}