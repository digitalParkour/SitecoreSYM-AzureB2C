using System.Collections.Generic;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    public interface IAccountService
    {
        List<PowerAccount> GetAccounts(UserID userId);

        List<PowerAccountDetail> FindAccounts(FindAccountParameters props, out FindAccountStatus status);

        void VerifyProfile(VerifyProfileParameters props, bool forceUpdate);

        bool TryAddAccount(AddAccountParameters props, out string message);

        bool TryRemoveAccount(EditAccountParameters props, out string message);

        bool CheckAccountPassword(UserID userId, string password);

        bool DeleteProfile(UserID userId);

        string GetBusinessName(UserID userId);

        string GetEmailByUserId(UserID userId);
    }
}
