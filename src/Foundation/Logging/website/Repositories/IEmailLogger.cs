using System;
using System.Net.Mail;

namespace SYMB2C.Foundation.Logging.Repositories
{
    public interface IEmailLogger
    {
        void LogEmail(MailMessage email);

        void LogError(MailMessage email, Exception ex);
    }
}
