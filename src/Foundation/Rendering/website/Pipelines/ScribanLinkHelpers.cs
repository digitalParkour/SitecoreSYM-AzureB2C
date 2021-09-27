using System.Collections.Generic;
using System.Collections.Specialized;
using SYMB2C.Foundation.Rendering.Cache;
using Scriban.Runtime;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;
using Sitecore.Xml.Xsl;
using static System.String;
using FieldRendererBase = Sitecore.XA.Foundation.Scriban.FieldRendererBase;

namespace SYMB2C.Foundation.Rendering.Pipelines
{
    public class ScribanLinkHelpers : FieldRendererBase, IGenerateScribanContextProcessor
    {
        protected readonly IPageMode PageMode;

        private delegate string LinkText(Item item, object fieldName);

        public ScribanLinkHelpers(IPageMode pageMode)
        {
            PageMode = pageMode;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            RenderingWebEditingParams = args.RenderingWebEditingParams;
            args.GlobalScriptObject.Import("sc_link_text", new LinkText(LinkTextRenderImpl));
        }

        public string LinkTextRenderImpl(Item item, object field)
        {
            string fieldName = null;
            switch (field)
            {
                case string fieldString:
                    fieldName = fieldString;
                    break;

                case ScriptArray array:
                    using (List<object>.Enumerator enumerator = array.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current is string current)
                            {
                                fieldName = fieldName ?? current;
                                bool experienceEditorEditing = PageMode.IsExperienceEditorEditing;
                                Field currentField = item.Fields[current];
                                if (currentField != null)
                                {
                                    if (experienceEditorEditing)
                                    {
                                        fieldName = current;
                                        break;
                                    }
                                    if (!IsNullOrWhiteSpace(currentField.GetValue(true)))
                                    {
                                        fieldName = current;
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
            }

            if (string.IsNullOrWhiteSpace(fieldName))
                return string.Empty;

            var linkField = (Sitecore.Data.Fields.LinkField)item.Fields[fieldName];
            return linkField?.Text;

        }

    }
}