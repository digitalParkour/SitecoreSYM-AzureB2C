using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SYMB2C.Feature.Login.Models.Identity;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.Logging.Enum;
using SYMB2C.Foundation.Logging.Repositories;
using SYMB2C.Foundation.Mail.Models;
using SYMB2C.Foundation.Mail.Repositories;
using OtpNet;
using Sitecore.Diagnostics;

namespace SYMB2C.Feature.Login.Controllers
{
    /// <summary>
    /// Identity API Controller.
    /// </summary>
    public class VerificationController : ApiController
    {
        public static readonly string TOTPSecret = Sitecore.Configuration.Settings.GetSetting("B2C_Verification_TOTP_Secret");
        public static readonly int TOTPStep = Sitecore.Configuration.Settings.GetIntSetting("B2C_Verification_TOTP_Step", 360);

        private readonly IEventLogger eventLogger;
        private readonly IAccountService accountService;
        private readonly IMailService mailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationController"/> class.
        /// </summary>
        /// <param name="accountService">AccountService object.</param>
        /// <param name="eventLogger">EventLogger object.</param>
        public VerificationController(IAccountService accountService, IEventLogger eventLogger, IMailService mailService)
        {
            this.accountService = accountService;
            this.eventLogger = eventLogger;
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("identity/send")]
        public async Task<IHttpActionResult> Send()
        {
            var input = await this.Request.Content.ReadAsStringAsync();

            // If not data came in, then return
            if (string.IsNullOrEmpty(input))
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Request content is null", HttpStatusCode.Conflict));
            }

            // Convert the input string into EmailInputClaimsModel object
            var inputClaims = EmailInputClaimsModel.Parse(input);

            var loggerData = new NameValueCollection { { "Email", $"{inputClaims?.email}" } };
            this.eventLogger.LogEvent(Event.Identity, "B2C send email api", loggerData, Level.INFO);

            if (inputClaims == null)
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            if (string.IsNullOrEmpty(inputClaims.email))
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Email address is missing.", HttpStatusCode.Conflict));
            }

            try
            {
                var totp = new Totp(
                    Encoding.UTF8.GetBytes(inputClaims.email.Trim().ToLowerInvariant() + TOTPSecret),
                    step: TOTPStep,
                    totpSize: 6);

                string code = totp.ComputeTotp();

                var db = Sitecore.Data.Database.GetDatabase("web");
                var mailTemplateItem = new MailTemplateModel(db.GetItem(Templates.MailTemplates.B2CVerify.ID));
                if (mailTemplateItem.IsValid())
                {
                    if (mailTemplateItem.HtmlBody.Contains("{0}") && mailTemplateItem.HtmlBody.Contains("{1}"))
                    {
                        var mailMessage = mailTemplateItem.ToMailMessage(inputClaims.email, inputClaims.email, code);
                        this.mailService.SendMail(mailMessage);
                    }
                    else
                    {
                        Log.Error($"B2C Verification email body expects two placeholders. Review email defn item {Templates.MailTemplates.B2CVerify.ID}. Unable to send email to {inputClaims.email}", this);
                    }
                }
                else
                {
                    Log.Error($"Invalid Email defn in {db.Name} db, {Templates.MailTemplates.B2CVerify.ID}. Unable to send email to {inputClaims.email}", this);
                }

            }
            catch (Exception ex)
            {
                this.eventLogger.LogEvent(Event.Identity, "B2C send email api failed", loggerData, Level.ERROR, ex);
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Internal error: " + ex.Message, HttpStatusCode.Conflict));
            }

            return this.Ok("Sent");
        }

        [HttpPost]
        [Route("identity/verify")]
        public async Task<IHttpActionResult> Verify()
        {
            var input = await this.Request.Content.ReadAsStringAsync();

            // If not data came in, then return
            if (string.IsNullOrEmpty(input))
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Request content is null", HttpStatusCode.Conflict));
            }

            // Convert the input string into EmailInputClaimsModel object
            var inputClaims = EmailInputClaimsModel.Parse(input);

            var loggerData = new NameValueCollection { { "Email", $"{inputClaims?.email}" }, { "Code", $"{inputClaims?.code}" } };
            this.eventLogger.LogEvent(Event.Identity, "B2C verify api", loggerData, Level.INFO);

            if (inputClaims == null)
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            if (string.IsNullOrEmpty(inputClaims.email))
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Email address is missing.", HttpStatusCode.Conflict));
            }

            if (string.IsNullOrEmpty(inputClaims.code))
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Verification code is missing.", HttpStatusCode.Conflict));
            }

            var totp = new Totp(
                Encoding.UTF8.GetBytes(inputClaims.email.Trim().ToLowerInvariant() + TOTPSecret),
                step: TOTPStep,
                totpSize: 6);

            if (totp.VerifyTotp(inputClaims.code.Trim(), out long timeWindowUsed))
            {
                this.eventLogger.LogEvent(Event.Identity, "B2C verify api - OK", loggerData, Level.INFO);
                return this.Ok();
            }
            else
            {
                this.eventLogger.LogEvent(Event.Identity, "B2C verify api - Invalid", loggerData, Level.INFO);
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("The verification provided is invalid or expired.", HttpStatusCode.Conflict));
            }
        }
    }
}