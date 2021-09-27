using SYMB2C.Feature.Login.Models.XDb;

namespace SYMB2C.Feature.Login.Services
{
    public interface IXDBContactService
    {
        void IdentifyCurrent(IXDbContactWithEmail contact);

        void UpdateOrCreateServiceContact(IXDbContactWithEmail serviceContact);
    }
}