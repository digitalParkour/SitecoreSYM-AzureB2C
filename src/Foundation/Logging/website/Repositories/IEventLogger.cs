using System;
using System.Collections.Specialized;
using SYMB2C.Foundation.Logging.Enum;

namespace SYMB2C.Foundation.Logging.Repositories
{
    /// <summary>
    /// Abstraction Event Logger.
    /// </summary>
    public interface IEventLogger
    {
        void LogEvent( Event eventName, string message, NameValueCollection data = null, Level level = Level.INFO, Exception exception = null);
    }
}
