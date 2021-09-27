using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources.Media;
using Sitecore.Shell.Applications.Dialogs;
using Sitecore.Shell.Framework;
using Sitecore.StringExtensions;
using Sitecore.Utils;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Xml;
using static SYMB2C.Foundation.LinkTracker.Data.Constants.LinkTrackerConstants;

namespace SYMB2C.Foundation.LinkTracker.sitecore.shell.Applications.Dialogs.GeneralLink
{
    public class GeneralLinkForm : LinkForm
    {
        protected Edit Class;
        protected Literal Custom;
        protected Edit CustomTarget;
        protected DataContext InternalLinkDataContext;
        protected TreeviewEx InternalLinkTreeview;
        protected Border InternalLinkTreeviewContainer;
        protected Memo JavascriptCode;
        protected Edit LinkAnchor;
        protected Border MailToContainer;
        protected Edit MailToLink;
        protected DataContext MediaLinkDataContext;
        protected TreeviewEx MediaLinkTreeview;
        protected Border MediaLinkTreeviewContainer;
        protected Border MediaPreview;
        protected Border Modes;
        protected Edit Querystring;
        protected Literal SectionHeader;
        protected Combobox Target;
        protected Edit Text;
        protected Edit Title;
        protected Scrollbox TreeviewContainer;
        protected Button UploadMedia;
        protected Edit Url;
        protected Border UrlContainer;

        protected Combobox GoalId;
        protected Checkbox TriggerGoal;
        protected Combobox EventId;
        protected Checkbox TriggerEvent;

        private NameValueCollection analyticsLinkAttributes;

        protected NameValueCollection AnalyticsLinkAttributes
        {
            get
            {
                if (this.analyticsLinkAttributes == null)
                {
                    this.analyticsLinkAttributes = new NameValueCollection();
                    this.ParseLinkAttributes(this.GetLink());
                }

                return this.analyticsLinkAttributes;
            }
        }

        protected virtual void ParseLinkAttributes(string link)
        {
            Assert.ArgumentNotNull(link, "link");
            XmlDocument xmlDocument = XmlUtil.LoadXml(link);

            XmlNode node = xmlDocument?.SelectSingleNode("/link");
            if (node == null)
            {
                return;
            }

            this.AnalyticsLinkAttributes[Attributes.goalid] = XmlUtil.GetAttribute(Attributes.goalid, node);
            this.AnalyticsLinkAttributes[Attributes.triggergoal] = XmlUtil.GetAttribute(Attributes.triggergoal, node);
            this.AnalyticsLinkAttributes[Attributes.eventid] = XmlUtil.GetAttribute(Attributes.eventid, node);
            this.AnalyticsLinkAttributes[Attributes.triggerevent] = XmlUtil.GetAttribute(Attributes.triggerevent, node);

            this.AnalyticsLinkAttributes[Attributes.anchor] = XmlUtil.GetAttribute(Attributes.anchor, node);
            this.AnalyticsLinkAttributes[Attributes._class] = XmlUtil.GetAttribute(Attributes._class, node);
            this.AnalyticsLinkAttributes[Attributes.id] = XmlUtil.GetAttribute(Attributes.id, node);
            this.AnalyticsLinkAttributes[Attributes.linktype] = XmlUtil.GetAttribute(Attributes.linktype, node);
            this.AnalyticsLinkAttributes[Attributes.querystring] = XmlUtil.GetAttribute(Attributes.querystring, node);
            this.AnalyticsLinkAttributes[Attributes.target] = XmlUtil.GetAttribute(Attributes.target, node);
            this.AnalyticsLinkAttributes[Attributes.text] = XmlUtil.GetAttribute(Attributes.text, node);
            this.AnalyticsLinkAttributes[Attributes.title] = XmlUtil.GetAttribute(Attributes.title, node);
            this.AnalyticsLinkAttributes[Attributes.url] = XmlUtil.GetAttribute(Attributes.url, node);
        }

        private string CurrentMode
        {
            get
            {
                string serverProperty = this.ServerProperties["current_mode"] as string;
                if (!string.IsNullOrEmpty(serverProperty))
                    return serverProperty;
                return "internal";
            }
            set
            {
                Assert.ArgumentNotNull((object)value, nameof(value));
                this.ServerProperties["current_mode"] = (object)value;
            }
        }

        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull((object)message, nameof(message));
            if (this.CurrentMode != "media")
            {
                base.HandleMessage(message);
            }
            else
            {
                Item obj = (Item)null;
                if (message.Arguments.Count > 0 && ID.IsID(message.Arguments["id"]))
                {
                    IDataView dataView = this.MediaLinkTreeview.GetDataView();
                    if (dataView != null)
                        obj = dataView.GetItem(message.Arguments["id"]);
                }
                if (obj == null)
                    obj = this.MediaLinkTreeview.GetSelectionItem();
                Dispatcher.Dispatch(message, obj);
            }
        }

        protected void OnListboxChanged()
        {
            if (this.Target.Value == "Custom")
            {
                this.CustomTarget.Disabled = false;
                this.Custom.Class = string.Empty;
            }
            else
            {
                this.CustomTarget.Value = string.Empty;
                this.CustomTarget.Disabled = true;
                this.Custom.Class = "disabled";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull((object)e, nameof(e));
            base.OnLoad(e);
            if (Sitecore.Context.ClientPage.IsEvent)
                return;

            if (!Sitecore.Context.ClientPage.IsPostBack)
            {
                (new Events.Processors.LoadControl()).Reload();
            }

            this.CurrentMode = this.LinkType ?? string.Empty;
            this.InitControls();
            this.SetModeSpecificControls();
            GeneralLinkForm.RegisterScripts();
        }

        protected void OnMediaOpen()
        {
            Item selectionItem = this.MediaLinkTreeview.GetSelectionItem();
            if (selectionItem == null || !selectionItem.HasChildren)
                return;
            this.MediaLinkDataContext.SetFolder(selectionItem.Uri);
        }

        protected void OnModeChange(string mode)
        {
            Assert.ArgumentNotNull((object)mode, nameof(mode));
            this.CurrentMode = mode;
            this.SetModeSpecificControls();
            if (Sitecore.UIUtil.IsIE())
                return;
            SheerResponse.Eval("scForm.browser.initializeFixsizeElements();");
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, nameof(sender));
            Assert.ArgumentNotNull((object)args, nameof(args));
            Packet packet = new Packet("link", (string[])Array.Empty<string>());
            this.SetCommonAttributes(packet);
            string currentMode = this.CurrentMode;
            bool flag;
            if (!(currentMode == "internal"))
            {
                if (!(currentMode == "media"))
                {
                    if (!(currentMode == "external"))
                    {
                        if (!(currentMode == "mailto"))
                        {
                            if (!(currentMode == "anchor"))
                            {
                                if (!(currentMode == "javascript"))
                                    throw new ArgumentException("Unsupported mode: " + this.CurrentMode);
                                flag = this.SetJavascriptLinkAttributes(packet);
                            }
                            else
                                flag = this.SetAnchorLinkAttributes(packet);
                        }
                        else
                            flag = this.SetMailToLinkAttributes(packet);
                    }
                    else
                        flag = this.SetExternalLinkAttributes(packet);
                }
                else
                    flag = this.SetMediaLinkAttributes(packet);
            }
            else
                flag = this.SetInternalLinkAttributes(packet);
            if (!flag)
                return;
            SheerResponse.SetDialogValue(packet.OuterXml);
            base.OnOK(sender, args);
        }

        protected void SelectMediaTreeNode()
        {
            Item selectionItem = this.MediaLinkTreeview.GetSelectionItem();
            if (selectionItem == null)
                return;
            this.UpdateMediaPreview(selectionItem);
        }

        protected void UploadImage()
        {
            Item selectionItem = this.MediaLinkTreeview.GetSelectionItem();
            if (selectionItem == null)
                return;
            if (!selectionItem.Access.CanCreate())
                SheerResponse.Alert("You do not have permission to create a new item here.", (string[])Array.Empty<string>());
            else
                Sitecore.Context.ClientPage.SendMessage((object)this, "media:upload(edit=1,load=1)");
        }

        private static void HideContainingRow(Sitecore.Web.UI.HtmlControls.Control control)
        {
            Assert.ArgumentNotNull((object)control, nameof(control));
            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                GridPanel parent = control.Parent as GridPanel;
                if (parent == null)
                    return;
                parent.SetExtensibleProperty((System.Web.UI.Control)control, "row.style", "display:none");
            }
            else
                SheerResponse.SetStyle(control.ID + "Row", "display", "none");
        }

        private static void ShowContainingRow(Sitecore.Web.UI.HtmlControls.Control control)
        {
            Assert.ArgumentNotNull((object)control, nameof(control));
            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                GridPanel parent = control.Parent as GridPanel;
                if (parent == null)
                    return;
                parent.SetExtensibleProperty((System.Web.UI.Control)control, "row.style", string.Empty);
            }
            else
                SheerResponse.SetStyle(control.ID + "Row", "display", string.Empty);
        }

        private void InitControls()
        {
            string goalValue = this.AnalyticsLinkAttributes[Attributes.goalid];
            string triggerGoalValue = this.AnalyticsLinkAttributes[Attributes.triggergoal];
            string eventValue = this.AnalyticsLinkAttributes[Attributes.eventid];
            string triggerPageEventValue = this.AnalyticsLinkAttributes[Attributes.triggerevent];
            if (!string.IsNullOrWhiteSpace(goalValue))
            {
                this.GoalId.Value = goalValue;
                this.TriggerGoal.Value = triggerGoalValue;
            }
            if (!string.IsNullOrWhiteSpace(eventValue))
            {
                this.EventId.Value = eventValue;
                this.TriggerEvent.Value = triggerPageEventValue;
            }

            string str = string.Empty;
            string linkAttribute = this.AnalyticsLinkAttributes[Attributes.target];
            string linkTargetValue = LinkForm.GetLinkTargetValue(linkAttribute);
            if (linkTargetValue == "Custom")
            {
                str = linkAttribute;
                this.CustomTarget.Disabled = false;
                this.Custom.Class = string.Empty;
            }
            else
            {
                this.CustomTarget.Disabled = true;
                this.Custom.Class = "disabled";
            }
            this.Text.Value = this.AnalyticsLinkAttributes[Attributes.text];
            this.Target.Value = linkTargetValue;
            this.CustomTarget.Value = str;
            this.Class.Value = this.AnalyticsLinkAttributes[Attributes._class];
            this.Querystring.Value = this.AnalyticsLinkAttributes[Attributes.querystring];
            this.Title.Value = this.AnalyticsLinkAttributes[Attributes.title];
            this.InitMediaLinkDataContext();
            this.InitInternalLinkDataContext();
        }

        private void InitInternalLinkDataContext()
        {
            this.InternalLinkDataContext.GetFromQueryString();
            string queryString = WebUtil.GetQueryString("ro");
            string linkAttribute = this.AnalyticsLinkAttributes[Attributes.id];
            if (!string.IsNullOrEmpty(linkAttribute) && ID.IsID(linkAttribute))
                this.InternalLinkDataContext.SetFolder(new ItemUri(new ID(linkAttribute), Sitecore.Configuration.Factory.GetDatabase("master")));
            if (queryString.Length <= 0)
                return;
            this.InternalLinkDataContext.Root = queryString;
        }

        private void InitMediaLinkDataContext()
        {
            this.MediaLinkDataContext.GetFromQueryString();
            string str = string.IsNullOrEmpty(this.AnalyticsLinkAttributes[Attributes.url]) ? this.AnalyticsLinkAttributes[Attributes.id] : this.AnalyticsLinkAttributes[Attributes.url];
            if (this.CurrentMode != "media")
                str = string.Empty;
            if (str.Length == 0)
            {
                str = "/sitecore/media library";
            }
            else
            {
                if (!ID.IsID(str) && !str.StartsWith("/sitecore", StringComparison.InvariantCulture) && !str.StartsWith("/{11111111-1111-1111-1111-111111111111}", StringComparison.InvariantCulture))
                    str = "/sitecore/media library" + str;
                IDataView dataView = this.MediaLinkTreeview.GetDataView();
                if (dataView == null)
                    return;
                Item obj = dataView.GetItem(str);
                if (obj != null && obj.Parent != null)
                    this.MediaLinkDataContext.SetFolder(obj.Uri);
            }
            this.MediaLinkDataContext.AddSelected(new DataUri(str));
            this.MediaLinkDataContext.Root = "/sitecore/media library";
        }

        private static void RegisterScripts()
        {
            Sitecore.Context.ClientPage.ClientScript.RegisterClientScriptBlock(Sitecore.Context.ClientPage.GetType(), "translationsScript", "window.Texts = {{ ErrorOcurred: \"{0}\"}};".FormatWith((object)Translate.Text("An error occured:")), true);
        }

        private bool SetAnchorLinkAttributes(Packet packet)
        {
            Assert.ArgumentNotNull((object)packet, nameof(packet));
            string str = this.LinkAnchor.Value;
            if (str.Length > 0 && str.StartsWith("#", StringComparison.InvariantCulture))
                str = str.Substring(1);
            LinkForm.SetAttribute(packet, Attributes.url, str);
            LinkForm.SetAttribute(packet, Attributes.anchor, str);

            this.TrimComboboxControl(this.GoalId);
            LinkForm.SetAttribute(packet, Attributes.triggergoal, this.TriggerGoal);
            LinkForm.SetAttribute(packet, Attributes.goalid, this.GoalId);

            this.TrimComboboxControl(this.EventId);
            LinkForm.SetAttribute(packet, Attributes.triggerevent, this.TriggerEvent);
            LinkForm.SetAttribute(packet, Attributes.eventid, this.EventId);
            return true;
        }

        private void SetAnchorLinkControls()
        {
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.LinkAnchor);
            string str = this.AnalyticsLinkAttributes[Attributes.anchor];
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.GoalId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.EventId);
            if (this.LinkType != Attributes.anchor && string.IsNullOrEmpty(this.LinkAnchor.Value))
                str = string.Empty;
            if (!string.IsNullOrEmpty(str) && !str.StartsWith("#", StringComparison.InvariantCulture))
                str = "#" + str;
            this.LinkAnchor.Value = str ?? string.Empty;
            this.SectionHeader.Text = Translate.Text("Specify the name of the anchor, e.g. #header1, and any additional properties");
        }

        private void SetCommonAttributes(Packet packet)
        {
            Assert.ArgumentNotNull((object)packet, nameof(packet));
            LinkForm.SetAttribute(packet, Attributes.linktype, this.CurrentMode);
            LinkForm.SetAttribute(packet, Attributes.text, (Sitecore.Web.UI.HtmlControls.Control)this.Text);
            LinkForm.SetAttribute(packet, Attributes.title, (Sitecore.Web.UI.HtmlControls.Control)this.Title);
            LinkForm.SetAttribute(packet, Attributes._class, (Sitecore.Web.UI.HtmlControls.Control)this.Class);
        }

        private bool SetExternalLinkAttributes(Packet packet)
        {
            Assert.ArgumentNotNull((object)packet, nameof(packet));
            string str = this.Url.Value;
            if (str.Length > 0 && str.IndexOf("://", StringComparison.InvariantCulture) < 0 && !str.StartsWith("/", StringComparison.InvariantCulture))
                str = "http://" + str;
            string attributeFromValue = LinkForm.GetLinkTargetAttributeFromValue(this.Target.Value, this.CustomTarget.Value);
            LinkForm.SetAttribute(packet, Attributes.url, str);
            LinkForm.SetAttribute(packet, Attributes.anchor, string.Empty);
            LinkForm.SetAttribute(packet, Attributes.target, attributeFromValue);

            this.TrimComboboxControl(this.GoalId);
            LinkForm.SetAttribute(packet, Attributes.goalid, this.GoalId);

            this.TrimComboboxControl(this.EventId);
            LinkForm.SetAttribute(packet, Attributes.eventid, this.EventId);

            return true;
        }

        private void SetExternalLinkControls()
        {
            if (this.LinkType == "external" && string.IsNullOrEmpty(this.Url.Value))
                this.Url.Value = this.AnalyticsLinkAttributes[Attributes.url];
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.UrlContainer);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.Target);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.CustomTarget);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.GoalId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.EventId);
            this.SectionHeader.Text = Translate.Text("Specify the URL, e.g. http://www.sitecore.net and any additional properties.");
        }

        private bool SetInternalLinkAttributes(Packet packet)
        {
            Assert.ArgumentNotNull((object)packet, nameof(packet));
            Item selectionItem = this.InternalLinkTreeview.GetSelectionItem();
            if (selectionItem == null)
            {
                Sitecore.Context.ClientPage.ClientResponse.Alert("Select an item.");
                return false;
            }
            string attributeFromValue = LinkForm.GetLinkTargetAttributeFromValue(this.Target.Value, this.CustomTarget.Value);
            string str = this.Querystring.Value;
            if (str.StartsWith("?", StringComparison.InvariantCulture))
                str = str.Substring(1);
            LinkForm.SetAttribute(packet, Attributes.anchor, (Sitecore.Web.UI.HtmlControls.Control)this.LinkAnchor);
            LinkForm.SetAttribute(packet, Attributes.querystring, str);
            LinkForm.SetAttribute(packet, Attributes.target, attributeFromValue);
            LinkForm.SetAttribute(packet, Attributes.id, selectionItem.ID.ToString());


            this.TrimComboboxControl(this.GoalId);
            LinkForm.SetAttribute(packet, Attributes.goalid, this.GoalId);

            this.TrimComboboxControl(this.EventId);
            LinkForm.SetAttribute(packet, Attributes.eventid, this.EventId);

            return true;
        }

        private void SetInternalLinkContols()
        {
            this.LinkAnchor.Value = this.AnalyticsLinkAttributes[Attributes.anchor];
            this.InternalLinkTreeviewContainer.Visible = true;
            this.MediaLinkTreeviewContainer.Visible = false;
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.TreeviewContainer);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.Querystring);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.GoalId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.EventId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.LinkAnchor);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.Target);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.CustomTarget);
            this.SectionHeader.Text = Translate.Text("Select the item that you want to create a link to and specify the appropriate properties.");
        }

        private void SetJavaScriptLinkControls()
        {
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.JavascriptCode);
            string str = this.AnalyticsLinkAttributes[Attributes.url];
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.GoalId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.EventId);
            if (this.LinkType != "javascript" && string.IsNullOrEmpty(this.JavascriptCode.Value))
                str = string.Empty;
            this.JavascriptCode.Value = str;
            this.SectionHeader.Text = Translate.Text("Specify the JavaScript and any additional properties.");
        }

        private bool SetJavascriptLinkAttributes(Packet packet)
        {
            Assert.ArgumentNotNull((object)packet, nameof(packet));
            string str = this.JavascriptCode.Value;
            if (str.Length > 0 && str.IndexOf("javascript:", StringComparison.InvariantCulture) < 0)
                str = "javascript:" + str;
            LinkForm.SetAttribute(packet, Attributes.url, str);
            LinkForm.SetAttribute(packet, Attributes.anchor, string.Empty);

            this.TrimComboboxControl(this.GoalId);
            LinkForm.SetAttribute(packet, Attributes.goalid, this.GoalId);

            this.TrimComboboxControl(this.EventId);
            LinkForm.SetAttribute(packet, Attributes.eventid, this.EventId);
            return true;
        }

        private void SetMailLinkControls()
        {
            if (this.LinkType == "mailto" && string.IsNullOrEmpty(this.Url.Value))
                this.MailToLink.Value = this.AnalyticsLinkAttributes[Attributes.url];
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.GoalId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.EventId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.MailToContainer);
            this.SectionHeader.Text = Translate.Text("Specify the email address and any additional properties. To send a test mail use the 'Send a test mail' button.");
        }

        private bool SetMailToLinkAttributes(Packet packet)
        {
            Assert.ArgumentNotNull((object)packet, nameof(packet));
            string str = this.MailToLink.Value;
            string email = Sitecore.StringUtil.GetLastPart(str, ':', str);
            if (!EmailUtility.IsValidEmailAddress(email))
            {
                SheerResponse.Alert("The e-mail address is invalid.", (string[])Array.Empty<string>());
                return false;
            }
            if (!string.IsNullOrEmpty(email))
                email = "mailto:" + email;
            LinkForm.SetAttribute(packet, Attributes.url, email ?? string.Empty);
            LinkForm.SetAttribute(packet, Attributes.anchor, string.Empty);

            this.TrimComboboxControl(this.GoalId);
            LinkForm.SetAttribute(packet, Attributes.goalid, this.GoalId);

            this.TrimComboboxControl(this.EventId);
            LinkForm.SetAttribute(packet, Attributes.eventid, this.EventId);
            return true;
        }

        private bool SetMediaLinkAttributes(Packet packet)
        {
            Assert.ArgumentNotNull((object)packet, nameof(packet));
            Item selectionItem = this.MediaLinkTreeview.GetSelectionItem();
            if (selectionItem == null)
            {
                Sitecore.Context.ClientPage.ClientResponse.Alert("Select a media item.");
                return false;
            }
            string attributeFromValue = LinkForm.GetLinkTargetAttributeFromValue(this.Target.Value, this.CustomTarget.Value);
            LinkForm.SetAttribute(packet, Attributes.target, attributeFromValue);
            LinkForm.SetAttribute(packet, Attributes.id, selectionItem.ID.ToString());

            this.TrimComboboxControl(this.GoalId);
            LinkForm.SetAttribute(packet, Attributes.goalid, this.GoalId);

            this.TrimComboboxControl(this.EventId);
            LinkForm.SetAttribute(packet, Attributes.eventid, this.EventId);
            return true;
        }

        private void SetMediaLinkControls()
        {
            this.InternalLinkTreeviewContainer.Visible = false;
            this.MediaLinkTreeviewContainer.Visible = true;
            this.MediaPreview.Visible = true;
            this.UploadMedia.Visible = true;
            Item folder = this.MediaLinkDataContext.GetFolder();
            if (folder != null)
                this.UpdateMediaPreview(folder);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.TreeviewContainer);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.Target);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.CustomTarget);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.GoalId);
            GeneralLinkForm.ShowContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.EventId);
            this.SectionHeader.Text = Translate.Text("Select an item from the media library and specify any additional properties.");
        }

        private void SetModeSpecificControls()
        {
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.TreeviewContainer);
            this.MediaPreview.Visible = false;
            this.UploadMedia.Visible = false;
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.UrlContainer);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.Querystring);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.GoalId);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.EventId);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.MailToContainer);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.LinkAnchor);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.JavascriptCode);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.Target);
            GeneralLinkForm.HideContainingRow((Sitecore.Web.UI.HtmlControls.Control)this.CustomTarget);
            string currentMode = this.CurrentMode;
            if (!(currentMode == "internal"))
            {
                if (!(currentMode == "media"))
                {
                    if (!(currentMode == "external"))
                    {
                        if (!(currentMode == "mailto"))
                        {
                            if (!(currentMode == "anchor"))
                            {
                                if (!(currentMode == "javascript"))
                                    throw new ArgumentException("Unsupported mode: " + this.CurrentMode);
                                this.SetJavaScriptLinkControls();
                            }
                            else
                                this.SetAnchorLinkControls();
                        }
                        else
                            this.SetMailLinkControls();
                    }
                    else
                        this.SetExternalLinkControls();
                }
                else
                    this.SetMediaLinkControls();
            }
            else
                this.SetInternalLinkContols();
            foreach (Border control in this.Modes.Controls)
            {
                if (control != null)
                    control.Class = control.ID.ToLowerInvariant() == this.CurrentMode ? "selected" : string.Empty;
            }
        }

        private void UpdateMediaPreview(Item item)
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            MediaUrlOptions thumbnailOptions = MediaUrlOptions.GetThumbnailOptions((MediaItem)item);
            thumbnailOptions.UseDefaultIcon = true;
            thumbnailOptions.Width = 96;
            thumbnailOptions.Height = 96;
            thumbnailOptions.Language = item.Language;
            thumbnailOptions.AllowStretch = false;
            this.MediaPreview.InnerHtml = "<img src=\"" + MediaManager.GetMediaUrl((MediaItem)item, thumbnailOptions) + "\" width=\"96px\" height=\"96px\" border=\"0\" alt=\"\" />";
        }

        protected virtual void TrimComboboxControl(Combobox control)
        {
            if (string.IsNullOrEmpty(control?.Value))
            {
                return;
            }

            control.Value = control.Value.Trim();
        }
              

        public List<Item> GetDefinitionItems(string path, string tempId)
        {
            var context = Sitecore.Configuration.Factory.GetDatabase("master");
            Item item = context.SelectSingleItem(path);
            List<Item> items = item.Axes.GetDescendants().Where(x => x.TemplateID.ToString() == tempId).ToList();
            return items;
        }

        private string GetWebRootPath(string assembly)
        {
            string assemblyLoc = Assembly.Load(assembly).CodeBase;

            if (!string.IsNullOrEmpty(assemblyLoc))
            {
                assemblyLoc = assemblyLoc.Replace("file:///", "");

                string webRootPath = assemblyLoc.Replace(SYMB2C.Foundation.LinkTracker.Data.Constants.LinkTrackerConstants.AssemblyLinkTrackerPath,
                    "");

                return webRootPath;
            }

            return string.Empty;
        }
    }
}