namespace SYMB2C.Foundation.Common.Constants
{
    public static class PageRedirect
    {
         public const string EXCEPTION_CRITERIA = "/Login/error?criteria=";

        #region Login
         public const string LOGIN_UPDATE = "/Login/Update";
         public const string LOGIN_LOGIN = "/user/signin";
         public const string LOGIN_MYACCOUNT = "/";
         public const string LOGIN_REGISTER = "/user/signup";
        #endregion

        #region Cust Dir
         public const string CUSTDIR_MYACCT_ENROLL = "/Custdir/myaccount_enroll";
         public const string CUSTDIR_MYACCT_TEXTING = "/Custdir/myaccount_texting";
         public const string CUSTDIR_MYACCT_RPTOUTAGE = "/custdir/myaccount_reportoutage";
         public const string CUSTDIR_EBILL = "/Custdir/ebill";
         public const string CUSTDIR_MANAGEACCOUNTS = "/Custdir/manageaccounts";
         public const string CUSTDIR_CSRADMIN = "/Custdir/csradmin";
         public const string CUSTDIR_SOLAR = "/Custdir/solar";
        #endregion

        #region Vendor
         public const string VENDOR_NEWVENDORXML = "/Vendor/newVendorXML";
         public const string VENDOR_MYVENDORACCOUNT = "/Vendor/myVendorAccount";
        #endregion

        #region BidCenter
         public const string BIDCENTER_BIDDETAILS = "/BidCenter/BidDetails";
         public const string BIDCENTER_BIDQUESTIONS = "/BidCenter/BidQuestions";
         public const string BIDCENTER_BIDQUESTION = "/BidCenter/BidQuestion";
        #endregion

        #region MyAccount
        public const string MY_ACCOUNT = "/my-account";
        #endregion
    }
}