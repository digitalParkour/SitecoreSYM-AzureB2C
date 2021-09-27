using System;
using System.Net.Mail;
using log4net;
using SYMB2C.Foundation.Logging.Helpers;

namespace SYMB2C.Foundation.Logging.Repositories
{
    public class EmailLogger : IEmailLogger
    {
        private ILog logger;

        public ILog Logger
        {
            get
            {
                if (this.logger == null)
                {
                    this.logger = Sitecore.Diagnostics.LoggerFactory.GetLogger(Constants.EMAIL_LOG);
                }

                return this.logger;
            }
        }

        public void LogEmail(MailMessage email)
        {
            var subject = email?.Subject;
            var from = email?.From?.ToString();
            var to = email?.To?.ToString();
            var cc = email?.CC?.ToString();
            var body = email?.Body;

            this.Logger.Info($"Sending Subject: {subject}; From: {from}; To: {to}; CC: {cc}; Message: {body}");
        }

        public void LogError(MailMessage email, Exception ex)
        {
            var subject = email?.Subject;
            var from = email?.From?.ToString();
            var to = email?.To?.ToString();
            var cc = email?.CC?.ToString();
            var body = email?.Body;

            this.Logger.Error($"Failed to send Subject: {subject}; From: {from}; To: {to}; CC: {cc}; Message: {body}", ex);
        }
    }
}