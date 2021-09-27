using Sitecore.Pipelines.RenderField;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.Xml.Xsl;

namespace SYMB2C.Foundation.Icons.Pipelines.RenderField
{
    public class GetImageFieldValuePatchSVG : GetImageFieldValue
    {
        private IMultisiteContext _context;

        public GetImageFieldValuePatchSVG(IMultisiteContext context) : base()
        {
            _context = context;
        }

        /// <summary>
        /// Creates the renderer.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The renderer.
        /// </returns>
        protected override ImageRenderer CreateRenderer()
        {
            return new SvgImageRenderer(_context.GetSettingsItem(Sitecore.Context.Item));
        }
    }
}