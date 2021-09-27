using System;
using System.Globalization;
using Sitecore.Diagnostics;

namespace SYMB2C.Foundation.Common.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime? ParseDate(string date, params string[] formats)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return null;
            }

            if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }

            Log.Warn($"Unable to parse BillingInfo date: {date}. Did not match expected formats: {string.Join(", ", formats)}", nameof(ParseDate));

            return null;
        }
    }
}