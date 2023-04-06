using SYMB2C.Foundation.Common.Constants;
using SYMB2C.Foundation.Logging.Helpers;
using SYMB2C.Foundation.Logging.Repositories;
using Sitecore;
using System;
using System.Net.Mail;
using Convert = System.Convert;

namespace SYMB2C.Foundation.Mail
{
    public class SendMailNotification
    {
        private static readonly ILogRepository _logRepository;
        protected ICustomLogger _customLogger;
        public string StrSmtphost { get; set; }
        public int IPort { get; set; }
        public bool StrSSL { get; set; }
        public string Strusername { get; set; }
        public string Strpassword { get; set; }

        public SendMailNotification()
        {
            this.StrSmtphost = Sitecore.Context.Site.SiteInfo.Properties.Get("SmtpHost");
            this.IPort = Convert.ToInt32(Sitecore.Context.Site.SiteInfo.Properties.Get("SmtpPort"));
            this.StrSSL = Convert.ToBoolean(Sitecore.Context.Site.SiteInfo.Properties.Get("SmtpSSL"));
            this.Strusername = Context.Site.SiteInfo.Properties.Get("SmtpUserName");
            this.Strpassword = Context.Site.SiteInfo.Properties.Get("SmtpPassword");

            this._customLogger = new CustomLogger();
        }

        public bool PrepareSendMail(MailMessage mailMessage)
        {
            _customLogger.LogMessage(SYMB2CConstants.CustomLogFileAppender, "Sending email", Logtype.INFO);
            try
            {
                SmtpClient smtpClient = new SmtpClient
                {
                    EnableSsl = StrSSL,
                    Host = StrSmtphost,
                    Port = IPort,
                    Credentials = new System.Net.NetworkCredential(Strusername, Strpassword)
                };
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _customLogger.LogMessage(SYMB2CConstants.CustomLogFileAppender, ex.StackTrace + ex.Message, Logtype.ERROR);
                _logRepository.LogFormattedError(ex.StackTrace);
                return false;
            }
        }
    }
}