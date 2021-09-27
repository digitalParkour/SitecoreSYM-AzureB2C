using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Foundation.Mail.Repositories;
using Sitecore.DependencyInjection;

namespace SYMB2C.Foundation.Mail.IoC
{
    public class MailConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(IMailService), typeof(MailService));
        }
    }
}