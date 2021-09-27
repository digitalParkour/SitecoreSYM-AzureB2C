using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Sitecore.Diagnostics;
using SYMB2C.Feature.Login.Models.Identity;
using SYMB2C.Foundation.Account.Extensions;
using SYMB2C.Foundation.Common.Attributes;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.Logging.Enum;
using SYMB2C.Foundation.Logging.Repositories;
using SYMB2C.Foundation.Mail.Repositories;
using static SYMB2C.Foundation.Common.Constants.AuthenticationFilter;

namespace SYMB2C.Feature.Login.Controllers
{
    /// <summary>
    /// Identity API Controller.
    /// </summary>
    [IdentityBasicAuthentication]
    [Authorize]
    public class IdentityController : ApiController
    {
        public static readonly string TOTPSecret = Sitecore.Configuration.Settings.GetSetting("B2C_Verification_TOTP_Secret");
        public static readonly int TOTPStep = Sitecore.Configuration.Settings.GetIntSetting("B2C_Verification_TOTP_Step", 360);

        private readonly IEventLogger eventLogger;
        private readonly IAccountService accountService;
        private readonly IMailService mailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityController"/> class.
        /// </summary>
        /// <param name="accountService">AccountService object.</param>
        /// <param name="eventLogger">EventLogger object.</param>
        public IdentityController(IAccountService accountService, IEventLogger eventLogger, IMailService mailService)
        {
            this.accountService = accountService;
            this.eventLogger = eventLogger;
            this.mailService = mailService;
        }

        /// <summary>
        /// Identity Signin Async.
        /// </summary>
        /// <returns>Json response.</returns>
        [HttpPost]
        [Route("identity/signin")]
        public async Task<IHttpActionResult> SignInAsync()
        {
            Assert.IsNotNull(this.Request.Content, "Content cannot be null");

            // Read the input claims from the request body
            var input = await this.Request.Content.ReadAsStringAsync();

            // Check input content value
            if (string.IsNullOrEmpty(input))
            {
                Log.Warn($"{nameof(this.SignInAsync)}: input is empty", this);
                return this.Content(HttpStatusCode.BadRequest, new B2CResponseContent("Request content is empty", HttpStatusCode.BadRequest));
            }

            // Convert the input string into InputClaimsModel object
            var inputClaims = JsonConvert.DeserializeObject(input, typeof(SigninInputClaimsModel)) as SigninInputClaimsModel;

            var loggerData = new NameValueCollection { { "Legacy Username", $"{inputClaims?.username}" } };
            this.eventLogger.LogEvent(Event.Identity, "B2C api - Seamless migration - legacy signin attempt", loggerData, Level.INFO);

            if (inputClaims == null)
            {
                return this.Content(HttpStatusCode.Conflict, new B2CResponseContent("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            var legacyUsername = inputClaims.username;
            var password = inputClaims.password;

            var isAuthenticated = this.accountService.CheckAccountPassword(legacyUsername, password);

            loggerData.Add(nameof(isAuthenticated), isAuthenticated ? "true" : "false");
            this.eventLogger.LogEvent(Event.Identity, $"B2C api - Seamless migration - legacy signin result: {isAuthenticated}", loggerData, Level.INFO);

            if (isAuthenticated)
            {
                var outputClaims = new SigninOutputClaimsModel
                {
                    tokenSuccess = isAuthenticated,
                    migrationRequired = !isAuthenticated
                };

                return this.Ok(outputClaims);
            }

            return this.Content(HttpStatusCode.Conflict, new B2CResponseContent(ReasonPhrase.InvalidBoth, HttpStatusCode.Conflict));
        }

        /// <summary>
        /// Identity Get Account Number Async.
        /// </summary>
        /// <returns>Json response.</returns>
        [HttpPost]
        [Route("identity/getaccountnumber")]
        public async Task<IHttpActionResult> GetAccountNumberAsync()
        {
            Assert.IsNotNull(this.Request.Content, "Content cannot be null");

            // Read the input claims from the request body
            var input = await this.Request.Content.ReadAsStringAsync();

            // Check input content value
            if (string.IsNullOrEmpty(input))
            {
                Log.Warn($"{nameof(this.GetAccountNumberAsync)}: input should not be empty", this);
                return this.Content(HttpStatusCode.BadRequest, new B2CResponseContent("Request content is empty", HttpStatusCode.BadRequest));
            }

            var outputClaim = new GetAccountNumberOutputClaimsModel();

            // Convert the input string into InputClaimsModel object
            if (JsonConvert.DeserializeObject(input, typeof(GetAccountNumberInputClaimsModel)) is GetAccountNumberInputClaimsModel inputClaims)
            {
                var userId = inputClaims?.legacyId;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    // Fall back to email as username
                    userId = inputClaims?.email;
                }

                try
                {
                    var accounts = this.accountService.GetAccounts(userId);
                    var accountNumbers = accounts.SerializeAccountList();

                    var loggerData = new NameValueCollection { { "UserId", $"{userId}" }, { "Accounts", accountNumbers } };
                    this.eventLogger.LogEvent(Event.Identity, "B2C getaccountnumber api - Loading account numbers", loggerData, Level.INFO);

                    if (!string.IsNullOrWhiteSpace(accountNumbers))
                    {
                        outputClaim.accountNumbers = accountNumbers;
                        return this.Ok(outputClaim);
                    }
                }
                catch (Exception ex)
                {
                    var loggerData = new NameValueCollection { { "UserId", $"{userId}" } };
                    this.eventLogger.LogEvent(Event.Identity, "B2C getaccountnumber api - unexpected error", loggerData, Level.ERROR, ex);
                    return this.Ok(outputClaim);
                }
            }

            Log.Info($"{nameof(this.GetAccountNumberAsync)}: Returning empty claim", this);
            outputClaim.accountNumbers = string.Empty;
            return this.Ok(outputClaim);
        }
    }
}