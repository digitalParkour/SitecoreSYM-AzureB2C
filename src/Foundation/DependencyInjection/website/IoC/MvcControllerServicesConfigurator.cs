namespace SYMB2C.Foundation.DependencyInjection.IoC
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("SYMB2C.Feature.*");
            serviceCollection.AddMvcControllers("Feature.Forms*");
            serviceCollection.AddApiControllers("SYMB2C.Feature.*");
            serviceCollection.AddApiControllers("Feature.Forms*");

            serviceCollection.AddClassesWithServiceAttribute("SYMB2C.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("SYMB2C.Foundation.*");
            serviceCollection.AddClassesWithServiceAttribute("Feature.Forms*");
        }
    }
}