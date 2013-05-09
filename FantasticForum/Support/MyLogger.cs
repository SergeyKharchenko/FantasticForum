using NLog;

namespace Support
{
    public class MyLogger : ILogger
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void WriteToLog(string message)
        {
            logger.Info(message);
        }
    }
}