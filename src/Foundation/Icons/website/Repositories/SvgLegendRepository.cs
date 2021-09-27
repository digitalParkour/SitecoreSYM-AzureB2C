using Microsoft.Extensions.DependencyInjection;
using SYMB2C.Foundation.Icons.Models;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SYMB2C.Foundation.Icons.Repositories
{
    public class SvgLegendRepository : VariantsRepository, ISvgLegendRepository
    {
        private IMultisiteContext _context;

        public SvgLegendRepository() : base()
        {
            _context = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IMultisiteContext>();
        }

        public SvgLegendRepository (IMultisiteContext context) : base()
        {
            _context = context;
        }

        public override IRenderingModelBase GetModel()
        {
            SvgLegendModel model = new SvgLegendModel();
            FillBaseProperties(model);

            return model;
        }

        /// <summary>
        /// Populates the svg legend model
        /// </summary>
        /// <param name="m"></param>
        protected override void FillBaseProperties(object m)
        {
            base.FillBaseProperties(m);
            SvgLegendModel model = m as SvgLegendModel;
            if (model == null)
            {
                return;
            }

            model.Legend = new List<string>();

            // Get site setting data
            var settingsItem = _context.GetSettingsItem(Sitecore.Context.Item);
            var mySettingItem = settingsItem?.GetChildren()?.FirstOrDefault(x => x.TemplateID == Templates.MySetting.ID);
            Sitecore.Data.Fields.MultilistField multiselectField = mySettingItem.Fields["Svg_Legend"];
            Sitecore.Data.Items.Item[] items = multiselectField.GetItems();
            Sitecore.Data.Items.MediaItem mediaItem;

            for (int i = 0; i < items.Length; i++)
            {
                // Add a svg definition to the legend 
                mediaItem = new Sitecore.Data.Items.MediaItem(items[i]);
                model.Legend.Add(CreateSvgDefinition(mediaItem));
            }
        }

        /// <summary>
        /// Creates the string for a single svg
        /// definition
        /// </summary>
        /// <param name="mediaItem"></param>
        /// <returns></returns>
        private string CreateSvgDefinition(MediaItem mediaItem)
        {
            string result,
                   definition;

            using (StreamReader reader = new StreamReader(mediaItem.GetMediaStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            // Get svg string
            XDocument svg = XDocument.Parse(result);
            result = svg.LastNode.ToString();

            // Get content between svg tags
            int contentStart = result.IndexOf(">") + ">".Length;

            int contentEnd = result.LastIndexOf("<");
            string content = result.Substring(contentStart, contentEnd - contentStart);

            // Get viewbox string
            int viewboxStart = result.ToLower().IndexOf("viewbox=\"");
            int viewboxEnd = result.IndexOf("\"", viewboxStart + "viewBox=\"".Length) + 1;
            string viewbox = result.Substring(viewboxStart, viewboxEnd - viewboxStart);

            // Create definition string
            definition = "<symbol id=\"" + mediaItem.DisplayName + "\" " + viewbox + ">";
            definition += content;
            definition += "</symbol>";

            return definition;
        }
    }
}