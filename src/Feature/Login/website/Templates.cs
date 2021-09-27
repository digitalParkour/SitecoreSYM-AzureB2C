using Sitecore.Data;

namespace SYMB2C.Feature.Login
{
    public struct Templates
    {
        public struct LoginBox
        {
            public struct WelcomeText
            {
                public static readonly string FieldName = "WelcomeText";
                public static readonly ID ID = new ID("{407D504C-1E0D-46EA-A720-A550DE36E1F9}");
            }

            public struct SigninDirections
            {
                public static readonly string FieldName = "SigninDirections";
                public static readonly ID ID = new ID("{23862B3B-0740-4E8A-B10A-1E0E0169161B}");
            }

            public struct ButtonText
            {
                public static readonly string FieldName = "ButtonText";
                public static readonly ID ID = new ID("{DA5D64C9-DF58-4C56-A554-B560EA7C0AC0}");
            }

            public struct ForgotPasswordText
            {
                public static readonly string FieldName = "ForgotPasswordText";
                public static readonly ID ID = new ID("{6EB356E1-1E13-4456-B193-33B99EDABDC9}");
            }

            public struct ForgotPasswordLinkText
            {
                public static readonly string FieldName = "ForgotPasswordLinkText";
                public static readonly ID ID = new ID("{0E7C2236-7E86-4620-9487-06CA7420E722}");
            }

            public struct BidCenterText
            {
                public static readonly string FieldName = "BidCenterText";
                public static readonly ID ID = new ID("{7FCE11D8-6092-4E67-845F-995B2C60E2F6}");
            }

            public struct VendorLoginLink
            {
                public static readonly string FieldName = "VendorLoginLink";
                public static readonly ID ID = new ID("{F3E2D6BA-25EC-457B-9768-64B20868CB68}");
            }
        }

        public struct MailTemplates
        {
            public struct B2CVerify
            {
                public static readonly ID ID = new ID("{933B339F-A030-4146-9BB8-7F580C1F8089}");
            }
        }
    }
}