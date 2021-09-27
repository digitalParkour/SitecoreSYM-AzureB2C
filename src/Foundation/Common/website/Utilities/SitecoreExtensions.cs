using SYMB2C.Foundation.Common.Models.Sitecore;

namespace SYMB2C.Foundation.Common.Utilities
{
    public static class SitecoreExtensions
    {
        public static GeneralLink ToGeneralLink(this Sitecore.Data.Fields.Field field)
        {
            var generalLink = new GeneralLink(new Sitecore.Data.Fields.LinkField(field));

            return generalLink;
        }
    }
}