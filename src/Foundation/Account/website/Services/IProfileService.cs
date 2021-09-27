using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Account.Services
{
    public interface IProfileService
    {
        ProfileData GetProfile();

        ProfileData AddAccount(PowerAccount account, bool andSave = true);

        ProfileData RemoveAccount(PowerAccountID accountId, bool andSave = true);

        ProfileData SetSelectedAccount(int indexBase1, bool andSave = true);
    }
}