using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.EBill
{
    public class EnrollParameters
    {
        public UserID UserId { get; set; }

        public PowerAccount PowerAccount { get; set; }

        public override string ToString()
        {
            return $"{UserId}-{PowerAccount}";
        }
    }
}
