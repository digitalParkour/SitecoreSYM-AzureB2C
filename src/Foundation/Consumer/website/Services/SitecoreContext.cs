using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.DependencyInjection;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace SYMB2C.Foundation.Consumer.Services
{
    [Service(typeof(ISitecoreContext))]
    public class SitecoreContext : ISitecoreContext
    {
        public User User()
        {
            return Sitecore.Context.User;
        }

        public void SignOut()
        {
            AuthenticationManager.Logout();
        }
    }
}