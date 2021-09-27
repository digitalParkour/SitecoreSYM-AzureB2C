using Sitecore.Data;
using SYMB2C.Foundation.LinkTracker.Data.Constants;

namespace SYMB2C.Foundation.LinkTracker.Events.Processors
{
    public class CampaignProvider : TrackedEventProviderProcessor
    {
        protected override string DefinitionItemPath => LinkTrackerConstants.SitecoreCampaignPath;
        protected override ID TemplateId => LinkTrackerConstants.CampaignTemplateID;
        protected override int Index => 3;
    }
}