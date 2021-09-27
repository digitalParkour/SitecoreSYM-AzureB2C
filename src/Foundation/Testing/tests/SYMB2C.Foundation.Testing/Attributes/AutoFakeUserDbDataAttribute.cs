using AutoFixture;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using NSubstitute;
using Sitecore.Security.Accounts;
using System.Security.Principal;

namespace SYMB2C.Foundation.Testing.Attributes
{
    public class AutoFakeUserDbDataAttribute : AutoDbDataAttribute
    {
        public bool IsAuthenticated { get; set; }

        public string Domain { get; set; }

        public AutoFakeUserDbDataAttribute()
        {
            this.Fixture.Register(() =>
            {
                var fakeIdentity = Substitute.For<IIdentity>();
                var fakePrincipal = Substitute.For<IPrincipal>();
                var sitecoreContext = Substitute.For<ISitecoreContext>();
                fakeIdentity.IsAuthenticated.Returns(IsAuthenticated);

                if (!string.IsNullOrEmpty(this.Domain))
                {
                    fakeIdentity.Name.Returns($"{Domain}\\testuser");
                }
                else
                {
                    fakeIdentity.Name.Returns("symb2c\\testuser");
                }
                
                fakePrincipal.Identity.Returns(fakeIdentity);

                var fakeUser = User.FromPrincipal(fakePrincipal);
                sitecoreContext.User().Returns(fakeUser);

                return sitecoreContext;
            });
        }
        
    }
}
