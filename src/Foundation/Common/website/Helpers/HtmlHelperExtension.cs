using System.Web.Mvc;
using System.Web.Mvc.Html;
using SYMB2C.Foundation.Common.Attributes;
using Sitecore.Mvc;

namespace SYMB2C.Foundation.Common.Helpers
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString AddUniqueFormId(this HtmlHelper htmlHelper)
        {
            var uid = htmlHelper.Sitecore().CurrentRendering?.UniqueId;
            return !uid.HasValue ? null : htmlHelper.Hidden(ValidateRenderingIdAttribute.FormUniqueid, uid.Value);
        }
    }
}