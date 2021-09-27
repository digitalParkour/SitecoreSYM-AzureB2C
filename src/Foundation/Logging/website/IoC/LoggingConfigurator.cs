using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Foundation.Logging.Repositories;
using Sitecore.DependencyInjection;

namespace SYMB2C.Foundation.Logging.IoC
{
    public class LoggingConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(IEventLogger), typeof(EventLogger));
            serviceCollection.AddSingleton(typeof(IEmailLogger), typeof(EmailLogger));
        }
    }
}