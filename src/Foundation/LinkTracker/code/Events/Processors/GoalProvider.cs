using Sitecore.Data;
using SYMB2C.Foundation.LinkTracker.Data.Constants;

namespace SYMB2C.Foundation.LinkTracker.Events.Processors
{
    public class GoalProvider : TrackedEventProviderProcessor
    {
        protected override string DefinitionItemPath => LinkTrackerConstants.SitecoreGoalPath;
        protected override ID TemplateId => LinkTrackerConstants.GoalTemplateId;
        protected override int Index => 1;
    }
}