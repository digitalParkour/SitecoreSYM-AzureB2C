using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace SYMB2C.Foundation.LinkTracker.Pipelines.ScApi
{
    public class RegisterApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Foundation.ScApi",
                "sc/api/{controller}/{action}", new
                {

                });
        }
    }
}