using System;
using System.Xml;
using System.Xml.Linq;
using Sitecore.Pipelines.RenderField;
using Sitecore.Xml;

namespace SYMB2C.Foundation.LinkTracker.Pipelines.RenderField
{
    public abstract class SetTrackedAttributeOnLink
    {
        protected string XmlAttributeName { get; set; }

        protected string AttributeName { get; set; }

        protected string BeginningHtml { get; set; }

        public abstract void Process(RenderFieldArgs args);

        protected virtual bool CanProcess(RenderFieldArgs args)
        {
            return !string.IsNullOrWhiteSpace(this.AttributeName)
                   && !string.IsNullOrWhiteSpace(this.BeginningHtml)
                   && !string.IsNullOrWhiteSpace(this.XmlAttributeName)
                   && args != null
                   && args.Result != null
                   && args.FieldTypeKey == "general link"
                   && !string.IsNullOrWhiteSpace(args.Result.FirstPart)
                   && args.Result.FirstPart.ToLower().StartsWith(this.BeginningHtml.ToLower())
                   && this.HasXmlAttributeValue(args.FieldValue, this.AttributeName);
        }

        protected virtual bool CanProcessRichText(RenderFieldArgs args, string link)
        {
            return !string.IsNullOrWhiteSpace(this.AttributeName)
                   && !string.IsNullOrWhiteSpace(this.BeginningHtml)
                   && !string.IsNullOrWhiteSpace(this.XmlAttributeName)
                   && args != null
                   && args.Result != null
                   && !string.IsNullOrWhiteSpace(args.Result.FirstPart)
                   && this.HasXmlAttributeValue(link, this.AttributeName);
        }

        protected virtual bool HasXmlAttributeValue(string linkXml, string attributeName)
        {
            return !string.IsNullOrWhiteSpace(this.GetXmlAttributeValue(linkXml, attributeName));
        }

        protected virtual string GetXmlAttributeValue(string linkXml, string attributeName)
        {
            try
            {
                if (!linkXml.Contains(this.BeginningHtml))
                {
                    if(isValidXml(linkXml))
                    {
                        XmlDocument xmlDocument = XmlUtil.LoadXml(linkXml);

                        XmlNode node = xmlDocument?.SelectSingleNode("/link");

                        if (node == null)
                        {
                            return string.Empty;
                        }

                        return XmlUtil.GetAttribute(attributeName, node);
                    }
                    else
                    {
                        return string.Empty;
                    }   
                }

                if (!linkXml.Contains(attributeName))
                {
                    return string.Empty;
                }

                var attributeStart = linkXml.IndexOf(attributeName) + attributeName.Length + 2;
                var attributeEnd = linkXml.IndexOf('"', attributeStart);

                return linkXml.Substring(attributeStart, attributeEnd - attributeStart);
            }
            // Ignore various parsing exceptions as other fields can be false positives starting with "<a"
            catch (XmlException ex)
            {
                Sitecore.Diagnostics.Log.Debug($"LinkTracker - Unable to parse XML: {ex.Message}", this);
                return string.Empty;
            }
        }

        protected virtual string AddOrExtendAttributeValue(string html, string attributeName, string attributeValue)
        {
            if (string.IsNullOrWhiteSpace(html) || string.IsNullOrWhiteSpace(attributeName) || string.IsNullOrWhiteSpace(attributeValue))
            {
                return html;
            }

            int index = html.LastIndexOf(">", StringComparison.Ordinal);
            if (index < 0)
            {
                return html;
            }

            string existingAttrivute = $"{attributeName}=\"";
            string firstPart, attribute, lastPart;
            int existingAttributeIndex = html.IndexOf(existingAttrivute, StringComparison.OrdinalIgnoreCase);
            if (existingAttributeIndex >= 0)
            {
                int endofExistingAttributeIndex = html.IndexOf("\"", existingAttributeIndex + existingAttrivute.Length,
                    StringComparison.OrdinalIgnoreCase);
                firstPart = html.Substring(0, endofExistingAttributeIndex);
                attribute = attributeValue;
                lastPart = html.Substring(endofExistingAttributeIndex);
                return string.Concat(firstPart, attribute, lastPart);
            }

            firstPart = html.Substring(0, index);
            attribute = $" {attributeName}=\"{attributeValue}\"";
            lastPart = html.Substring(index);
            return string.Concat(firstPart, attribute, lastPart);
        }

        private bool isValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}