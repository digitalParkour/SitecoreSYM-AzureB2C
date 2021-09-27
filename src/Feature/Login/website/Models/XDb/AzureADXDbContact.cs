namespace SYMB2C.Feature.Login.Models.XDb
{
    public class AzureADXDbContact : IXDbContactWithEmail
    {
        public AzureADXDbContact(string source)
        {
            Source = source;
        }

        private string Source;

        public string IdentifierSource => this.Source;

        public string IdentifierValue => this.Email;

        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; }

        public string City { get; set; }

        public string StateOrProvince { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

    }
}