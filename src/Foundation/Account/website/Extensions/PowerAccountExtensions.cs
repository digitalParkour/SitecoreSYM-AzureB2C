using System;
using System.Collections.Generic;
using System.Linq;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;

namespace SYMB2C.Foundation.Account.Extensions
{
    public static class PowerAccountExtensions
    {
        private const string MajorDelim = "|";
        private const char MinorDelim = ':';

        /// <summary>
        /// Collapse into string: account-number1:nickname1|account-number2:nickname2.
        /// </summary>
        /// <param name="list">List of power accounts.</param>
        /// <returns>Serialized string of Accounts List.</returns>
        public static string SerializeAccountList(this IList<PowerAccount> list)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }

            return string.Join(MajorDelim, list.Select(x => $"{x.Id?.Value}{MinorDelim}{x.Nickname}").ToArray());
        }

        /// <summary>
        /// Parse format: account-number1:nickname1|account-number2:nickname2.
        /// </summary>
        /// <param name="rawList">Raw list of power accounts.</param>
        /// <returns>List of PowerAccount.</returns>
        public static IList<PowerAccount> DeserializeAccountList(this string rawList)
        {
            var result = new List<PowerAccount>();

            if (string.IsNullOrWhiteSpace(rawList))
            {
                return result;
            }

            var accounts = rawList.Split(new[] { MajorDelim }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var account in accounts)
            {
                // CASE: delimited pair as expected
                if (account.Contains(MinorDelim))
                {
                    var pair = account.Split(MinorDelim);
                    result.Add(new PowerAccount
                    {
                        Id = pair[0],
                        Nickname = pair[1]
                    });
                }

                // CASE: unexpected, but assume only account id
                else
                {
                    result.Add(new PowerAccount
                    {
                        Id = account
                    });
                }
            }

            return result;
        }
    }
}