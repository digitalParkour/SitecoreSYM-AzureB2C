namespace SYMB2C.Feature.Login.Constants
{
    /// <summary>
    /// Azure Identity Role Claims Constants.
    /// </summary>
    public static class AzureAdClaimsConstants
    {
        /// <summary>
        /// The e-mail address of the user.
        /// </summary>
        public static string EmailAddressClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        /// <summary>
        /// The given name of the user.
        /// </summary>
        public static string GivenNameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

        /// <summary>
        /// The unique name of the user.
        /// </summary>
        public static string NameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        /// <summary>
        /// The user principal name (UPN) of the user.
        /// </summary>
        public static string UpnClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";

        /// <summary>
        /// The common name of the user.
        /// </summary>
        public static string CommonNameClaim = "http://schemas.xmlsoap.org/claims/CommonName";

        /// <summary>
        /// A group that the user is a member of.
        /// </summary>
        public static string GroupClaim = "http://schemas.xmlsoap.org/claims/Group";

        /// <summary>
        /// A role that the user has.
        /// </summary>
        public static string RoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        /// <summary>
        /// A role that the user has.
        /// </summary>
        public static string SurnameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

        /// <summary>
        /// The private identifier of the user.
        /// </summary>
        public static string PPIDClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier";

        /// <summary>
        /// The SAML name identifier of the user.
        /// </summary>
        public static string NameIdentifierClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        /// <summary>
        /// The method used to authenticate the user.
        /// </summary>
        public static string AuthenticationMethodClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod";

        /// <summary>
        /// The deny-only group SID of the user.
        /// </summary>
        public static string DenyOnlyGroupSIDClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid";

        /// <summary>
        /// The deny-only primary SID of the user.
        /// </summary>
        public static string DenyOnlyPrimarySIDClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid";

        /// <summary>
        /// The deny-only primary group SID of the user.
        /// </summary>
        public static string DenyOnlyPrimaryGroupSIDClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid";

        /// <summary>
        /// The group SID of the user.
        /// </summary>
        public static string GroupSIDClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";

        /// <summary>
        /// The primary group SID of the user.
        /// </summary>
        public static string PrimaryGroupSIDClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid";

        /// <summary>
        /// The primary SID of the user.
        /// </summary>
        public static string PrimarySIDClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";

        /// <summary>
        /// The domain account name of the user in the form of <domain>\<user>.
        /// </summary>
        public static string WindowsAccountNameClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname";
    }
}