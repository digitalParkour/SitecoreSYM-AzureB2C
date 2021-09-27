using SYMB2C.Foundation.LinkTracker.Events.Processors;
using Sitecore.Data.Items;

namespace SYMB2C.Foundation.LinkTracker.Tasks
{
    public class Reload
    {
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            ReloadTask();
        }
        public void ReloadTask()
        {
            (new GoalProvider()).Reload();
            (new PageEventProvider()).Reload();
            (new CampaignProvider()).Reload();
        }
    }
}