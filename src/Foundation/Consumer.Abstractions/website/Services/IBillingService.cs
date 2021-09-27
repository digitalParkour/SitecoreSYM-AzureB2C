using SYMB2C.Foundation.Consumer.Abstractions.Models.Billing;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    public interface IBillingService
    {
        BillingInfo GetBillingInfo(PowerAccountID accountId);

        BillingInfo GetBillingInfoByPhone(string phone);
    }
}
