using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Account
{
    public class EditAccountParameters
    {
        public UserID UserId { get; set; }

        public PowerAccountID AccountId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
    }
}
