using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using SYMB2C.Foundation.Common.Constants;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using Sitecore.Owin.Authentication.Services;

namespace SYMB2C.Feature.Login.Pipelines
{
    public partial class AzureAdB2CIdentityProviderProcessor : IdentityProvidersProcessor
    {
        public static string EditProfilePolicyId = Sitecore.Configuration.Settings.GetSetting("ida:EditProfilePolicyId");

        private OpenIdConnectAuthenticationOptions CreateEditProfileOptions()
        {
            var policy = EditProfilePolicyId;

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
                    RedirectToIdentityProvider = this.EditProfile_OnRedirectToIdentityProvider,
                    AuthenticationFailed = this.EditProfile_OnAuthenticationFailed,
                    SecurityTokenValidated = this.EditProfile_OnSecurityTokenValidated,
                },
            };
        }

        private Task EditProfile_OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.EditProfile, "Attempting Profile Edit");

            return Task.CompletedTask;
        }

        private Task EditProfile_OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
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
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.EditProfile, "B2C Error", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);
                context.Response.Redirect("/");
            }
            else
            {
                string errorMessage = (!string.IsNullOrEmpty(context.ProtocolMessage.Error)) ? context.ProtocolMessage.ErrorDescription : context.Exception.Message;

                Log.Error($"B2C Exception: {errorMessage}", context.Exception, this);
                this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.EditProfile, $"B2C Error - {errorMessage}", null, Foundation.Logging.Enum.Level.ERROR, context.Exception);

                context.Response.Redirect("/");
            }

            return Task.CompletedTask;
        }

        private Task EditProfile_OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            var identityProvider = this.GetIdentityProvider();
            var identity = notification.AuthenticationTicket.Identity;

            // Map claims
            foreach (var claimTransformationService in identityProvider.Transformations)
            {
                claimTransformationService.Transform(identity, new TransformationContext(this.FederatedAuthenticationConfiguration, identityProvider));
            }

            var logData = new NameValueCollection();
            foreach (var claim in identity.Claims)
            {
                logData.Add(claim.Type, claim.Value);
            }

            // ::::: WORKAROUND :::::
            // Send profile data to SYMB2C for CSR search and existing Stored Procs
            var email = identity.FindFirstValue(CustomClaim.Email);
            var userId = identity.FindFirstValue(CustomClaim.LegacyId);
            if (string.IsNullOrWhiteSpace(userId))
            {
                // Fall back to email as username
                userId = email;
            }

            var verifyProps = new VerifyProfileParameters
            {
                Email = email,
                UserId = userId,
                FirstName = identity.FindFirstValue(CustomClaim.FirstName),
                LastName = identity.FindFirstValue(CustomClaim.LastName),
                Phone = identity.FindFirstValue(CustomClaim.Phone),
                BusinessName = identity.FindFirstValue(CustomClaim.BusinessName),
            };
            var accountService = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IAccountService>();
            accountService.VerifyProfile(verifyProps, true);
            // ::: End Workaround :::

            this.eventLogger.LogEvent(Foundation.Logging.Enum.Event.EditProfile, "Editing Profile", logData);
            return Task.CompletedTask;
        }
    }
}