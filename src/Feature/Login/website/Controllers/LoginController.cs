using System.Linq;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using SYMB2C.Feature.Login.Pipelines;
using SYMB2C.Foundation.Account.Services;
using SYMB2C.Foundation.Caching;
using SYMB2C.Foundation.Common.Constants;
using SYMB2C.Foundation.Consumer.Abstractions.Services;

namespace SYMB2C.Feature.Login.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISitecoreContext sitecoreContext;
        private readonly IOwinService owinService;
        private readonly IStateManager stateManager;

        public LoginController(ISitecoreContext sitecoreContext, IOwinService owinService)
            : base()
        {
            this.sitecoreContext = sitecoreContext;
            this.owinService = owinService;
            this.stateManager = ServiceLocator.ServiceProvider.GetServices<IStateManager>().First(x => x.GetType() == typeof(SessionManager));
        }

        public LoginController(ISitecoreContext sitecoreContext, IOwinService owinService, IStateManager stateManager)
            : base()
        {
            this.sitecoreContext = sitecoreContext;
            this.owinService = owinService;
            this.stateManager = stateManager;
        }

        [Route("user/signin")]
        public ActionResult SignIn()
        {
            if (!this.sitecoreContext.User().IsAuthenticated)
            {
                // Launch User Flow
                this.owinService.Challenge(string.Empty, AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.SignInPolicyId);
                return new EmptyResult();
            }

            return this.Redirect(PageRedirect.MY_ACCOUNT);
        }

        [Route("user/signout")]
        public ActionResult SignOut()
        {
            // clear session data
            this.stateManager.Clear();

            // To sign out the user, you should issue an OpenIDConnect sign out request.
            if (this.sitecoreContext.User().IsAuthenticated)
            {
                this.owinService.SignOut(AzureAdB2CIdentityProviderProcessor.IdpName);
                this.sitecoreContext.SignOut();

                return new EmptyResult();
            }

            return this.Redirect("/");
        }

        [Route("user/profile/edit")]
        [Authorize]
        public ActionResult UpdateProfile()
        {
            // Launch User Flow
            this.owinService.Challenge(string.Empty, AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.EditProfilePolicyId);

            return new EmptyResult();
        }

        [Route("user/password/reset")]
        public void PasswordReset()
        {
            // Launch User Flow
            this.owinService.Challenge("/user/signin", AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.PasswordResetPolicyId);
        }

        [Route("user/password/change")]
        [Authorize]
        public ActionResult ChangePassword()
        {
            // Launch User Flow
            this.owinService.Challenge(PageRedirect.MY_ACCOUNT, AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.PasswordChangePolicyId);

            return new EmptyResult();
        }

        // ================================================ Experimental ============================================

        [Route("user/dayframein")]
        public void SignInIframeDay()
        {
            if (!this.sitecoreContext.User().IsAuthenticated)
            {
                // Launch User Flow
                var callback = "/identity/externallogincallback?ReturnUrl=%2fb2c%2fsignin-iframe-day-callback.html&sc_site=SYMB2C&authenticationSource=Default";
                this.owinService.Challenge(callback, AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.SignInIframeDayPolicyId);
            }
        }

        [Route("user/nightframein")]
        public void SignInIframeNight()
        {
            if (!this.sitecoreContext.User().IsAuthenticated)
            {
                // Launch User Flow
                var callback = "/identity/externallogincallback?ReturnUrl=%2fb2c%2fsignin-iframe-night-callback.html&sc_site=SYMB2C&authenticationSource=Default";
                this.owinService.Challenge(callback, AzureAdB2CIdentityProviderProcessor.IdpName, AzureAdB2CIdentityProviderProcessor.SignInIframeNightPolicyId);
            }
        }
    }
}