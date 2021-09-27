using System.Net.Mail;

namespace SYMB2C.Foundation.Mail.Repositories
{
    public interface IMailService
    {
        void SendMail(MailMessage emailMessage);
    }
}
