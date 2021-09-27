using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell;
using Sitecore.Speak.Applications;
using Sitecore.Web;
using System;
using System.Net;
using System.Xml.Linq;

namespace SYMB2C.Foundation.LinkTracker.sitecore.shell.client.Applications.Dialogs
{
    /// <summary>
    /// InsertLinkDialogTree class
    /// </summary>
    public class InsertLinkDialogTree : Sitecore.Web.PageCodes.PageCodeBase
    {
        /// <summary>
        /// The target active browser item id.
        /// </summary>
        private string targetActiveBrowserItemId = "{C5FA4571-37B3-472C-BDA1-0FADC2D2EFA7}";

        /// <summary>
        /// The target new browser item id.
        /// </summary>
        private string targetNewBrowserItemId = "{02A6C72E-17BB-48C5-8D35-AF9C494ED6BA}";

        /// <summary>
        /// The target custom item id.
        /// </summary>
        private string targetCustomItemId = "{07CF2A84-9C22-4E85-8F3F-C301AADF5218}";

        /// <summary>
        /// The target active browser item id.
        /// </summary>
        private string targetParent = "{21E8FEC9-4FB7-4B08-8649-DB8870F33E80}";

        /// <summary>
        /// The target new browser item id.
        /// </summary>
        private string targetBlank = "{FBC64F5C-BE88-4D76-ADEB-BFEEBAD6B01B}";

        /// <summary>
        /// The target top item id.
        /// </summary>
        private string targetTop = "{8CEC76AC-2B59-421B-ACFF-271773F5F2C2}";

        /// <summary>
        /// Gets or sets the Alt Text textbox.
        /// </summary>
        /// <value>
        /// The Alt Text textbox.
        /// </value>
        public Rendering AltText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the anchor text.
        /// </summary>
        public Rendering AnchorText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CustomUrl textbox.
        /// </summary>
        /// <value>
        /// The custom URL.
        /// </value>
        public Rendering CustomUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the DataSource.
        /// </summary>
        /// <value>
        /// The DataSource.
        /// </value>    
        public Rendering DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the insert anchor button.
        /// </summary>
        /// <value>
        /// The insert anchor button.
        /// </value>
        public Rendering InsertAnchorButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the insert email button.
        /// </summary>
        /// <value>
        /// The insert email button.
        /// </value>
        public Rendering InsertEmailButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list view toggle button.
        /// </summary>
        /// <value>
        /// The list view toggle button.
        /// </value>
        public Rendering ListViewToggleButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the QueryString textbox.
        /// </summary>
        /// <value>
        /// The QueryString textbox.
        /// </value>
        public Rendering QueryString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the StyleClass textbox.
        /// </summary>
        /// <value>
        /// The StyleClass textbox.
        /// </value>
        public Rendering StyleClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target combo box.
        /// </summary>
        /// <value>
        /// The target combo box.
        /// </value>
        public Rendering Target
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

        /// <summary>
        /// Gets or sets the item ID, which is defined as Active Brower in Target dropdown list.
        /// </summary>
        /// <value>
        /// The item id.
        /// </value>
        public string TargetActiveBrowserItemId
        {
            get
            {
                return this.targetActiveBrowserItemId;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                this.targetActiveBrowserItemId = value;
            }
        }

        /// <summary>
        /// Gets or sets the item ID, which is defined as Custom in Target dropdown list.
        /// </summary>
        /// <value>
        /// The item id.
        /// </value>
        public string TargetCustomItemId
        {
            get
            {
                return this.targetCustomItemId;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                this.targetCustomItemId = value;
            }
        }

        /// <summary>
        /// Gets or sets the TargetLoadedValue control.
        /// </summary>
        /// <value>
        /// The target loaded value.
        /// </value>
        public Rendering TargetLoadedValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the item ID, which is defined as New Brower in Target dropdown list.
        /// </summary>
        /// <value>
        /// The item id.
        /// </value>
        public string TargetNewBrowserItemId
        {
            get
            {
                return this.targetNewBrowserItemId;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                this.targetNewBrowserItemId = value;
            }
        }

        /// <summary>
        /// Gets or sets the Text Description textbox.
        /// </summary>
        /// <value>
        /// The Text Description textbox.
        /// </value>
        public Rendering TextDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tree view.
        /// </summary>
        /// <value>
        /// The tree view.
        /// </value>
        public Rendering TreeView
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tree view toggle button.
        /// </summary>
        /// <value>
        /// The tree view toggle button.
        /// </value>
        public Rendering TreeViewToggleButton
        {
            get;
            set;
        }

        public InsertLinkDialogTree()
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
        /// Executes this instance.
        /// </summary>
        public override void Initialize()
        {
            string setting = Settings.GetSetting("BucketConfiguration.ItemBucketsEnabled");
            this.ListViewToggleButton.Parameters["IsVisible"] = setting;
            this.TreeViewToggleButton.Parameters["IsVisible"] = setting;
            this.TreeView.Parameters["ShowHiddenItems"] = UserOptions.View.ShowHiddenItems.ToString();
            this.TreeView.Parameters["ContentLanguage"] = WebUtil.GetQueryString("la");
            this.ReadQueryParamsAndUpdatePlaceholders();
        }

        /// <summary>
        /// Reads the root parameter and update root placeholders.
        /// </summary>
        private void ReadQueryParamsAndUpdatePlaceholders()
        {
            Item item;
            string queryString = WebUtil.GetQueryString("ro");
            string str = WebUtil.GetQueryString("hdl");
            if (!string.IsNullOrEmpty(queryString) && queryString != "{0}")
            {
                this.TreeView.Parameters["RootItem"] = queryString;
            }
            this.InsertAnchorButton.Parameters["Click"] = string.Format(this.InsertAnchorButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str));
            this.InsertEmailButton.Parameters["Click"] = string.Format(this.InsertEmailButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str));
            this.ListViewToggleButton.Parameters["Click"] = string.Format(this.ListViewToggleButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str));
            bool empty = str != string.Empty;
            string empty1 = string.Empty;
            if (empty)
            {
                empty1 = UrlHandle.Get()["va"];
            }
            if (empty1 == string.Empty)
            {
                return;
            }
            XElement xElement = XElement.Parse(empty1);
            if (InsertLinkDialogTree.GetXmlAttributeValue(xElement, "linktype") == "internal")
            {
                string xmlAttributeValue = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "id");
                if (string.IsNullOrWhiteSpace(xmlAttributeValue))
                {
                    return;
                }
                if (string.IsNullOrEmpty(queryString))
                {
                    item = null;
                }
                else
                {
                    item = Sitecore.ClientHost.Databases.ContentDatabase.GetItem(queryString);
                }
                Item rootItem = item ?? Sitecore.ClientHost.Databases.ContentDatabase.GetRootItem();
                Item mediaItemFromQueryString = SelectMediaDialog.GetMediaItemFromQueryString(xmlAttributeValue);
                if (rootItem != null && mediaItemFromQueryString != null && mediaItemFromQueryString.Paths.LongID.StartsWith(rootItem.Paths.LongID))
                {
                    this.TreeView.Parameters["PreLoadPath"] = string.Concat(rootItem.ID, mediaItemFromQueryString.Paths.LongID.Substring(rootItem.Paths.LongID.Length));
                }
                this.TextDescription.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "text");
                this.AltText.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "title");
                this.StyleClass.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "class");
                this.QueryString.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "querystring");
                this.AnchorText.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "anchor");
                
                //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
                this.GoalLoadedValue.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "goalid");
                this.EventLoadedValue.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "eventid");
                //this.Goal.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "goal");
                //this.Event.Parameters["Text"] = InsertLinkDialogTree.GetXmlAttributeValue(xElement, "pageevent");
                //////////////////////////////////////////////////////// END CUSTOM BIT ////////////////////////////////////////////////////////

                this.SetupTargetDropbox(xElement);
            }
        }

        /// <summary>
        /// Setups the target dropbox.
        /// </summary>
        /// <param name="fieldContent">Content of the field.</param>
        private void SetupTargetDropbox(XElement fieldContent)
        {
            string targetCustomItemId;
            string xmlAttributeValue = InsertLinkDialogTree.GetXmlAttributeValue(fieldContent, "target");
            if (xmlAttributeValue.Equals("_blank", StringComparison.OrdinalIgnoreCase))
            {
                targetCustomItemId = string.Concat(this.TargetNewBrowserItemId, ",", this.targetBlank);
            }
            else if (xmlAttributeValue.Equals("_top", StringComparison.OrdinalIgnoreCase))
            {
                targetCustomItemId = this.targetTop;
            }
            else if (string.IsNullOrWhiteSpace(xmlAttributeValue) || xmlAttributeValue.Equals("_parent", StringComparison.OrdinalIgnoreCase))
            {
                targetCustomItemId = string.Concat(this.TargetActiveBrowserItemId, ",", this.targetParent);
            }
            else
            {
                targetCustomItemId = this.TargetCustomItemId;
                string str = xmlAttributeValue.Split(new char[] { '|' })[0];
                this.CustomUrl.Parameters["Text"] = str;
            }
            this.TargetLoadedValue.Parameters["Text"] = targetCustomItemId;
        }
        
    }
}