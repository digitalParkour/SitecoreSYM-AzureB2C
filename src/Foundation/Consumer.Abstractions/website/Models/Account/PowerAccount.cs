using System;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Account
{
    [Serializable]
    public class PowerAccount
    {
        public string Nickname { get; set; }

        public PowerAccountID Id { get; set; }

        public override string ToString()
        {
            return $"{this.Nickname}|{this.Id?.Value}";
        }
    }
}
