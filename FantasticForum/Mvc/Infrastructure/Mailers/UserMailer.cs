using Mvc.Mailer;

namespace Mvc.Infrastructure.Mailers
{ 
	public sealed class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName = "_Layout";
		}

		public MvcMailMessage Register(string email, string url)
		{
			ViewBag.Link = url;
			return Populate(x =>
			{
				x.Subject = "Registration Confirmation";
				x.ViewName = "Register";
				x.To.Add(email);
			});
		}
	}
}