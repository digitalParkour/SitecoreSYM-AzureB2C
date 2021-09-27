using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Foundation.Icons.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;
using System.Web.Mvc;

namespace SYMB2C.Foundation.Icons.Controllers
{
    public class SvgLegendController : VariantsController
    {
        public SvgLegendController() : this(ServiceLocator.ServiceProvider.GetService<ISvgLegendRepository>())
        {

        }

        public SvgLegendController(ISvgLegendRepository repository) : base(repository) { }
    }
}