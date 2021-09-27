using System.Globalization;
using System.Web;

namespace SYMB2C.Foundation.Common.Utilities
{
    public static class StringExtension
    {
        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static string ToUrlEncodedString(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        public static string ToCleanContactNumber(this string contactNumber)
        {
            var cleanedNumber = string.Empty;
            if (!string.IsNullOrEmpty(contactNumber))
            {
                cleanedNumber = contactNumber.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty);

                // Remove leading 1s if they exist
                cleanedNumber = cleanedNumber.TrimStart('1');
            }

            return cleanedNumber;
        }
    }
}