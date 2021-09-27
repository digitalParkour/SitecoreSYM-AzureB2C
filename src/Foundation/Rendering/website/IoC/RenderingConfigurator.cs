using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using SYMB2C.Foundation.Rendering.Cache;

namespace SYMB2C.Foundation.Rendering.IoC
{
    public class RenderingConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IScribanRenderCache, ScribanRenderCache>();
        }
    }
}