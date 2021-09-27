using log4net;
using SYMB2C.Foundation.DependencyInjection;
using SYMB2C.Foundation.Logging.Helpers;

namespace SYMB2C.Foundation.Logging.Repositories
{
    [Service(typeof(ICustomLogger))]
    public class CustomLogger : ICustomLogger
    {
        public ILog _logger;
        public void LogMessage(string logAppender, string message, Logtype logtype)
        {
            try
            {
                _logger = LogManager.GetLogger(logAppender);
            }
            catch
            {
                _logger = LogManager.GetLogger(logAppender);
            }
            switch (logtype)
            {
                case Logtype.DEBUG:
                    _logger.Debug(message);
                    break;
                case Logtype.ERROR:
                    _logger.Error(message);
                    break;
                case Logtype.INFO:
                    _logger.Info(message);
                    break;
                case Logtype.WARN:
                    _logger.Warn(message);
                    break;
                default:
                    break;
            }
        }
    }
}