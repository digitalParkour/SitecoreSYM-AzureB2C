using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Account
{
    public class AddAccountParameters : EditAccountParameters
    {
        public string AccountNickname { get; set; }
    }
}
