namespace Mvc.Infrastructure.Assistants.Abstract
{
    public interface IMailAssistant
    {
        void SendMail(string subject, string text);
    }
}