using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Xml.Xsl;

namespace SYMB2C.Foundation.Icons.Pipelines.RenderField
{
    public class SvgImageRenderer : ImageRenderer
    {
        private Item settingsItem;

        public SvgImageRenderer(Item siteSettingsItem) : base()
        {
            settingsItem = siteSettingsItem;
        }

        public override RenderFieldResult Render()
        {
            var obj = Item;
            if (obj == null)
            {
                return RenderFieldResult.Empty;
            }

            if (IsSvg(obj, out MediaItem mediaItem))
            {
                return new RenderFieldResult(RenderSvgImage(mediaItem));
            }
            else 
            {
                return base.Render();
            }
        }

        private bool IsSvg(Item obj, out MediaItem svgMediaItem) {

            var innerField = obj.Fields[FieldName];
            if (innerField != null)
            {
                var imgField = new ImageField(innerField, FieldValue);
                if (imgField.MediaItem != null)
                {
                    MediaItem mediaItem = new MediaItem(imgField.MediaItem);

                    if (mediaItem.MimeType == "image/svg+xml")
                    {
                        svgMediaItem = mediaItem;
                        return true;
                    }
                }
            }

            svgMediaItem = null;
            return false;
        }

        private string RenderSvgImage(MediaItem mediaItem)
        {
            Assert.ArgumentNotNull(mediaItem, "mediaItem");

            string result;

            using (StreamReader reader = new StreamReader(mediaItem.GetMediaStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            XDocument svg = XDocument.Parse(result);

            if (svg.Document?.Root != null)
            {
                //if (width > 0)
                //{
                //    svg.Document.Root.SetAttributeValue("width", width);
                //}

                //if (height > 0)
                //{
                //    svg.Document.Root.SetAttributeValue("height", height);
                //}

                if(InSvgLegend(mediaItem))
                {
                    string altText = string.Empty;
                    if (!string.IsNullOrWhiteSpace(mediaItem.Alt))
                    {
                        altText = "<title>" + mediaItem.Alt + "</title>";
                    }

                    result = "<svg class=\"icon\">" + altText + "<use xlink:href=\"#" + mediaItem.DisplayName + "\"></use></svg>";
                } else
                {
                    // remove id attribute which is likely to be the same for many SVGs which impacts Accessibility and HTML validation
                    svg.Elements().Where(x => x.HasAttributes && x.Attribute("id") != null).ToList().ForEach(x => x.Attribute("id").Remove());

                    result = svg.ToString();
                }
            }

            return result;
        }

        private bool InSvgLegend(MediaItem mediaItem)
        {
            if (mediaItem == null)
                return false;

            var mySettingItem = settingsItem?.GetChildren()?.FirstOrDefault(x => x.TemplateID == Templates.MySetting.ID);
            var idsInLegend = mySettingItem?["Svg_Legend"];
            // check if ID is in piped list
            return idsInLegend?.ToUpperInvariant().Contains(mediaItem.ID.ToString().ToUpperInvariant()) ?? false;
        }
    }
}