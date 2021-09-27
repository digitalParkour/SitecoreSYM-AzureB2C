using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Feature.Login.Controllers;
using SYMB2C.Feature.Login.Pipelines;
using SYMB2C.Foundation.Account.Services;
using SYMB2C.Foundation.Caching;
using SYMB2C.Foundation.Common.Constants;
using SYMB2C.Foundation.Consumer.Abstractions.Services;
using SYMB2C.Foundation.Testing.Attributes;
using NSubstitute;
using Sitecore.Common;
using Sitecore.DependencyInjection;
using Sitecore.Security.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace SYMB2C.Feature.Login.Tests.Controllers
{
    public class LoginControllerTests
    {
        [Theory]
        [AutoFakeUserDbData(IsAuthenticated = true)]
        public void Signin_Should_Return_Redirect_To_My_Account([Greedy] LoginController sut)
        {

            var fakeDomain = new Domain("symb2c");
            using (new Switcher<Domain, Domain>(fakeDomain))
            {
                sut.SignIn().Should().BeOfType<RedirectResult>().Which.Url.Should().Be(PageRedirect.MY_ACCOUNT);
            }
        }

        [Theory]
        [AutoFakeUserDbData(IsAuthenticated = false)]
        public void Signin_Should_Return_Empty_Result_On_Signin([Frozen] IOwinService owinService,[Greedy] LoginController sut)
        {
            var fakeDomain = new Domain("symb2c");
            using (new Switcher<Domain, Domain>(fakeDomain))
            {
                owinService.Challenge(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
                sut.SignIn().Should().BeOfType<EmptyResult>();
            }
        }

        [Theory]
        [AutoFakeUserDbData(IsAuthenticated = false)]
        public void Signin_IFrame_Should_Call_Challenge_Owin_Context_User_Not_Authenticated([Frozen] IOwinService owinService, [Greedy] LoginController sut)
        {
            var fakeDomain = new Domain("symb2c");
            using (new Switcher<Domain, Domain>(fakeDomain))
            {
                sut.SignInIframeDay();
                owinService.Received(1).Challenge(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
            }
        }

        [Theory]
        [AutoFakeUserDbData(IsAuthenticated = true)]
        public void Authenticated_User_Should_Successfully_SignOut([Frozen] ISitecoreContext sitecoreContext, [Frozen] IOwinService owinService, [Frozen] IStateManager stateManager, [Greedy] LoginController sut)
        {
            var fakeDomain = new Domain("symb2c");

            using (new Switcher<Domain, Domain>(fakeDomain))
            {
                sut.SignOut().Should().BeOfType<EmptyResult>();
                stateManager.Received(1).Clear();
                owinService.Received(1).SignOut(Arg.Any<string>());
                sitecoreContext.Received(1).SignOut();
            }
        }

        [Theory]
        [AutoFakeUserDbData(IsAuthenticated = false)]
        public void Non_Authenticated_User_Should_Be_Redirected_To_My_Account([Greedy] LoginController sut)
        {
            sut.SignOut().Should().BeOfType<RedirectResult>().Which.Url.Should().Be("/");
        }

        [Theory]
        [AutoDbData]
        public void Authorized_User_Should_Update_Profile_Get_Empty_Result([Frozen] IOwinService owinService, [Greedy] LoginController sut)
        {
            sut.UpdateProfile().Should().BeOfType<EmptyResult>();
            owinService.Received(1).Challenge(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [AutoDbData]
        public void Password_Reset_Should_Call_Owin_Challange([Frozen] IOwinService owinService, [Greedy] LoginController sut)
        {
            sut.PasswordReset();
            owinService.Received(1).Challenge(Arg.Any<string>(), Arg.Any<string>(), AzureAdB2CIdentityProviderProcessor.PasswordResetPolicyId);
        }

        [Theory]
        [AutoDbData]
        public void Authorized_User_Should_Change_Password_Get_Empty_Result([Frozen] IOwinService owinService, [Greedy] LoginController sut)
        {
            sut.ChangePassword().Should().BeOfType<EmptyResult>();
            owinService.Received(1).Challenge(Arg.Any<string>(), Arg.Any<string>(), AzureAdB2CIdentityProviderProcessor.PasswordChangePolicyId);
        }

    }
}
