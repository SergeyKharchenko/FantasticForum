using Mvc.Infrastructure.Abstract;
using NLog;

namespace Mvc.Infrastructure.Concrete
{
    class MyLogger : ILogger
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void WriteToLog(string message)
        {
            logger.Info(message);
        }
    }
}