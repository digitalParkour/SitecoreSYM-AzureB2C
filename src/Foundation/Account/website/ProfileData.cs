using System;
using System.Collections.Generic;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Account
{
    [Serializable]
    public class ProfileData
    {
        public UserID UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string BusinessName { get; set; }

        public IList<PowerAccount> PowerAccounts { get; set; }

        public int SelectedAccountIndex { get; set; }

        public PowerAccount SelectedAccount { get; set; }
    }
}