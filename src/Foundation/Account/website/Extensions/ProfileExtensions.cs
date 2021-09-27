using System;
using System.Collections.Generic;
using System.Linq;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Account.Extensions
{
    public static class ProfileExtensions
    {
        /// <summary>
        /// Returns legacy user id if available otherwise email username.
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>UserID object.</returns>
        public static UserID GetUserId(this ConsumerProfile profile)
        {
            return string.IsNullOrWhiteSpace(profile.LegacyUserId) ? profile.Email : profile.LegacyUserId;
        }

        /// <summary>
        /// Get an entry from AccountList based on SelectedAccountIndex value.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="selectedIndexBase1"></param>
        /// <returns>Returns null if list is empty, otherwise best match.</returns>
        public static PowerAccount GetSelectedAccount(this IList<PowerAccount> list, string selectedIndexBase1)
        {
            var count = list?.Count ?? 0;

            // CASE: none available
            if (count <= 0)
            {
                return null;
            }

            // CASE: one available
            else if (count == 1)
            {
                return list.First();
            }

            // Now we have multiple, pick one

            // CASE: has index
            if (int.TryParse(selectedIndexBase1, out int indexBase1))
            {
                // CASE: invalid low (including first)
                if (indexBase1 <= 1)
                {
                    return list.First();
                }

                // CASE: invalid high (including last)
                else if (indexBase1 >= count)
                {
                    return list.Last();
                }

                // CASE: valid index (transform to zero based index)
                return list[indexBase1 - 1];
            }

            // CASE: no index
            else
            {
                return list.First();
            }
        }

        /// <param name="id"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int GetIndexById(this IList<PowerAccount> list, PowerAccountID id)
        {
            // :: Validate
            if (id == null || !id.IsValid)
            {
                throw new ArgumentException($"GetIndexById: Invalid Account Number", nameof(id));
            }

            // CASE: empty list
            if (list == null || !list.Any())
            {
                return -1;
            }

            // find index by Account number
            for (var i = 0; i < list.Count; i++)
            {
                var account = list[i];
                if (account.Id == id)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}