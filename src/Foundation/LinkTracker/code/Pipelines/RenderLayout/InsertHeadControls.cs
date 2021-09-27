using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore;

namespace SYMB2C.Foundation.LinkTracker.Pipelines.RenderLayout
{
    public class InsertHeadControls : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (Context.Site.Name == "shell")
            {
                return;
            }

            if (!args.Writer.ToString().Contains(Data.Constants.LinkTrackerConstants.LinkTrackerMgdJSPath))
            {
                if (args.Writer.ToString().Contains("<title>"))
                {
                    args.Writer.WriteLine(Data.Constants.LinkTrackerConstants.JQueryScript);
                    args.Writer.WriteLine(Data.Constants.LinkTrackerConstants.LinkTrackerMgrScript);
                }
            }
        }
    }
}