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
    public class BeginEndFieldProcessor : FieldRendererBase, IGenerateScribanContextProcessor
    {
        protected readonly IPageMode PageMode;

        private readonly IScribanRenderCache _scribanRenderCache;
        private delegate string BeginRenderDelegate(Item item, object fieldName, ScriptArray parameters);
        private delegate string EndRenderDelegate();

        public BeginEndFieldProcessor(IPageMode pageMode, IScribanRenderCache scribanRenderCache)
        {
            PageMode = pageMode;
            _scribanRenderCache = scribanRenderCache;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            RenderingWebEditingParams = args.RenderingWebEditingParams;
            args.GlobalScriptObject.Import("sc_beginfield", new BeginRenderDelegate(BeginRenderImpl));
            args.GlobalScriptObject.Import("sc_endfield", new EndRenderDelegate(EndRenderImpl));
        }

        public string BeginRenderImpl(Item item, object field, ScriptArray parameters)
        {
            var parametersCollection = new NameValueCollection();
            if (parameters != null)
            {
                foreach (object parameter in parameters)
                {
                    if (parameter is ScriptArray scriptArray && scriptArray.Count > 1)
                    {
                        parametersCollection.Add(scriptArray[0].ToString(), scriptArray[1].ToString());
                    }
                }
            }
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

            RenderFieldResult fieldRenderer = CreateFieldRenderer(item, fieldName, parametersCollection).RenderField();
            _scribanRenderCache.PushEndFieldStack(fieldRenderer.LastPart ?? Empty);
            return fieldRenderer.FirstPart;
        }

        public string EndRenderImpl()
        {
            return _scribanRenderCache.PopEndFieldStack();
        }
    }
}