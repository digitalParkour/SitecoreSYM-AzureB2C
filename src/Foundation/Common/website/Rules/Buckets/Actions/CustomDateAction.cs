using Sitecore.Buckets.Rules.Bucketing;
using Sitecore.Buckets.Util;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Rules.Actions;
using Sitecore;

namespace SYMB2C.Foundation.Common.Rules.Buckets.Actions
{
    public class CustomDateAction<T> : RuleAction<T> where T : BucketingRuleContext
    {
        public ID FieldId { get; set; }
        public string Format { get; set; }

        public override void Apply(T ruleContext)
        {
            var date = ruleContext.CreationDate;

            var item = ruleContext.Database.GetItem(ruleContext.NewItemId);

            if (item == null)
            {
                return;
            }

            DateField field = (DateField)item.Fields[FieldId];

            if (field != null)
            {
                date = field.DateTime;
            }

            if (date == null)
            {
                return;
            }

            var format = Format;
            if (string.IsNullOrEmpty(format))
            {
                format = BucketConfigurationSettings.BucketFolderPath;
            }

            ruleContext.ResolvedPath = date.ToString(format, Context.Culture);
        }
    }
}