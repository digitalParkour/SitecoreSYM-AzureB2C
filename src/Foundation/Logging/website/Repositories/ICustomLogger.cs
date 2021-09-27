using SYMB2C.Foundation.Logging.Helpers;

namespace SYMB2C.Foundation.Logging.Repositories
{
   public interface ICustomLogger
    {
        void LogMessage(string logAppender, string message, Logtype logtype);
    }
}
