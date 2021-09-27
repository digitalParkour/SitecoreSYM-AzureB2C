using Sitecore.Security.Accounts;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    public interface ISitecoreContext
    {
        User User();
        void SignOut();
    }
}
