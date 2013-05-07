using Mvc.Mailer;

namespace Mvc.Mailers
{
    public sealed class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public MvcMailMessage Register()
        {
            ViewBag.Data = "John";
            return Populate(x =>
                {
                    x.Subject = "Register";
                    x.ViewName = "Register";
                    x.To.Add("Sergey.Kharchenko@airbionicfx.com");
                });
        }
    }
}