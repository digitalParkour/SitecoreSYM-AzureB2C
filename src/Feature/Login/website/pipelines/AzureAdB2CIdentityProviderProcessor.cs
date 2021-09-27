using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Sitecore.Abstractions;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using SYMB2C.Foundation.Logging.Repositories;

namespace SYMB2C.Feature.Login.Pipelines
{
    public partial class AzureAdB2CIdentityProviderProcessor : IdentityProvidersProcessor
    {
        public const string IdpName = "AzureAdB2C";

        // OWIN auth middleware constants
        public const string ObjectIdElement = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        // App config settings
        public static readonly string ClientId = Sitecore.Configuration.Settings.GetSetting("ida:ClientId");
        public static readonly string ClientSecret = Sitecore.Configuration.Settings.GetSetting("ida:ClientSecret");
        public static readonly string AadInstance = Sitecore.Configuration.Settings.GetSetting("ida:AadInstance");
        public static readonly string Tenant = Sitecore.Configuration.Settings.GetSetting("ida:Tenant");
        public static readonly string RedirectUri = Sitecore.Configuration.Settings.GetSetting("ida:RedirectUri");
        public static readonly string PostLogoutRedirectUri = Sitecore.Configuration.Settings.GetSetting("ida:PostLogoutRedirectUri");
        public static readonly string LogOutUri = Sitecore.Configuration.Settings.GetSetting("ida:LogoutUrl");

        public static readonly bool RequireHttps = Sitecore.Configuration.Settings.GetBoolSetting("RequireHttps", true);

        // API Scopes
        public static readonly string ApiIdentifier = Sitecore.Configuration.Settings.GetSetting("api:ApiIdentifier");
        public static readonly string ReadTasksScope = ApiIdentifier + Sitecore.Configuration.Settings.GetSetting("api:ReadScope");
        public static readonly string WriteTasksScope = ApiIdentifier + Sitecore.Configuration.Settings.GetSetting("api:WriteScope");
        public static readonly string DefaultScope = string.Concat(ApiIdentifier, "/", Sitecore.Configuration.Settings.GetSetting("api:DefaultScope"));

        public static readonly string[] Scopes = new string[] { ReadTasksScope, WriteTasksScope };

        // Authorities
        public static readonly string WellKnownMetadata = $"{AadInstance}/v2.0/.well-known/openid-configuration";

        private readonly IEventLogger eventLogger;

        public AzureAdB2CIdentityProviderProcessor(
            FederatedAuthenticationConfiguration federatedAuthenticationConfiguration,
            ICookieManager cookieManager,
            IEventLogger eventLogger,
            BaseSettings settings)
            : base(federatedAuthenticationConfiguration, cookieManager, settings)
        {
            this.eventLogger = eventLogger;
        }

        public string DefaultPolicy => SignInPolicyId;

        public string Authority => string.Format(AadInstance, Tenant, this.DefaultPolicy);

        protected override string IdentityProviderName => IdpName;

        protected override void ProcessCore(IdentityProvidersArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            // Required for Azure webapps, as by default they force TLS 1.2 and this project attempts 1.0
            // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            args.App.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/user/signin"),
                CookieSecure = this.Settings.GetSetting("XA.Foundation.Multisite.Environment", "LOCAL").Equals("LOCAL", StringComparison.OrdinalIgnoreCase) ? CookieSecureOption.SameAsRequest : CookieSecureOption.Always,
                ExpireTimeSpan = TimeSpan.FromMinutes(15),
                CookieManager = this.CookieManager
            });

            args.App.UseOpenIdConnectAuthentication(this.CreateSignInOptions());
            args.App.UseOpenIdConnectAuthentication(this.CreateResetPasswordOptions());
            args.App.UseOpenIdConnectAuthentication(this.CreateChangePasswordOptions());
            args.App.UseOpenIdConnectAuthentication(this.CreateEditProfileOptions());

            // Experimental
            args.App.UseOpenIdConnectAuthentication(this.CreateIframeDaySignInOptions());
            args.App.UseOpenIdConnectAuthentication(this.CreateIframeNightSignInOptions());
        }

        public struct CustomClaim
        {
            public const string Email = "email";
            public const string FirstName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
            public const string LastName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
            public const string Phone = "extension_Phone";
            public const string LegacyId = "extension_LegacyUserId";
            public const string BusinessName = "extension_BusinessName";
            public const string AccountList = "extension_PowerAccountList";
            public const string IsNew = "newUser";
        }
    }
}