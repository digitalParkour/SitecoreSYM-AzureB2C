using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SYMB2C.Foundation.Logging.Repositories
{
    public static class RedirectManagerLog
    {
        private static readonly Object LogLock = new object();
        private static ILog log { get; set; }
        public static ILog Log
        {
            get
            {
                if (log != null) return log;
                lock (LogLock)
                {
                    return log ?? (log = LogManager.GetLogger(typeof(RedirectManagerLog)));
                }
            }
        }
    }
}