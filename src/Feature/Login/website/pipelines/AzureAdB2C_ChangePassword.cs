using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using SYMB2C.Foundation.Common.Constants;
using Sitecore.Diagnostics;

namespace SYMB2C.Feature.Login.Pipelines
{
    public partial class AzureAdB2CIdentityProviderProcessor
    {
        public static string PasswordChangePolicyId = Sitecore.Configuration.Settings.GetSetting("ida:PasswordChangePolicyId");

        private OpenIdConnectAuthenticationOptions CreateChangePasswordOptions()
        {
            var policy = PasswordChangePolicyId;

            //var identityProvider = GetIdentityProvider();
            return new OpenIdConnectAuthenticationOptions
            {
                // For each policy, give OWIN the policy-specific metadata address, and
                // set the authentication type to the id of the policy
                MetadataAddress = string.Format(WellKnownMetadata, Tenant, policy),

                AuthenticationType = policy,
                AuthenticationMode = AuthenticationMode.Passive,
                //RequireHttpsMetadata = RequireHttps,

                // These are standard OpenID Connect parameters, with values pulled from settings
                ClientId = ClientId,
                Scope = OpenIdConnectScope.OpenId,
                ResponseType = OpenIdConnectResponseType.IdToken,
                RedirectUri = RedirectUri, //SK RedirectIframeUri
                PostLogoutRedirectUri = PostLogoutRedirectUri,
                UseTokenLifetime = false,
                CookieManager = this.CookieManager,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = this.ChangePassword_OnRedirectToIdentityProvider,
                    AuthenticationFailed = this.ChangePassword_OnAuthenticationFailed,
                    SecurityTokenValidated = this.ChangePassword_OnSecurityTokenValidated,
                },
            };
        }

        private Task ChangePassword_OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ChangePassword, "Attempting Password Change");

            return Task.CompletedTask;
        }

        private Task ChangePassword_OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();

            if (context.ProtocolMessage.ErrorDescription != null && context.ProtocolMessage.ErrorDescription.Contains("AADB2C90118"))
            {
                // If the user clicked the reset password link, redirect to the reset password route
                context.Response.Redirect("/user/password/reset");
            }
            else if (context.Exception.Message?.Contains("access_denied") ?? false)
            {
                // If the user canceled, redirect back to the home page
                context.Response.Redirect(PageRedirect.MY_ACCOUNT);
            }
            else if (context.Exception is HttpException httpException)
            {
                Log.Error("B2C HttpException", httpException, this);
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ChangePassword, "B2C Error", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);
                context.Response.Redirect("/");
            }
            else
            {
                string errorMessage = (!string.IsNullOrEmpty(context.ProtocolMessage.Error)) ? context.ProtocolMessage.ErrorDescription : context.Exception.Message;

                Log.Error($"B2C Exception: {errorMessage}", context.Exception, this);
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ChangePassword, $"B2C Error - {errorMessage}", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);

                context.Response.Redirect("/");
            }

            return Task.CompletedTask;
        }

        private Task ChangePassword_OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            var identity = notification.AuthenticationTicket.Identity;

            var logData = new NameValueCollection();
            foreach (var claim in identity.Claims)
            {
                logData.Add(claim.Type, claim.Value);
            }

            this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ChangePassword, "Password Changed", logData);
            return Task.CompletedTask;
        }
    }
}