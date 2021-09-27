using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;

namespace SYMB2C.Feature.Login.Pipelines
{
    public partial class AzureAdB2CIdentityProviderProcessor : IdentityProvidersProcessor
    {

        public static string PasswordResetPolicyId = Sitecore.Configuration.Settings.GetSetting("ida:PasswordResetPolicyId");

        private OpenIdConnectAuthenticationOptions CreateResetPasswordOptions()
        {
            var policy = PasswordResetPolicyId;

            //var identityProvider = GetIdentityProvider();
            return new OpenIdConnectAuthenticationOptions
            {
                // For each policy, give OWIN the policy-specific metadata address, and
                // set the authentication type to the id of the policy
                MetadataAddress = string.Format(WellKnownMetadata, Tenant, policy),
                AuthenticationType = policy,
                AuthenticationMode = AuthenticationMode.Passive,

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
                    RedirectToIdentityProvider = this.ResetPassword_OnRedirectToIdentityProvider,
                    AuthenticationFailed = this.ResetPassword_OnAuthenticationFailed,
                    SecurityTokenValidated = this.ResetPassword_OnSecurityTokenValidated,
                },
            };
        }

        private Task ResetPassword_OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ResetPassword, "Attempting Password Reset");

            return Task.CompletedTask;
        }

        private Task ResetPassword_OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();

            if (context.Exception.Message.Contains("IDX21323"))
            {
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.SignIn, "B2C IDX21323 - empty nonce, reset password redirect to signin", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);
                context.OwinContext.Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/identity/externallogincallback?ReturnUrl=%2fmy-account&sc_site=SYMB2C&authenticationSource=Default" },
                    new string[] { AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.SignInPolicyId });
            }
            else if (context.ProtocolMessage.ErrorDescription != null && context.ProtocolMessage.ErrorDescription.Contains("AADB2C90118"))
            {
                // If the user clicked the reset password link, redirect to the reset password route
                context.Response.Redirect("/user/password/reset");
            }
            else if (context.Exception.Message?.Contains("access_denied") ?? false)
            {
                // If the user canceled, redirect back to the home page
                context.Response.Redirect("/");
            }
            else if (context.Exception is HttpException httpException)
            {
                Log.Error("B2C HttpException", httpException, this);
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ResetPassword, "B2C Error", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);
                context.Response.Redirect("/");
            }
            else
            {
                string errorMessage = (!string.IsNullOrEmpty(context.ProtocolMessage.Error)) ? context.ProtocolMessage.ErrorDescription : context.Exception.Message;

                Log.Error($"B2C Exception: {errorMessage}", context.Exception, this);
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ResetPassword, $"B2C Error - {errorMessage}", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);

                context.Response.Redirect("/");
            }

            return Task.CompletedTask;
        }

        private Task ResetPassword_OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            var identity = notification.AuthenticationTicket.Identity;

            var logData = new NameValueCollection();
            foreach (var claim in identity.Claims)
            {
                logData.Add(claim.Type, claim.Value);
            }

            this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.ResetPassword, "Resetting Password", logData);
            return Task.CompletedTask;
        }
    }
}