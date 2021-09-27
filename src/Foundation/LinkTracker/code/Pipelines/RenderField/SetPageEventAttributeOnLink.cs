using Sitecore.Pipelines.RenderField;
using SYMB2C.Foundation.LinkTracker.Data.Constants;

namespace SYMB2C.Foundation.LinkTracker.Pipelines.RenderField
{
    public class SetPageEventAttributeOnLink : SetTrackedAttributeOnLink
    {
        public override void Process(RenderFieldArgs args)
        {
            if (!this.CanProcess(args))
            {
                return;
            }

            string shouldTriggerPageEvent;

            if(!string.IsNullOrEmpty(this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.PageEventTriggerAttName)) || !string.IsNullOrEmpty(this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.PageEventAttributeName)))
            {
                shouldTriggerPageEvent = (this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.PageEventTriggerAttName) == "1" || !string.IsNullOrEmpty(this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.PageEventAttributeName))) ? "true" : "false";
            }
            else
            {
                shouldTriggerPageEvent = "false";
            }
            if (shouldTriggerPageEvent == "true")
            {
                args.Result.FirstPart = this.AddOrExtendAttributeValue(args.Result.FirstPart, "onclick", "triggerPageEvent('" + this.GetXmlAttributeValue(args.FieldValue, this.XmlAttributeName) + "', '" + shouldTriggerPageEvent + "', '" + this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.PageEventDataAttName) + "');");
            }          
        }
    }
}