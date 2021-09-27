using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Identity;
using Sitecore.Owin.Authentication.Services;
using Sitecore.SecurityModel.Cryptography;
using SYMB2C.Feature.Login.Constants;
using SYMB2C.Feature.Login.Models.XDb;
using SYMB2C.Feature.Login.Services;

namespace SYMB2C.Feature.Login.Processor
{
    public class SYMB2CExternalUserBuilder : DefaultExternalUserBuilder
    {
        public const string B2C_Login_Source = "B2CLogin";

        private readonly IXDBContactService xdbContactService;

        public SYMB2CExternalUserBuilder(ApplicationUserFactory applicationUserFactory, IHashEncryption hashEncryption, IXDBContactService xdbContactService)
            : base(applicationUserFactory, hashEncryption)
        {
            this.IsPersistentUser = false;
            this.xdbContactService = xdbContactService;
        }

        /// <summary>
        /// External User Build. Since we are using Virtual users, this is called every time. Also identiy the user to xConnect (this may not be the best place).
        /// </summary>
        /// <param name="userManager">UserManager of Application User object.</param>
        /// <param name="externalLoginInfo">ExternalLoginInfo object.</param>
        /// <returns>ApplicationUser object.</returns>
        public override ApplicationUser BuildUser(UserManager<ApplicationUser> userManager, ExternalLoginInfo externalLoginInfo)
        {
            Assert.ArgumentNotNull(userManager, "userManager");
            Assert.ArgumentNotNull(externalLoginInfo, "externalLoginInfo");

            ClaimsIdentity externalIdentity = externalLoginInfo.ExternalIdentity;
            IdentityProvider identityProvider = this.FederatedAuthenticationConfiguration.GetIdentityProvider(externalIdentity);
            if (identityProvider == null)
            {
                throw new InvalidOperationException("Unable to retrieve an identity provider for the given identity");
            }

            var username = this.GetUserNameForAzureADB2C(userManager, externalIdentity);
            var applicationUser = this.ApplicationUserFactory.CreateUser(username);
            applicationUser.IsVirtual = true;

            var azureAdxDbContact = this.CreateAzureAdxDbContact(B2C_Login_Source, userManager, externalLoginInfo);

            try
            {
                this.xdbContactService.IdentifyCurrent(azureAdxDbContact);
                this.xdbContactService.UpdateOrCreateServiceContact(azureAdxDbContact);
            }
            catch (Exception ex)
            {
                // exit quietly, do not impede login
                Log.Error("Failed to identify contact", ex, this);
            }

            return applicationUser;
        }

        protected string GetUserNameForAzureADB2C(UserManager<ApplicationUser> userManager, ClaimsIdentity externalIdentity)
        {
            var emailClaim = "email";

            IdentityProvider identityProvider = this.FederatedAuthenticationConfiguration.GetIdentityProvider(externalIdentity);

            string domain = identityProvider.Domain;

            var username = externalIdentity.Claims.FirstOrDefault(x => x.Type == emailClaim)?.Value?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new NotSupportedException("Username not provided");
            }

            // Verify unique name
            if (UserManagerExtensions.FindByName(userManager, username) != null)
            {
                throw new NotSupportedException("Username already exists");
            }

            return $"{domain}\\{username}";
        }

        protected AzureADXDbContact CreateAzureAdxDbContact(string source, UserManager<ApplicationUser> userManager, ExternalLoginInfo externalLoginInfo)
        {
            var azureADXDbContact = new AzureADXDbContact(source);

            ClaimsIdentity externalIdentity = externalLoginInfo.ExternalIdentity;
            IdentityProvider identityProvider = this.FederatedAuthenticationConfiguration.GetIdentityProvider(externalIdentity);
            if (identityProvider == null)
            {
                throw new InvalidOperationException("Unable to retrieve an identity provider for the given identity");
            }

            string domain = identityProvider.Domain;

            // Email
            var email = externalIdentity.FindFirstValue(AzureAdClaimsConstants.EmailAddressClaim)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
            {
                email = externalIdentity.FindFirstValue("email")?.ToLowerInvariant();
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                azureADXDbContact.Email = email;
                azureADXDbContact.NickName = string.Concat(domain, "\\", email);
            }

            // First
            var firstName = externalIdentity.FindFirstValue(AzureAdClaimsConstants.GivenNameClaim);
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                azureADXDbContact.FirstName = firstName;
            }

            // Last
            var lastName = externalIdentity.FindFirstValue(AzureAdClaimsConstants.SurnameClaim);
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                azureADXDbContact.LastName = lastName;
            }

            return azureADXDbContact;
        }
    }
}