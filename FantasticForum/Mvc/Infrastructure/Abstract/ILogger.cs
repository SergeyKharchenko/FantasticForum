namespace Mvc.Infrastructure.Abstract
{
    public interface ILogger
    {
        void WriteToLog(string message);
    }
}