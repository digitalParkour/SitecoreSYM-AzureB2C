using System;
using System.Collections.Generic;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Account;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Consumer.Fake.Services
{
    [Service(typeof(IAccountService))]
    public class AccountServiceFake : IAccountService
    {

        public List<PowerAccount> GetAccounts(UserID userId)
        {
            return new List<PowerAccount>
            {
                new PowerAccount
                {
                    Nickname = "Test 1",
                    Id = "1300020-0000001"
                },

                new PowerAccount
                {
                    Nickname = "Test 2",
                    Id = "1300025-0000002"
                },

                new PowerAccount
                {
                    Nickname = "Test 3",
                    Id = "1300027-0000003"
                }
            };
        }

        public bool TryAddAccount(AddAccountParameters props, out string message)
        {
            message = "Not Implemented";
            return DateTime.Now.Minute % 2 == 0; // 50/50, predictable by minute even/odd
        }

        public bool TryRemoveAccount(EditAccountParameters props, out string message)
        {
            message = "Record not delete, just testing.";
            return DateTime.Now.Minute % 2 == 0; // 50/50, predictable by minute even/odd
        }

        public List<PowerAccountDetail> FindAccounts(FindAccountParameters props, out FindAccountStatus status)
        {
            status = FindAccountStatus.FoundRecords;
            return new List<PowerAccountDetail>
            {
                new PowerAccountDetail
                {
                    UserId = "james.gregory@americaneagle.com",
                    FirstName = "James",
                    LastName = "Gregory",
                    Nickname = "First Test",
                }
            };
        }

        public void VerifyProfile(VerifyProfileParameters props, bool forceUpdate)
        {
            // sync'd :)
        }

        public bool CheckAccountPassword(UserID userId, string password)
        {
            return true;
        }

        public bool DeleteProfile(UserID userId)
        {
            return true;
        }

        public string GetBusinessName(UserID userId)
        {
            return "Americaneagle.com";
        }

        public string GetEmailByUserId(UserID userId)
        {
            return "james.gregory@americaneagle.com";
        }
    }
}
