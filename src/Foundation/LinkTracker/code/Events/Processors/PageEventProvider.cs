using Sitecore.Data;
using SYMB2C.Foundation.LinkTracker.Data.Constants;

namespace SYMB2C.Foundation.LinkTracker.Events.Processors
{
    public class PageEventProvider : TrackedEventProviderProcessor
    {
        protected override string DefinitionItemPath => LinkTrackerConstants.SitecorePageEventPath;
        protected override ID TemplateId => LinkTrackerConstants.PageEventTemplateId;
        protected override int Index => 2;
    }
}