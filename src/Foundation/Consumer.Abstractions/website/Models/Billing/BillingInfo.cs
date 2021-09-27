using System;
using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Billing
{
    [Serializable]
    public class BillingInfo
    {
        public bool HasReadError { get; set; }

        public PowerAccountID AccountNumber { get; set; }

        public string HouseNumber { get; set; }

        public string StreetAddress { get; set; }

        public string ApartmentNumber { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal LastPaymentAmount { get; set; }

        public DateTime? NextReadDate { get; set; }

        public string PhoneNumber { get; set; }

        public string CustomerSSN { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsPastDue { get; set; }

        public string MeterNumber { get; set; }

        public override string ToString()
        {
            return $"{this.AccountNumber}-{this.HouseNumber}-{this.ApartmentNumber}-{this.StreetAddress}-{this.TotalBalance}-{this.LastPaymentAmount}-{this.PhoneNumber}-{this.CustomerSSN}-{this.MeterNumber}";
        }
    }
}
