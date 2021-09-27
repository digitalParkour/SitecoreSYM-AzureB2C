using Sitecore.Data.Fields;

namespace SYMB2C.Foundation.Common.Models.Sitecore
{
    public class GeneralLink
    {
        public GeneralLink(LinkField linkField)
        {
            if (linkField != null)
            {
                this.IsInternal = linkField.IsInternal;
                this.Anchor = linkField.Anchor;
                this.Class = linkField.Class;
                this.Text = linkField.Text;
                this.Url = linkField.GetFriendlyUrl();
                this.Target = linkField.Target;
            }
        }

        public bool IsInternal { get; set; }

        public string Anchor { get; set; }

        public string Class { get; set; }

        public string Url { get; set; }

        public string Text { get; set; }

        public string Target { get; set; }
    }
}