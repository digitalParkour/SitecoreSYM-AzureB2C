# Sitecore and Azure AD B2C Examples

## Purpose

This Solution is a reference for common Azure AD B2C integrations.

### Disclaimer

This Solution will not do anything on its own for several reasons:
* Configs need to be updated to a working Azure B2C Tenant
  * See Feature.Login.config
* Policies need to be adjusted for that tenant, and uploaded.
  * See
    * "yourDEVtenant", "yourQAtenant", yourProdtenant"
    * YOUR-APP-ID-HERE, YOUR-CLIENT-ID-HERE, YOUR-ID-TOKEN-HERE
    * Policy IDs
* HTML templates have absolute urls to alter, and need to be hosted publically.
  * See hostnames in html files, *symb2csite.com

For that reason docker files and deployment handling are not included as this is not intended to run as is, but instead be reviewed and pulled into your solution where helpful.

## Feature.Login

This module has all the goods.
### Sitecore Azure B2C Identity Provider implementation
* https://doc.sitecore.com/en/developers/93/sitecore-experience-manager/configure-federated-authentication.html
  * **Config patches:** \App_Config\Include\Feature\Feature.Login.config
  * **OWIN Provider class:** \pipelines\AzureAdB2CIdentityProviderProcessor.cs
  * **UserBuilder:** \Processor\SYMB2CExternalUserBuilder.cs
  * **Launch Flows**: \Controllers\LoginController.cs

### B2C Custom Policies

While the policy xml files can be deployed manually via the Azure Portal it is recommended to keep them in source control, and if able automate the uploads with CI/CD. This minimizes room for manual error.

Custom Policy files are organized by environment under **/Policies/[env]**. They are prefixed with a number to keep them in the order compatible for upload (dependencies first).

There is also a powershell script, **DeployPolicies.ps1**, as a sample for scripting the upload process.

### B2C HTML Templates

The custom policies reference the custom html templates and assets, which live under **/b2c/[env]** subfolders.

### Featured Examples
* Seamless Password Migration flow
  * https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-migration
  * See **...SeamlessMigrationExtensions.xml**
  * And Rest API calls
    * \Controllers\\**IdentityController.cs**
* Branded Emails
  * Custom TOTP, Temporary One-Time Passcode verification
    * https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-email-sendgrid?pivots=b2c-custom-policy
    * See JS in **resetpassword.html** and **signup.html**
    * And Rest API calls
      * \Controllers\\**VerificationController.cs**
* Extension to map multiple claims into one, such as first and last name into full name.
  * See **ConcatClaimsTransformation**.