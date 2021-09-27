using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using SYMB2C.Foundation.Account.Extensions;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.Logging.Enum;
using Sitecore.ContentSearch.Utilities;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using Sitecore.Owin.Authentication.Services;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;

namespace SYMB2C.Feature.Login.Pipelines
{
    public partial class AzureAdB2CIdentityProviderProcessor : IdentityProvidersProcessor
    {
        public static string SignInPolicyId = Sitecore.Configuration.Settings.GetSetting("ida:SignInPolicyId");
        // public static string SignUpPolicyId = Sitecore.Configuration.Settings.GetSetting("ida:SignUpPolicyId");
        // public static string SignUpSignInPolicyId = Sitecore.Configuration.Settings.GetSetting("ida:SignUpSignInPolicyId");

        private OpenIdConnectAuthenticationOptions CreateSignInOptions()
        {
            var policy = SignInPolicyId;
            var identityProvider = this.GetIdentityProvider();

            return new OpenIdConnectAuthenticationOptions
            {
                Caption = identityProvider.Caption,
                MetadataAddress = string.Format(WellKnownMetadata, Tenant, policy),
                AuthenticationType = this.GetAuthenticationType(),
                AuthenticationMode = AuthenticationMode.Passive,
                //RequireHttpsMetadata = RequireHttps,

                // These are standard OpenID Connect parameters, with values pulled from settings
                ClientId = ClientId,
                Scope = $"openid profile offline_access {DefaultScope}",
                //ResponseType = "id_token",
                RedirectUri = RedirectUri, //SK RedirectIframeUri
                PostLogoutRedirectUri = PostLogoutRedirectUri,
                UseTokenLifetime = false,
                CookieManager = this.CookieManager,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = this.SignIn_OnRedirectToIdentityProvider,
                    AuthenticationFailed = this.SignIn_OnAuthenticationFailed,
                    SecurityTokenValidated = this.SignIn_OnSecurityTokenValidated,
                },
            };
        }

        private Task SignIn_OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            var isLogout = notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout;
            var action = isLogout ? "Out" : "In";
            this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignIn, $"Attempting Sign {action}");

            return Task.CompletedTask;
        }

        private Task SignIn_OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();

            if (context.Exception.Message.Contains("IDX21323"))
            {
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignIn, "B2C IDX21323 - empty nonce, resending challenge", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);
                context.OwinContext.Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/identity/externallogincallback?ReturnUrl=%2fmy-account&sc_site=SYMB2C&authenticationSource=Default" },
                    new string[] { AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.SignInPolicyId });
            }
            else if (context.ProtocolMessage.ErrorDescription != null && context.ProtocolMessage.ErrorDescription.Contains("AADB2C90118"))
            {
                // If the user clicked the reset password link, redirect to the reset password route
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignIn, "B2C, redirecting to password reset", null, Foundation.Logging.Enum.Level.INFO);
                context.Response.Redirect("/user/password/reset");
            }
            else if (context.ProtocolMessage.ErrorDescription != null && context.ProtocolMessage.ErrorDescription.Contains("AADB2C90091"))
            {
                this.eventLogger.LogEvent(
                    Foundation.Logging.Enum.Event.SignIn,
                    "B2C, canceled signup, redirecting to home page",
                    new NameValueCollection { { "policy", context.Options.AuthenticationType } },
                    Foundation.Logging.Enum.Level.INFO);

                context.Response.Redirect("/"); // sign up is ejected from iframe, so return to home page always
            }
            else if (context.Exception.Message?.Contains("access_denied") ?? false)
            {
                var policy = context.Options.AuthenticationType;
                var redirect = this.GetStartUrl(policy);

                // If the user canceled, redirect back to the home page
                this.eventLogger.LogEvent(
                    Foundation.Logging.Enum.Event.SignIn,
                    "B2C - canceled",
                    new NameValueCollection { { "policy", policy } },
                    Foundation.Logging.Enum.Level.ERROR,
                    context.Exception);

                context.Response.Redirect(redirect);
            }
            else if (context.Exception is HttpException httpException)
            {
                var policy = context.Options.AuthenticationType;
                var redirect = this.GetErrorUrl(policy);

                this.eventLogger.LogEvent(
                    Foundation.Logging.Enum.Event.SignIn,
                    "B2C Error",
                    new NameValueCollection { { "policy", policy } },
                    Foundation.Logging.Enum.Level.ERROR,
                    context.Exception);

                context.Response.Redirect(redirect);
            }
            else
            {
                string errorMessage = (!string.IsNullOrEmpty(context.ProtocolMessage.Error)) ? context.ProtocolMessage.ErrorDescription : context.Exception.Message;

                var policy = context.Options.AuthenticationType;
                var redirect = this.GetErrorUrl(policy);

                this.eventLogger.LogEvent(
                    Foundation.Logging.Enum.Event.SignIn,
                    $"B2C Error - {errorMessage}",
                    new NameValueCollection { { "policy", policy } },
                    Foundation.Logging.Enum.Level.ERROR,
                    context.Exception);

                context.Response.Redirect(redirect);
            }

            return Task.CompletedTask;
        }

        private Task SignIn_OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            try
            {

                var identityProvider = this.GetIdentityProvider();
                var identity = notification.AuthenticationTicket.Identity;

                // Map claims
                foreach (var claimTransformationService in identityProvider.Transformations)
                {
                    claimTransformationService.Transform(identity, new TransformationContext(this.FederatedAuthenticationConfiguration, identityProvider));
                }

                // :::::::: LOOKUP ACCOUNT LIST :::::::::::::::::::::::::::::::::::::::::::::::::
                // - Inject as Claim for profile property mapping (this can eventually come from B2C) 

                var accountListClaim = identity.FindFirst(CustomClaim.AccountList);

                // This is now loaded in the Profile Service
                var needClaim = false; //string.IsNullOrWhiteSpace(accountListClaim?.Value);

                // Get UserId
                // Honor Legacy ID first if avaliable
                var email = identity.FindFirstValue(CustomClaim.Email);
                var userId = identity.FindFirstValue(CustomClaim.LegacyId);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    // Fall back to email as username
                    userId = email;
                }

                var accountService = ServiceLocator.ServiceProvider.GetService(typeof(IAccountService)) as IAccountService;

                if (needClaim)
                {
                    Log.Info($"SYMB2C LOGIN ATTEMPT:: NEEDS ACCOUNT LIST - email:{email} userId:{userId}", this);
                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        var accounts = accountService.GetAccounts(userId);
                        if (accounts?.Any() ?? false)
                        {
                            if (accountListClaim != null)
                            {
                                identity.RemoveClaim(accountListClaim);
                            }

                            identity.AddClaim(new Claim("extension_PowerAccountList", accounts.SerializeAccountList()));
                        }
                    }
                }
                else
                {
                    Log.Info($"SYMB2C LOGIN ATTEMPT:: HAS ACCOUNT LIST - email:{email} userId:{userId}", this);
                }

                // ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                var logData = new NameValueCollection { { "email", email }, { "userId", userId } };

                // ::::: WORKAROUND :::::
                // Send profile data to SYMB2C for CSR search and existing Stored Procs
                var verifyProps = new VerifyProfileParameters
                {
                    Email = email,
                    UserId = userId,
                    FirstName = identity.FindFirstValue(CustomClaim.FirstName),
                    LastName = identity.FindFirstValue(CustomClaim.LastName),
                    Phone = identity.FindFirstValue(CustomClaim.Phone),
                    BusinessName = identity.FindFirstValue(CustomClaim.BusinessName),
                };
                var isNew = identity.FindFirstValue(CustomClaim.IsNew)?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
                if (isNew)
                {
                    this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignUp, "New user Signup", logData);
                }

                accountService.VerifyProfile(verifyProps, isNew);
                // ::: End Workaround :::

                // Authenticate
                // notification.AuthenticationTicket = new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);
                foreach (var claim in identity.Claims)
                {
                    logData.Add(claim.Type, claim.Value);
                }

                // Fire goal - Login - Personal
                using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
                {
                    try
                    {
                        var reference = new IdentifiedContactReference("NesLogin", verifyProps.Email);

                        Contact contact = client.Get<Contact>(reference, new ContactExpandOptions() { });

                        Guid channelId = Guid.Parse("{B418E4F2-1013-4B42-A053-B6D4DCA988BF}");
                        string userAgent = HttpContext.Current.Request.UserAgent;
                        var interaction = new Interaction(contact, InteractionInitiator.Contact, channelId, userAgent);

                        Guid goalID = Guid.Parse("{F981437C-ACD8-44F6-BB2C-5B20930C9B97}"); // ID of goal item
                        var goal = new Goal(goalID, DateTime.UtcNow);

                        Sitecore.Data.Items.Item personalLogin = Sitecore.Context.Database.Items.GetItem("{F981437C-ACD8-44F6-BB2C-5B20930C9B97}");
                        goal.EngagementValue = Convert.ToInt32(personalLogin["Points"]);

                        interaction.Events.Add(goal);

                        client.AddInteraction(interaction);

                        client.Submit();
                    }
                    catch (Exception ex)
                    {
                        // Manage exceptions
                        this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignInGoalTrigger, "Login Goal Trigger Failed", null, Level.ERROR, ex);
                    }
                }

                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignIn, "User Logged In", logData);
            }
            catch (Exception ex)
            {
                var policy = notification.Options.AuthenticationType;
                var redirect = this.GetErrorUrl(policy);

                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignIn, "B2C Error - signin execution", new NameValueCollection { { "policy", policy } }, Foundation.Logging.Enum.Level.ERROR, ex);
                notification.Response.Redirect(redirect);
            }

            return Task.CompletedTask;
        }

        private string GetErrorUrl(string policy)
        {
            if (!string.IsNullOrWhiteSpace(policy))
            {
                if (policy.Equals(SignInIframeDayPolicyId))
                {
                    return "/b2c/signin-iframe-day-error.html";
                }
                else if (policy.Equals(SignInIframeNightPolicyId))
                {
                    return "/b2c/signin-iframe-night-error.html";
                }
            }

            return "/errorpages/symb2c.html";
        }

        private string GetStartUrl(string policy)
        {
            if (!string.IsNullOrWhiteSpace(policy))
            {
                if (policy.Equals(SignInIframeDayPolicyId))
                {
                    return "/user/dayframein";
                }
                else if (policy.Equals(SignInIframeNightPolicyId))
                {
                    return "/user/nightframein";
                }
            }

            return "/";
        }
    }
}