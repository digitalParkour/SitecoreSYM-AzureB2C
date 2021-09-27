using Sitecore.Pipelines.RenderField;
using SYMB2C.Foundation.LinkTracker.Data.Constants;

namespace SYMB2C.Foundation.LinkTracker.Pipelines.RenderField
{
    public class SetGoalAttributeOnLink : SetTrackedAttributeOnLink
    {
        public override void Process(RenderFieldArgs args)
        {
            if(args.FieldTypeKey == "rich text")
            {
                if(args.Result.FirstPart.Contains(this.BeginningHtml))
                {
                    var linkStart = args.Result.FirstPart.IndexOf(this.BeginningHtml);
                    var linkEnd = args.Result.FirstPart.IndexOf("</a>") + 4;
                    var textLink = args.Result.FirstPart.Substring(linkStart, linkEnd - linkStart);

                    if (!this.CanProcessRichText(args, textLink))
                    {
                        return;
                    }
                }                 
            }
            else if (!this.CanProcess(args))
            {
                return;
            }

            string shouldTriggerGoal;

            if (!string.IsNullOrEmpty(this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.GoalTriggerAttName)) || !string.IsNullOrEmpty(this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.GoalAttributeName)))
            {
                shouldTriggerGoal = (this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.GoalTriggerAttName) == "1" || !string.IsNullOrEmpty(this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.GoalAttributeName))) ? "true" : "false";
            }
            else
            {
                shouldTriggerGoal = "false";
            }
            if (shouldTriggerGoal == "true")
            {
                if(args.FieldTypeKey == "rich text")
                {
                    var goalIdIndex = args.Result.FirstPart.IndexOf("goalid=");
                    var startIndex = args.Result.FirstPart.LastIndexOf("<a", goalIdIndex);
                    var endIndex = args.Result.FirstPart.IndexOf("</a>", goalIdIndex);

                    var link = args.Result.FirstPart.Substring(startIndex, endIndex - startIndex);
                    link = this.AddOrExtendAttributeValue(link, "onclick", "triggerGoal('" + this.GetXmlAttributeValue(args.FieldValue, this.XmlAttributeName) + "', '" + shouldTriggerGoal + "', '" + this.GetXmlAttributeValue(link, LinkTrackerConstants.GoalDataAttName) + "');");
                    args.Result.FirstPart = args.Result.FirstPart.Remove(startIndex, endIndex - startIndex);
                    args.Result.FirstPart = args.Result.FirstPart.Insert(startIndex, link);
                }
                else
                {
                    args.Result.FirstPart = this.AddOrExtendAttributeValue(args.Result.FirstPart, "onclick", "triggerGoal('" + this.GetXmlAttributeValue(args.FieldValue, this.XmlAttributeName) + "', '" + shouldTriggerGoal + "', '" + this.GetXmlAttributeValue(args.FieldValue, LinkTrackerConstants.GoalDataAttName) + "');");
                }
                
            }           
        }
    }
}   