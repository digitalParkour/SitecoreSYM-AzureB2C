using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Script.Serialization;
using log4net;
using SYMB2C.Foundation.DependencyInjection;
using SYMB2C.Foundation.Logging.Enum;
using SYMB2C.Foundation.Logging.Helpers;

namespace SYMB2C.Foundation.Logging.Repositories
{
    /// <summary>
    /// Nes Event Logger.
    /// </summary>
    [Service(typeof(IEventLogger), Lifetime = Lifetime.Singleton)]
    public class EventLogger : IEventLogger
    {
        private readonly ILog logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogger"/> class.
        /// </summary>
        public EventLogger()
        {
            this.logger = Sitecore.Diagnostics.LoggerFactory.GetLogger(Constants.EVENT_LOG);
        }

        /// <summary>
        /// Log Event.
        /// </summary>
        /// <param name="eventName">Enum Event.</param>
        /// <param name="message">Message.</param>
        /// <param name="data">Data.</param>
        /// <param name="level">Enum Level.</param>
        /// <param name="exception">Exception object.</param>
        public void LogEvent(Event eventName, string message, NameValueCollection data = null, Level level = Level.INFO, Exception exception = null)
        {
            var dataJson = data == null ? "{}" : new JavaScriptSerializer().Serialize(data.AllKeys.ToDictionary(x => x, x => data.GetValues(x)));
            var msg = $"{eventName}|{message}|{dataJson}";

            switch (level)
            {
                case Level.DEBUG:
                    this.logger.Debug(msg, exception);
                    break;
                case Level.WARN:
                    this.logger.Warn(msg);
                    break;
                case Level.ERROR:
                    this.logger.Error(msg, exception);
                    break;
                case Level.FATAL:
                    this.logger.Fatal(msg, exception);
                    break;
                default:
                    this.logger.Info(msg);
                    break;
            }
        }
    }
}