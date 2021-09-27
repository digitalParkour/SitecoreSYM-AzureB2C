using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Foundation.Caching;
using NSubstitute;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYMB2C.Foundation.Testing.Attributes
{
    public class AutoFakeStateManagerAttribute : AutoDbDataAttribute
    {
        public AutoFakeStateManagerAttribute()
        {
            this.Fixture.Register(() =>
            {
                var fakeStateManager = Substitute.For<IStateManager>();
                var fakeServiceProvider = Substitute.For<IServiceProvider>();
                fakeStateManager.Get<string>("test").Returns("somevalue");
                
                var list = new List<IStateManager>();
                list.Add(fakeStateManager);
                fakeServiceProvider.GetServices<IStateManager>().First(x=> x.GetType() == typeof(SessionManager)).Returns(list.First());
                ServiceLocator.ServiceProvider.Returns(fakeServiceProvider);
                return ServiceLocator.ServiceProvider;
            });
        }
    }
}
