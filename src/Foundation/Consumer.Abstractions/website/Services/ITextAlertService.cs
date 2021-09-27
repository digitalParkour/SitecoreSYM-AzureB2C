using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;
using SYMB2C.Foundation.Consumer.Abstractions.Models.TextAlert;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    public interface ITextAlertService
    {
        bool OptOutNumber(UserID userId, PowerAccountID account, string contactnumber);

        bool OptInNumber(PowerAccountID account, string contactnumber);

        TextAlertInfo GetContactNumbers(UserID userId, PowerAccountID account, bool ignoreStateManager = false);

        bool SendPassCode(PowerAccountID account, string contactnumber, bool sendpasscode);

        bool WriteToAccountMasterDB(UserID userId, string contactnumber, PowerAccountID account, string status);

        // bool UpdateAccountMasterDB(UserID userId, string contactnumber, PowerAccountID account,string status);
        bool SetLocationsAndKeywords(UserID userId, string contactnumber, PowerAccountID account, bool optIn);

        string GetPasscodeFromSession();

        bool SyncBossmanWithMilsoft(PowerAccountID account);

        string FormatPhoneNumber(string phoneNumber);
    }
}
