using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Account
{
    public class PowerAccountDetail : PowerAccount
    {
        public UserID UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int AccountsEnrolled { get; set; }

        public int EbillEnrolled { get; set; }
    }
}
