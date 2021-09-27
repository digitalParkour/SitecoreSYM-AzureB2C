using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using Sitecore.Web.PageCodes;
using System.Xml.Linq;

namespace SYMB2C.Foundation.LinkTracker.sitecore.shell.client.Applications.Dialogs
{
    /// <summary>
    /// The insert anchor.
    /// </summary>
    public class InsertAnchor : PageCodeBase
    {
        /// <summary>
        /// Gets or sets the alternate text.
        /// </summary>
        /// <value>
        /// The alternate text.
        /// </value>
        public Rendering AlternateText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the anchor.
        /// </summary>
        /// <value>
        /// The anchor.
        /// </value>
        public Rendering Anchor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the back.
        /// </summary>
        public Rendering Back
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        public Rendering Style
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public Rendering Text
        {
            get;
            set;
        }

        //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
        public Rendering Goal
        {
            get;
            set;
        }
        public Rendering GoalLoadedValue
        {
            get;
            set;
        }
        public Rendering Event
        {
            get;
            set;
        }
        public Rendering EventLoadedValue
        {
            get;
            set;
        }
        //////////////////////////////////////////////////////// END CUSTOM BIT ////////////////////////////////////////////////////////

        public InsertAnchor()
        {
        }

        /// <summary>
        /// Gets the XML attribute value.
        /// </summary>
        /// <param name="element">The XML Element to look for a certain attribute on.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>Attributes value. If attribute wasn't found it just returns an empty string.</returns>
        private static string GetXmlAttributeValue(XElement element, string attrName)
        {
            if (element.Attribute(attrName) == null)
            {
                return string.Empty;
            }
            return element.Attribute(attrName).Value;
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            this.ReadRefererParamAndSetClickAction();
            this.ReadRawValueAndSetInitValues();
        }

        /// <summary>
        /// Reads the raw value and set initial values.
        /// </summary>
        private void ReadRawValueAndSetInitValues()
        {
            bool queryString = WebUtil.GetQueryString("hdl") != string.Empty;
            string empty = string.Empty;
            if (queryString)
            {
                empty = UrlHandle.Get()["va"];
            }
            if (empty == string.Empty)
            {
                return;
            }
            XElement xElement = XElement.Parse(empty);
            if (InsertAnchor.GetXmlAttributeValue(xElement, "linktype") == "anchor")
            {
                this.Text.Parameters["Text"] = InsertAnchor.GetXmlAttributeValue(xElement, "text");
                this.Style.Parameters["Text"] = InsertAnchor.GetXmlAttributeValue(xElement, "class");
                this.Anchor.Parameters["Text"] = InsertAnchor.GetXmlAttributeValue(xElement, "anchor");
                this.AlternateText.Parameters["Text"] = InsertAnchor.GetXmlAttributeValue(xElement, "title");

                //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
                this.GoalLoadedValue.Parameters["Text"] = InsertAnchor.GetXmlAttributeValue(xElement, "goal");
                this.EventLoadedValue.Parameters["Text"] = InsertAnchor.GetXmlAttributeValue(xElement, "pageevent");
                //this.Goal.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "goal");
                //this.Event.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "pageevent");
                //////////////////////////////////////////////////////// END CUSTOM BIT ////////////////////////////////////////////////////////

            }
        }

        /// <summary>
        /// Reads the referrer parameter and set click action.
        /// </summary>
        private void ReadRefererParamAndSetClickAction()
        {
            string queryString = WebUtil.GetQueryString("ref");
            string str = WebUtil.GetQueryString("hdl");
            string queryString1 = WebUtil.GetQueryString("ro");
            if (queryString == "tree")
            {
                this.Back.Parameters["Click"] = string.Concat(new string[] { "javascript:window.location.assign('/sitecore/client/applications/Dialogs/InsertLinkViaTreeDialog?hdl=", str, "&ro=", queryString1, "');" });
            }
            if (queryString == "list")
            {
                this.Back.Parameters["Click"] = string.Concat(new string[] { "javascript:window.location.assign('/sitecore/client/applications/Dialogs/InsertLinkDialog?hdl=", str, "&ro=", queryString1, "');" });
            }
            if (queryString == string.Empty)
            {
                this.Back.Parameters["IsVisible"] = "false";
            }
        }
    }
}