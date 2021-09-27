using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;

namespace SYMB2C.Feature.Login.Pipelines
{
    public partial class AzureAdB2CIdentityProviderProcessor : IdentityProvidersProcessor
    {
        public static readonly string SignInIframeNightPolicyId = Sitecore.Configuration.Settings.GetSetting("ida:SignInIframeNightPolicyId");

        private OpenIdConnectAuthenticationOptions CreateIframeNightSignInOptions()
        {
            var policy = SignInIframeNightPolicyId;

            return new OpenIdConnectAuthenticationOptions
            {
                MetadataAddress = string.Format(WellKnownMetadata, Tenant, policy),
                AuthenticationType = policy,
                AuthenticationMode = AuthenticationMode.Passive,

                // These are standard OpenID Connect parameters, with values pulled from settings
                ClientId = ClientId,
                Scope = $"openid profile offline_access {DefaultScope}",
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

    }
}