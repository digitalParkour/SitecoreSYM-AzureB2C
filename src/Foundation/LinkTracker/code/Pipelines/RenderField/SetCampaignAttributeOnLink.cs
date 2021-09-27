using Sitecore.Pipelines.RenderField;
using SYMB2C.Foundation.LinkTracker.Data.Constants;

namespace SYMB2C.Foundation.LinkTracker.Pipelines.RenderField
{
    public class SetCampaignAttributeOnLink : SetTrackedAttributeOnLink
    {
        public override void Process(RenderFieldArgs args)
        {
            if (!this.CanProcess(args))
            {
                return;
            }

            string shouldTriggerCampaign;

            if (!string.IsNullOrEmpty(this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.CampaignTriggerAttName)))
            {
                shouldTriggerCampaign = this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.CampaignTriggerAttName) == "1" ? "true" : "false";
            }
            else
            {
                shouldTriggerCampaign = "false";
            }
            if(shouldTriggerCampaign == "true")
            {
                args.Result.FirstPart = this.AddOrExtendAttributeValue(args.Result.FirstPart, "onclick", "triggerCampaign('" + this.GetXmlAttributeValue(args.FieldValue, this.XmlAttributeName) + "', '" + shouldTriggerCampaign + "', '" + this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.CampaignDataAttName) + "');");
            }           
        }
    }
}