using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Sitecore.XA.Feature.SiteMetadata.Sitemap;

namespace SYMB2C.Foundation.SEO.Sitemap
{
    public class CustomSitemapGenerator: SitemapGenerator
    {
        protected override XElement BuildAlternateLinkElement(string href, string hreflang, string rel = "alternate")
        {
            return null;
        }
    }
}