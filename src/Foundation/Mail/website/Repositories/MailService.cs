using System;
using System.Linq;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Foundation.Logging.Repositories;
using SYMB2C.Foundation.Settings.Services;
using Sitecore;
using Sitecore.DependencyInjection;

namespace SYMB2C.Foundation.Mail.Repositories
{
    public class MailService : IMailService
    {
        private readonly IEmailLogger emailLogger;
        private readonly IXAFoundationSettings xaFoundationSettings;

        public MailService()
        {
            this.emailLogger = ServiceLocator.ServiceProvider.GetService<IEmailLogger>();
            this.xaFoundationSettings = ServiceLocator.ServiceProvider.GetService<IXAFoundationSettings>();
        }

        public MailService(IEmailLogger emailLogger, IXAFoundationSettings xaFoundationSettings)
        {
            this.emailLogger = emailLogger;
            this.xaFoundationSettings = xaFoundationSettings;
        }

        private static readonly string[] NonProdEnvironments = new[] { "LOCAL", "DEV", "QA-CM", "QA-CD" };
        private static readonly string VirtualMta = Sitecore.Configuration.Settings.GetSetting("SMTP.VirtualMta", string.Empty);

        public void SendMail(MailMessage mailMessage)
        {
            try
            {
                if (mailMessage != null)
                {
                    var env = this.xaFoundationSettings.GetEnvironment() ?? string.Empty;
                    if (NonProdEnvironments.Contains(env))
                    {
                        mailMessage.Subject = $"[{env}] {mailMessage.Subject}";
                    }

                    this.emailLogger.LogEmail(mailMessage);
                    if (!string.IsNullOrWhiteSpace(VirtualMta))
                    {
                        mailMessage.Headers.Add("x-virtual-mta", VirtualMta);
                    }

                    MainUtil.SendMail(mailMessage);
                }
            }
            catch (Exception ex)
            {
                this.emailLogger.LogError(mailMessage, ex);
            }
        }
    }
}