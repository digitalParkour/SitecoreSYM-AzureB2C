namespace SYMB2C.Feature.Login.Models.XDb
{
    public interface IXDbContact
    {
        string IdentifierSource { get; }
        string IdentifierValue { get; }
    }

    public interface IXdbPhoneNumberFacets
    {
        string PhoneNumber { get; set; }
    }

    public interface IXdbAddressListFacets
    {
        string CountryCode { get; set; }
        string City { get; set; }
        string StateOrProvince { get; set; }
    }

    public interface IXdbContactFacets : IXdbPhoneNumberFacets, IXdbAddressListFacets
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string JobTitle { get; set; }
        string NickName { get; set; }
    }

    public interface IXDbContactWithEmail : IXDbContact, IXdbContactFacets
    {
        string Email { get; }
    }
}