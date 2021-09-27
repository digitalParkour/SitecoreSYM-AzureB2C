namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Account
{
    /// <summary>
    /// Properties can contain partial values for wildcard search.
    /// </summary>
    public class FindAccountParameters
    {
        public string UserId { get; set; }

        public string AccountId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
