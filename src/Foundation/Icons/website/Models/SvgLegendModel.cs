using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SYMB2C.Foundation.Icons.Models
{
    public class SvgLegendModel : VariantsRenderingModel
    {
        public List<string> Legend { get; set; }
    }
}