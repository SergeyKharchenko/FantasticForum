using Mvc.Mailer;

namespace Mvc.Mailers
{ 
	public interface IUserMailer
	{
		MvcMailMessage Register();
	}
}