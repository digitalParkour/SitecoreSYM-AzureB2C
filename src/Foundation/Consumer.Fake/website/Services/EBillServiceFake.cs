using SYMB2C.Foundation.Consumer.Abstractions.Models.EBill;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Consumer.Fake.Services
{
    [Service(typeof(IEBillService))]
    public class EBillService : IEBillService
    {
        public string GetViewBillUrl(UserID userId)
        {
            return null; // test error case
        }

        public bool IsEnrolled(UserID userId, PowerAccountID accountId)
        {
            return false;
        }

        public bool TryEnroll(EnrollParameters parms, out string message)
        {
            message = "Not Yet Implemented";
            return false;
        }

        public bool TryDeEnroll(EnrollParameters parms, out string message)
        {
            message = "Not Yet Implemented";
            return false;
        }
    }
}
