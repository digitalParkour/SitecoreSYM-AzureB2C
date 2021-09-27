using System;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Billing;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.DependencyInjection;

namespace SYMB2C.Foundation.Consumer.Fake.Services
{
    [Service(typeof(IBillingService))]
    public class BillingServiceFake : IBillingService
    {
        public BillingInfo GetBillingInfo(PowerAccountID accountId)
        {
            return new BillingInfo { 
                AccountNumber = new PowerAccountID("1300020-0130589"),
                HouseNumber = "105",
                StreetAddress = "Cimmarion Dr",
                TotalBalance = 82.86M,
                DueDate = DateTime.Parse("06/05/2020"),
                LastPaymentAmount = 106.70M,
                NextReadDate = DateTime.Parse("09/16/2020"),
                CustomerSSN = "9999",
                MeterNumber = "999999",
                PhoneNumber = "5551234",
            };
        }

        public BillingInfo GetBillingInfoByPhone(string phone)
        {
            return new BillingInfo
            {
                AccountNumber = new PowerAccountID("1300020-0130589"),
                HouseNumber = "105",
                StreetAddress = "Cimmarion Dr",
                TotalBalance = 82.86M,
                DueDate = DateTime.Parse("06/05/2020"),
                LastPaymentAmount = 106.70M,
                NextReadDate = DateTime.Parse("09/16/2020"),
                CustomerSSN = "9999",
                MeterNumber = "999999",
                PhoneNumber = "555123",
            };
        }
    }
}
