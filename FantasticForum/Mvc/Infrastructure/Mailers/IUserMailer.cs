using Mvc.Mailer;

namespace Mvc.Infrastructure.Mailers
{ 
	public interface IUserMailer
	{
		MvcMailMessage Register(string email, string url);
	}
}