using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SYMB2C.Foundation.Icons.Repositories;

namespace SYMB2C.Foundation.Icons.IoC
{
    public class SvgLegendConfigurator : IServicesConfigurator

    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISvgLegendRepository, SvgLegendRepository>();
        }

    }
}