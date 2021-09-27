using System;

namespace SYMB2C.Foundation.Common.Models
{
    [Serializable]
    public class UserObject
    {
        public string UserID { get; set; }
        public string CompanyName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime AccountCreated { get; set; }
        public char[] UserType { get; set; }
        public char UserActive { get; set; }
        public int SecretQuestion { get; set; }
        public string VendorNumbers { get; set; }
        public string KubraToken1 { get; set; }
        public string KubraToken2 { get; set; }
        public string KubraAccountList { get; set; }
        public string KubraEmailAddressSED { get; set; }
        public string KubraLastName { get; set; }
        public string KubraFirstName { get; set; }
        public string KubraPassword { get; set; }
        public string PreferredContactMethod { get; set; }
        public DateTime AccountUpdated { get; set; }
        public string KubraTokenFiserv { get; set; }
        //public String AccountType { get; set; }
    }

    /// <summary>
    /// Configuration Object for use w/ Configuration Record
    /// </summary>
    public class ConfigObject
    {
        public string ConfigKey { get; set; }
        public string Description { get; set; }
        public int? IntValue { get; set; }
        public string StrValue { get; set; }
    }
}