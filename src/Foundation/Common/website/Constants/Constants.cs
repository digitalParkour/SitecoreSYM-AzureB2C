
namespace SYMB2C.Foundation.Common.Constants
{
    public class SYMB2CConstants
    {
        public const string NesCustomLogFileAppender = "NesCustomLogFileAppender";
    }

    public class PageConstants
    {
        public const string PageTitle = "Title";
    }

    public class AuthenticationFilter
    {
        public const string BasicScheme = "Basic";

        public class ReasonPhrase
        {
            public const string Missing = "Missing credentials";

            public const string Invalid = "Missing credentials";

            public const string InvalidBoth = "Invalid email or password";
        }
    }
}