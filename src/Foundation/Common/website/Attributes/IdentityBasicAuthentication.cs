using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using SYMB2C.Foundation.Common.Constants;
using SYMB2C.Foundation.Common.Results;
using SYMB2C.Foundation.Settings.Provider;
using SYMB2C.Foundation.Settings.Services;

namespace SYMB2C.Foundation.Common.Attributes
{
    public class IdentityBasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private readonly IBasicAuthenticationSettings basicAuthenticationSettings;

        public bool AllowMultiple { get; }

        public string Realm { get; set; }

        public IdentityBasicAuthenticationAttribute()
        {
            this.basicAuthenticationSettings = new BasicAuthenticationSettings(new SitecoreSettingsProvider());
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != AuthenticationFilter.BasicScheme)
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult(AuthenticationFilter.ReasonPhrase.Missing, request);
                return;
            }

            Tuple<string, string> userNameAndPassword = ExtractUserNameAndPassword(authorization.Parameter);

            if (userNameAndPassword == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(AuthenticationFilter.ReasonPhrase.Invalid, request);
            }

            var userName = userNameAndPassword.Item1;
            var password = userNameAndPassword.Item2;

            IPrincipal principal = await this.AuthenticateAsync(userName, password, cancellationToken);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(AuthenticationFilter.ReasonPhrase.InvalidBoth, request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        private async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userName == this.basicAuthenticationSettings.GetUsername() && password == this.basicAuthenticationSettings.GetPassword())
            {
                return new ClaimsPrincipal(new GenericIdentity("AzureB2CIdentity"));
            }

            return null;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            this.Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (string.IsNullOrEmpty(this.Realm))
            {
                parameter = null;
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                parameter = "realm=\"" + this.Realm + "\"";
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.ChallengeWith(AuthenticationFilter.BasicScheme, parameter);
        }

        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            var encoding = Encoding.ASCII;
            encoding = (Encoding)encoding.Clone();
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (string.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            var userName = decodedCredentials.Substring(0, colonIndex);
            var password = decodedCredentials.Substring(colonIndex + 1);

            return new Tuple<string, string>(userName, password);
        }
    }
}