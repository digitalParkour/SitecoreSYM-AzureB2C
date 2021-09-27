using SYMB2C.Foundation.Consumer.Abstractions.Models.EBill;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    public interface IEBillService
    {
        bool IsEnrolled(UserID userId, PowerAccountID accountId);

        bool TryEnroll(EnrollParameters props, out string message);

        bool TryDeEnroll(EnrollParameters props, out string message);

        string GetViewBillUrl(UserID userId);
    }
}
