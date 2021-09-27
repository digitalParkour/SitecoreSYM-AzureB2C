using System.Linq;
using System.Net.Mail;
using System.Net.Mime;

namespace SYMB2C.Foundation.Mail.Models
{
    /// <summary>
    /// Object wrapper around Mail Template Item.
    /// </summary>
    public class MailTemplateModel
    {
        public string From { get; set; }

        public string Subject { get; set; }

        public string HtmlBody { get; set; }

        public string PlainBody { get; set; }

        private bool IsBodyHtml => !string.IsNullOrEmpty(this.HtmlBody);

        /// <summary>
        /// Initializes a new instance of the <see cref="MailTemplateModel"/> class.
        /// </summary>
        /// <param name="item">Sitecore data item.</param>
        public MailTemplateModel(Sitecore.Data.Items.Item item)
        {
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item["From"]))
                {
                    this.From = item["From"];
                }

                if (!string.IsNullOrEmpty(item["HtmlBody"]))
                {
                    this.HtmlBody = item["HtmlBody"];
                }

                if (!string.IsNullOrEmpty(item["PlainBody"]))
                {
                    this.PlainBody = item["PlainBody"];
                }

                if (!string.IsNullOrEmpty(item["Subject"]))
                {
                    this.Subject = item["Subject"];
                }
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.From) && (!string.IsNullOrEmpty(this.HtmlBody) || !string.IsNullOrEmpty(this.PlainBody)) && !string.IsNullOrEmpty(this.Subject);
        }

        public MailMessage ToMailMessage(string to, params string[] tokens)
        {
            if (!string.IsNullOrWhiteSpace(to))
            {
                MailMessage mailMessage;
                var notBoth = string.IsNullOrWhiteSpace(this.HtmlBody) || string.IsNullOrWhiteSpace(this.PlainBody);
                if (notBoth)
                {
                    mailMessage = new MailMessage(this.From, to)
                    {
                        IsBodyHtml = this.IsBodyHtml,
                        Body = this.ReplaceTokens(this.IsBodyHtml ? this.HtmlBody : this.PlainBody, tokens),
                        Subject = this.Subject
                    };
                }
                else
                {
                    mailMessage = new MailMessage(
                        this.From,
                        to,
                        this.Subject,
                        this.ReplaceTokens(this.PlainBody, tokens));

                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(this.ReplaceTokens(this.HtmlBody, tokens), new ContentType(MediaTypeNames.Text.Html));
                    mailMessage.AlternateViews.Add(alternate);
                }

                return mailMessage;
            }

            return null;
        }

        public string ReplaceTokens(string message, string[] tokens)
        {
            if (tokens == null || !tokens.Any())
            {
                return message;
            }

            return string.Format(message, tokens);
        }
    }
}