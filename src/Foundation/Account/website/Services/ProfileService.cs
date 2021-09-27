using System;
using System.Collections.Generic;
using System.Linq;
using SYMB2C.Foundation.Account.Extensions;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Account.Services
{
    [Service(typeof(IProfileService), Lifetime = Lifetime.Singleton)]
    public class ProfileService : IProfileService
    {
        private readonly IAccountService accountService;

        public ProfileService(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Get POCO object from User Profile.
        /// </summary>
        /// <returns></returns>
        public virtual ProfileData GetProfile()
        {
            return this.MapProfileData();
        }

        /// <summary>
        /// Sets session profile property SelectedAccountIndex.
        /// </summary>
        /// <param name="indexBase1">1 based index of AccountList.</param>
        /// <param name="andSave">Persist the data for next visit.</param>
        /// <returns></returns>
        public virtual ProfileData SetSelectedAccount(int indexBase1, bool andSave = true)
        {
            var profile = this.GetUserProfile();

            profile.SelectedAccountIndex = indexBase1.ToString();

            if (andSave)
            {
                profile.Save();
            }

            return this.MapProfileData(profile);
        }

        /// <summary>
        /// Add account to Profile property AccountList.
        /// </summary>
        /// <remarks>
        /// This also updates the selected index to match new entry.
        /// </remarks>
        /// <param name="account"></param>
        /// <param name="andSave"></param>
        /// <returns></returns>
        public virtual ProfileData AddAccount(PowerAccount account, bool andSave = true)
        {
            // :: Validate

            // Ensure valid account
            if (!account.Id.IsValid)
            {
                throw new ArgumentException("Adding Account to User Profile: Invalid Account Number");
            }

            var profile = this.GetUserProfile();

            // Ensure not already in list
            var list = this.accountService.GetAccounts(profile.GetUserId());
            if (list?.Any(x => x.Id == account.Id) ?? false)
            {
                var index = list.GetIndexById(account.Id);
                if (index > -1)
                {
                    index++; // property is 1-based index
                }

                profile.SelectedAccountIndex = index.ToString();

                // already added, nothing to do... except maybe honor save request
                if (andSave)
                {
                    profile.Save();
                }

                return this.MapProfileData(profile, list);
            }

            // :: Add account to profile
            list.Add(account);

            // update index to match new account
            profile.SelectedAccountIndex = list.Count.ToString();

            if (andSave)
            {
                profile.Save();
            }

            return this.MapProfileData(profile, list);
        }

        /// <summary>
        /// Remove Account from Profile property AccountList.
        /// </summary>
        /// <remarks>
        /// This does not update the selected index as it is okay for that index to be dirty.
        /// </remarks>
        /// <param name="accountId"></param>
        /// <param name="andSave"></param>
        /// <returns></returns>
        public virtual ProfileData RemoveAccount(PowerAccountID accountId, bool andSave = true)
        {
            // :: Validate

            // Ensure valid account
            if (accountId == null || !accountId.IsValid)
            {
                throw new ArgumentException("Removing Account from User Profile: Invalid Account Number", nameof(accountId));
            }

            var profile = this.GetUserProfile();

            // :: Find entry to delete
            var list = this.accountService.GetAccounts(profile.GetUserId());
            var index = list.GetIndexById(accountId);

            // CASE: nothing to delete
            if (index <= -1)
            {
                // nothing to do... except maybe honor save
                if (andSave)
                {
                    profile.Save();
                }

                return this.MapProfileData(profile, list);
            }

            // CASE: delete
            list.RemoveAt(index);

            // no need to update and save index, that will auto correct on read

            return this.MapProfileData(profile, list);
        }

        // == HELPERS ===========================================================================================
        private ConsumerProfile GetUserProfile()
        {
            return Sitecore.Context.User.Profile as ConsumerProfile;
        }

        private ProfileData MapProfileData(ConsumerProfile profile = null, IList<PowerAccount> accounts = null)
        {
            profile = profile ?? this.GetUserProfile();

            if (!int.TryParse(profile?.SelectedAccountIndex, out int selectedIndex))
            {
                selectedIndex = 0;
            }

            var accountList = accounts ?? this.accountService.GetAccounts(profile?.GetUserId());

            return new ProfileData
            {
                Email = profile?.Email,
                FirstName = profile?.FirstName,
                LastName = profile?.LastName,
                Phone = profile?.Phone,
                UserId = profile?.GetUserId(),
                BusinessName = profile?.BusinessName,
                PowerAccounts = accountList,
                SelectedAccountIndex = selectedIndex,
                SelectedAccount = accountList?.GetSelectedAccount(profile?.SelectedAccountIndex)
            };
        }
    }
}