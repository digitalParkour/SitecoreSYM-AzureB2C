using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using Sitecore.Web.PageCodes;
using System;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml.Linq;

namespace SYMB2C.Foundation.LinkTracker.sitecore.shell.client.Applications.Dialogs
{
    /// <summary>
    /// The insert email.
    /// </summary>
    public class InsertEmail : PageCodeBase
    {
        /// <summary>
        /// Gets or sets the back.
        /// </summary>
        public Rendering Back
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        public Rendering DisplayedText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public Rendering EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        public Rendering Style
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public Rendering Subject
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


        public InsertEmail()
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
            this.ReadRawValueAndSetInitValues();
            this.ReadRefererParamAndSetClickAction();
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
            if (InsertEmail.GetXmlAttributeValue(xElement, "linktype") == "mailto")
            {
                string xmlAttributeValue = InsertEmail.GetXmlAttributeValue(xElement, "url");
                bool flag = xmlAttributeValue.IndexOf("?subject=", StringComparison.OrdinalIgnoreCase) != -1;
                string str = xmlAttributeValue.Replace("mailto:", string.Empty);
                string empty1 = string.Empty;
                if (flag)
                {
                    string[] strArrays = str.Replace("subject=", string.Empty).Split(new char[] { '?' });
                    str = strArrays[0];
                    empty1 = HttpUtility.UrlDecode(strArrays[1]);
                }
                this.DisplayedText.Parameters["Text"] = InsertEmail.GetXmlAttributeValue(xElement, "text");
                this.Style.Parameters["Text"] = InsertEmail.GetXmlAttributeValue(xElement, "style");
                this.EmailAddress.Parameters["Text"] = str;
                this.Subject.Parameters["Text"] = empty1;


                //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
                this.GoalLoadedValue.Parameters["Text"] = InsertEmail.GetXmlAttributeValue(xElement, "goal");
                this.EventLoadedValue.Parameters["Text"] = InsertEmail.GetXmlAttributeValue(xElement, "pageevent");
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