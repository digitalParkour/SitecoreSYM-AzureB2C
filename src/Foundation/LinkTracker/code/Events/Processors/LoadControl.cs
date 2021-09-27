using Sitecore.Pipelines.Save;

namespace SYMB2C.Foundation.LinkTracker.Events.Processors
{
    public class LoadControl
    {
        public void Process(SaveArgs args)
        {
            Reload();
        }
        
        public void Reload()
        {
            (new GoalProvider()).Reload();
            (new PageEventProvider()).Reload();
            (new CampaignProvider()).Reload();
        }
        
    }
}