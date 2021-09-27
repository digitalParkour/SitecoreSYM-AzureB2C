using System;
using System.Web;
using Microsoft.Owin.Security;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Consumer.Services
{
    /// <summary>
    /// Service for Microsoft Owin Context using AzureAdB2C Identity Server for authentication.
    /// </summary>
    [Service(typeof(IOwinService))]
    public class OwinService : IOwinService
    {
        private const string DefaultRedirectUri = "/identity/externallogincallback?ReturnUrl=%2fmy-account&sc_site=SYMB2C&authenticationSource=Default";

        public void Challenge(string redirectUri, string authenticationName, string authenticationPolicy)
        {
            HttpContext.Current.GetOwinContext().Authentication.Challenge(
                 new AuthenticationProperties() { RedirectUri = this.GetDefaultRedirectUri(redirectUri) },
                 new string[] { authenticationName, authenticationPolicy });
        }

        public void SignOut(string authenticationName)
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(
                    new AuthenticationProperties { },
                    new string[] { authenticationName, authenticationName });
        }

        private string GetDefaultRedirectUri(string redirectUri)
        {
            if (string.IsNullOrEmpty(redirectUri))
            {
                return DefaultRedirectUri;
            }

            return redirectUri;
        }
    }
}