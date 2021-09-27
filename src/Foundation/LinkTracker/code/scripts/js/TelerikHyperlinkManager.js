Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.LinkManager = function (element) {
    Telerik.Web.UI.Widgets.LinkManager.initializeBase(this, [element]);
    this._clientParameters = null;
}

Telerik.Web.UI.Widgets.LinkManager.prototype = {
    initialize: function () {
        Telerik.Web.UI.Widgets.LinkManager.callBaseMethod(this, "initialize");
        this.setupChildren();
    },

    dispose: function () {
        $clearHandlers(this._linkTargetCombo);
        this._linkTargetCombo = null;
        if (this._existingAnchor)
            $clearHandlers(this._existingAnchor);
        this._existingAnchor = null;
        if (this._insertButton)
            $clearHandlers(this._insertButton);
        this._insertButton = null;
        if (this._cancelButton)
            $clearHandlers(this._cancelButton);
        this._cancelButton = null;
        Telerik.Web.UI.Widgets.LinkManager.callBaseMethod(this, "dispose");
    },

    clientInit: function (clientParameters) {
        this._clientParameters = clientParameters;
        var selectedIndex = this._clientParameters.selectedTabIndex;
        if (selectedIndex && selectedIndex >= 0) {
            this._tab.set_selectedIndex(selectedIndex);
        }
        //clean
        this._cleanInputBoxes();
        this._loadLinkArchor();
        //load data
        this._loadLinkProperties();
    },

    _cleanInputBoxes: function () {
        this._linkUrl.value = "";
        this._linkText.value = "";
        if (this._linkTargetCombo.options && this._linkTargetCombo.options.length > 0) {
            this._linkTargetCombo.options[0].selected = true;
        }
        this._linkTooltip.value = "";
        this._anchorName.value = "";
        this._emailAddress.value = "";
        this._emailLinkText.value = "";
        this._emailSubject.value = "";
        this._linkCssClass.set_value("");
        this._emailCssClass.set_value("");

        // XAContext customization
        if (this._overlaySource) {
            this._overlaySource.checked = false;
        }

        this._selectedGoal.text = "Select a Goal";
        this._goalValue.value = "";
        if (this._triggerGoal) {
            this._triggerGoal.checked = false;
        }
    },

    _loadLinkProperties: function () {
        var currentLink = this._clientParameters.get_value();
        var currentHref = currentLink.getAttribute("href", 2);
        var anchors = this._clientParameters.documentAnchors;


        if (this._clientParameters.showText) {
            this._linkText.value = currentLink.innerHTML;
            this._emailLinkText.value = currentLink.innerHTML;

            if (this._texTextBoxParentNode) this._texTextBoxParentNode.style.display = "";
            if (this._emailTextBoxParentNode) this._emailTextBoxParentNode.style.display = "";
        }
        else {
            if (this._texTextBoxParentNode) this._texTextBoxParentNode.style.display = "none";
            if (this._emailTextBoxParentNode) this._emailTextBoxParentNode.style.display = "none";
        }

        this._loadCssClasses(currentLink);
        this._loadAnalytics(currentLink);

        if (currentHref && currentHref.match(/^(mailto:)([^\?&]*)/ig)) // "email"
        {
            this._loadEmailAddressAndSubject();
            this._tab.set_selectedIndex(2);
            return;
        }

        if (currentLink.name && currentLink.name.trim() != "") // "anchor"
        {
            this._anchorName.value = currentLink.name;
            this._tab.set_selectedIndex(1);
            return;
        }

        var href = "http://"; //"link"

        if (currentLink.href) {
            href = currentHref;
        }

        this._linkUrl.value = href;
        this._loadLinkTarget();
        this._linkTooltip.value = currentLink.title;

        this._tab.set_selectedIndex(0);
    },

    _loadAnalytics: function (currentLink) {
        this._triggerGoal.checked = currentLink.getAttribute('trigger-goal') == "1";
        var ddlSelectedIndex = currentLink.getAttribute('goal-index') || -1;

        if (ddlSelectedIndex > 0) {
            //get option text and value based on index
            var selectedText = currentLink.getAttribute('goal');
            var selectedTextValue = currentLink.getAttribute('goalid');
            //then assign that to the text
            this._selectedGoal.options[this._selectedGoal.selectedIndex].text = selectedText;
            //also assign the goal id
            this._selectedGoal.options[this._selectedGoal.selectedIndex].value = selectedTextValue;
        }

        this._goalValue.value = currentLink.getAttribute('goal-value') || '';
    },

    _loadLinkTarget: function () {
        var linkTarget = this._clientParameters.get_value().target;

        if (!linkTarget) {
            return;
        }

        var optgroups = this._linkTargetCombo.getElementsByTagName("optgroup");

        for (var i = 0; i < optgroups.length; i++) {
            if (optgroups[i].nodeName.toLowerCase() == "optgroup") {
                var options = optgroups[i].getElementsByTagName("option");

                for (var j = 0; j < options.length; j++) {
                    if (options[j].nodeName.toLowerCase() == "option" &&
                        options[j].value.toLowerCase() == linkTarget.toLowerCase()) {
                        options[j].selected = true;
                        return;
                    }
                }
            }
        }

        var customOption = document.createElement("option");
        customOption.value = linkTarget;
        customOption.text = linkTarget;
        this._linkTargetCombo.options.add(customOption);
        customOption.selected = true;
    },

    _loadLinkArchor: function () {
        if (!this._existingAnchor) {
            return;
        }
        var anchors = this._clientParameters.documentAnchors;
        var linkHref = this._clientParameters.get_value().getAttribute("href", 2) ? this._clientParameters.get_value().getAttribute("href", 2).toLowerCase() : "";
        //clear existing options
        this._existingAnchor.innerHTML = "";
        this._existingAnchor.options.add(new Option(localization["None"], ""));
        this._existingAnchor.options[0].selected = true;

        for (var i = 0; i < anchors.length; i++) {
            var anchorOption = new Option(anchors[i].name, "#" + anchors[i].name);
            this._existingAnchor.options.add(anchorOption);

            if ("#" + anchors[i].name.toLowerCase() == linkHref) {
                anchorOption.selected = true;
            }
        }
    },

    _loadCssClasses: function (currentLink) {
        var cssClasses = this._clientParameters.CssClasses;
        //localization
        this._linkCssClass.set_showText(true);
        this._linkCssClass.set_clearclasstext(localization["ClearClass"]);
        this._linkCssClass.set_text(localization["ApplyClass"]);
        this._linkCssClass.set_value("");
        this._emailCssClass.set_showText(true);
        this._emailCssClass.set_clearclasstext(localization["ClearClass"]);
        this._emailCssClass.set_text(localization["ApplyClass"]);
        this._emailCssClass.set_value("");

        //Copy the css classes to avoid one collection being modified by the other dropdown
        this._linkCssClass.set_items(cssClasses.concat([]));
        this._emailCssClass.set_items(cssClasses);

        if (currentLink.className !== null && currentLink.className !== "") {
            // XAContext customization
            if (this._overlaySource && currentLink.className.indexOf('overlay-source') !== -1) {
                currentLink.className = currentLink.className.replace('overlay-source', '').replace(/^\s+|\s+$/g, '');
                this._overlaySource.checked = true;
            }

            this._linkCssClass.updateValue(currentLink.className);
            this._emailCssClass.updateValue(currentLink.className);
        }
    },

    _loadEmailAddressAndSubject: function () {
        var currentHref = this._clientParameters.get_value().getAttribute("href", 2);
        this._emailAddress.value = RegExp.$2;

        if (currentHref.match(/(\?|&)subject=([^\b]*)/ig)) {
            var val = RegExp.$2.replace(/&amp;/gi, "&");
            val = usymb2ccape(val);
            this._emailSubject.value = val;
        }
    },
    getSelectedTabValue: function () {
        var selectedTabValue = this._tab.get_selectedTab().get_value();

        if (selectedTabValue == "AnalyticsEventTab") {
            var hasHyperlinkValue = Boolean(this._linkUrl.value);
            if (hasHyperlinkValue) {
                var hyperlinkValueTrimmed = this._linkUrl.value.trim();
                hasHyperlinkValue = hyperlinkValueTrimmed.length > 0 && hyperlinkValueTrimmed != "http://" && hyperlinkValueTrimmed != "https://";
            }
            if (hasHyperlinkValue) {
                selectedTabValue = "HyperlinkTab";
            } else if (this._anchorName.value && this._anchorName.value.trim().length > 0) {
                selectedTabValue = "AnchorTab";
            } else if (this._emailAddress.value && this._emailAddress.value.trim().length > 0) {
                selectedTabValue = "EmailTab";
            } else {
                selectedTabValue = "HyperlinkTab";
            }
        }

        return selectedTabValue;
    },
    addAnalyticsToLink: function (resultLink) {
        if (this._triggerGoal.checked) {
            var attr_chk = document.createAttribute("trigger-goal");
            attr_chk.value = "1";
            resultLink.setAttributeNode(attr_chk);

            if (this._selectedGoal.options[this._selectedGoal.selectedIndex].text !== "Select a Goal") {
                var attr_goal = document.createAttribute("goal");
                attr_goal.value = this._selectedGoal.options[this._selectedGoal.selectedIndex].text;
                resultLink.setAttributeNode(attr_goal);

                var attr_goal_id = document.createAttribute("goalid");
                attr_goal_id.value = this._selectedGoal.options[this._selectedGoal.selectedIndex].value;
                resultLink.setAttributeNode(attr_goal_id);

                var attr_goal_index = document.createAttribute("goal-index");
                attr_goal_index.value = this._selectedGoal.selectedIndex;
                resultLink.setAttributeNode(attr_goal_index);

                if (this._goalValue.value.trim() !== "") {
                    var attr_val = document.createAttribute("goal-value");
                    attr_val.value = this._goalValue.value;
                    resultLink.setAttributeNode(attr_val);
                }

                //adding id
                resultLink.classList.add("rte-external-link");
            }
        } else {
            resultLink.removeAttribute("trigger-goal", 0);
            resultLink.removeAttribute("goal", 0);
        }
        return resultLink;
    },
    getModifiedLink: function () {
        var resultLink = this._clientParameters.get_value();
        //var selectedTabValue = this._tab.get_selectedTab().get_value();
        var selectedTabValue = this.getSelectedTabValue();

        if (selectedTabValue == "HyperlinkTab") //"link"
        {
            resultLink.href = this._linkUrl.value;
            if (this._linkTargetCombo.value == "_none") {
                resultLink.removeAttribute("target", 0);
            } else {
                resultLink.target = this._linkTargetCombo.value;
            }

            if (this._texTextBoxParentNode && this._texTextBoxParentNode.style.display != "none") {
                resultLink.innerHTML = this._linkText.value;
            }

            if (resultLink.innerHTML.trim() == "" || resultLink.innerHTML.trim().length < this._linkText.value.trim().length) {
                resultLink.innerHTML = this._linkText.value.replace(/&/gi, "&amp;").replace(/</gi, "&lt;").replace(/>/gi, "&gt;");
            }

            if (resultLink.innerHTML.trim() == "") {
                resultLink.innerHTML = resultLink.href;
            }

            if (this._linkTooltip.value.trim() == "") {
                resultLink.removeAttribute("title", 0);
            }
            else {
                resultLink.title = this._linkTooltip.value;
            }

            // XAContext customization
            resultLink.removeAttribute("name", 0);
            var linkCssClass = this._linkCssClass;
            if (this._overlaySource && this._overlaySource.checked) {
                if (linkCssClass) {
                    linkCssClass += " overlay-source";
                } else {
                    linkCssClass = "overlay-source";
                }
            }

            this._setClass(resultLink, linkCssClass);

        }
        else if (selectedTabValue == "AnchorTab") //"anchor"
        {
            resultLink.removeAttribute("name");
            resultLink.removeAttribute("NAME");
            resultLink.name = null;
            resultLink.name = this._anchorName.value;
            resultLink["NAME"] = this._anchorName.value;

            //Make sure the href and some other attributes are removed just in case they are present
            resultLink.removeAttribute("href");
            resultLink.removeAttribute("target");
            resultLink.removeAttribute("title");
        }
        else if (selectedTabValue == "EmailTab" && this._emailAddress.value.trim().length > 0)//"email"
        {
            resultLink.href = "mailto:" + this._emailAddress.value;

            if (this._emailSubject.value != "") {
                resultLink.href += "?subject=" + this._emailSubject.value;
            }

            if (this._emailTextBoxParentNode && this._emailTextBoxParentNode.style.display != "none") {
                resultLink.innerHTML = this._emailLinkText.value;
            }

            this._setClass(resultLink, this._emailCssClass);
        }

        //Goal Analytics
        resultLink = this.addAnalyticsToLink(resultLink);

        return resultLink;
    },

    _setClass: function (element, cssClassHolder) {
        if (cssClassHolder.get_value() == "") {
            $telerik.$(element).removeClass();
        }
        else {
            element.className = cssClassHolder.get_value();
        }
    },

    setupChildren: function () {
        this._linkUrl = $get("LinkURL");
        if (this._linkUrl == null) {
            this._linkUrl = {};
            this._linkUrl.value = "";
        }
        this._linkText = $get("LinkText");

        this._linkTargetCombo = $get("LinkTargetCombo");
        this._setLinkTargetLocalization();
        this._existingAnchor = $get("ExistingAnchor");
        this._linkTooltip = $get("LinkTooltip");

        //NEW: Document manager support
        this._documentManager = $find("DocumentManagerCaller");
        this._insertInternalLink = $find("InsertInternalLink");

        this._linkCssClass = $find("LinkCssClass");

        this._anchorName = $get("AnchorName");

        this._emailAddress = $get("EmailAddress");
        this._emailLinkText = $get("EmailLinkText");
        this._emailSubject = $get("EmailSubject");
        this._emailCssClass = $find("EmailCssClass");

        this._insertButton = $get("lmInsertButton");
        if (this._insertButton)
            this._insertButton.title = localization["OK"];
        this._cancelButton = $get("lmCancelButton");
        if (this._cancelButton)
            this._cancelButton.title = localization["Cancel"];
        this._tab = $find("LinkManagerTab");

        //NEW: In IE RadFormDecorator styles textboxes in a way that the direct parent [of the textbox] changes, so the original implementation stopped working properly (in IE)
        this._texTextBoxParentNode = $get("texTextBoxParentNode");
        this._emailTextBoxParentNode = $get("emailTextBoxParentNode");

        // XAContext customization
        this._overlaySource = $get("OverlaySource");

        //Selected Goal
        this._triggerGoal = $get("CheckTriggerGoal");
        this._selectedGoal = $get("ApplyGoalDropDown");
        this._goalValue = $get("GoalDataValue");

        this._initializeChildEvents();
    },

    _initializeChildEvents: function () {
        this._linkCssClass.add_valueSelected(this._cssValueSelected);
        this._emailCssClass.add_valueSelected(this._cssValueSelected);
        //NEW: Document manager
        if (this._documentManager) {
            this._documentManager.add_valueSelected(Function.createDelegate(this, this._documentManagerClicked));
        }
        this._insertInternalLink && this._insertInternalLink.add_valueSelected(Function.createDelegate(this, this._insertInternalLinkClicked));


        $addHandlers(document, { "keydown": this._keyDownHandler }, this); //NEW add ENTER click handler
        $addHandlers(this._linkTargetCombo, { "change": this._linkTargetChangeHandler }, this);
        if (this._existingAnchor)
            $addHandlers(this._existingAnchor, { "change": this._existingAnchorChangeHandler }, this);
        if (this._insertButton)
            $addHandlers(this._insertButton, { "click": this._insertClickHandler }, this);
        if (this._cancelButton)
            $addHandlers(this._cancelButton, { "click": this._cancelClickHandler }, this);
    },
    _insertInternalLinkClicked: function (oTool, args) {
        var editor = this._clientParameters.editor,
            id = window.parent.scFormatId &&
                window.parent.scItemID &&
                window.parent.scFormatId(window.parent.scItemID),
            scDatabase = window.parent.scDatabase,
            scLanguage = window.parent.scLanguage;
        if (!editor || !scLanguage || !id) {
            return;
        }
        editor.showExternalDialog(
            "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertLink&la=" +
            window.parent.scLanguage +
            "&fo=" +
            id +
            (scDatabase ? "&databasename=" + scDatabase : ""),
            null,
            1100,
            700,
            handleOnValueCreated.bind(this),
            null,
            "Insert Link",
            true,
            window.parent.Telerik.Web.UI.WindowBehaviors.Close,
            false,
            false
        );

        function handleOnValueCreated(sender, returnValue) {
            if (returnValue && returnValue.url) {
                this._linkUrl.value = returnValue.url;
            }
        }
    },
    //NEW: Document manager
    _documentManagerClicked: function (oTool, args) {
        //Editor object is supplied to all dialogs in the dialog parameters
        var editor = this._clientParameters.editor;
        var callbackFunction = Function.createDelegate(this, function (sender, args) {
            //For the time being just set the URL
            //Returned link - TODO: Use args.get_value() when the dialog returm methods are changed to return proper args object
            var link = args.get_value ? args.get_value() : args.Result;
            if (link && link.tagName == "A") {
                //Set various fileds - classname, target, etc - but only if their value in the returned link is != ""
                var href = link.getAttribute("href", 2);
                this._linkUrl.value = href;
                if (!this._linkText.value) this._linkText.value = href;
                var target = link.target;
                if (target) this._linkTargetCombo.value = target;
                var title = link.title;
                if (title) this._linkTooltip.value = title;
                var className = link.className;
                if (className) this._linkCssClass.set_value(className);
            }
        });
        var modifiedLink = this.getModifiedLink();
        var argument = new Telerik.Web.UI.EditorCommandEventArgs("DocumentManager", null, modifiedLink);
        Telerik.Web.UI.Editor.CommandList._getDialogArguments(argument, "A", editor, "DocumentManager");
        editor.showDialog("DocumentManager", argument, callbackFunction);
    },

    _cssValueSelected: function (oTool, args) {
        if (!oTool) return;
        var commandName = oTool.get_name();

        if ("ApplyClass" == commandName) {
            var attribValue = oTool.get_selectedItem();
            oTool.updateValue(attribValue);
        }
    },

    _linkTargetChangeHandler: function (e) {
        if (this._linkTargetCombo.value == "_custom") {
            var targetprompttext = "Type Custom Target Here";
            var targetprompt = prompt(targetprompttext, "CustomWindow");

            if (targetprompt) {
                var newoption = document.createElement("option"); // create new <option> node
                newoption.innerHTML = targetprompt; // set innerHTML to the new <option> none
                newoption.setAttribute("selected", "selected"); // set the new <option> node selected="selected"
                newoption.setAttribute("value", targetprompt); // change the value of the new <option> node with the value of the prompt
                this._linkTargetCombo.getElementsByTagName("optgroup")[1].appendChild(newoption); // append the new <option> node to the <optgroup>
                return;
            }

            this._linkTargetCombo.selectedIndex = 0;
        }
    },

    _setLinkTargetLocalization: function () {
        var optgroups = this._linkTargetCombo.getElementsByTagName("optgroup");
        for (var i = 0; i < optgroups.length; i++) {
            var options = optgroups[i].getElementsByTagName("option");
            var grpName = optgroups[i].label;
            if (localization[grpName])
                optgroups[i].label = localization[grpName];
            for (var j = 0; j < options.length; j++) {
                var optName = options[j].text;
                if (localization[optName])
                    options[j].text = localization[optName];
            }
        }
    },

    _existingAnchorChangeHandler: function (e) {
        if (this._existingAnchor.selectedIndex != 0) {
            this._linkUrl.value = this._existingAnchor.value;
        }
    },

    _insertClickHandler: function (e) {
        var modifiedLink = this.getModifiedLink();
        var args = new Telerik.Web.UI.EditorCommandEventArgs("LinkManager", null, modifiedLink);
        //backwards compatibility
        args.realLink = modifiedLink;
        Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close(args);
    },

    _cancelClickHandler: function (e) {
        Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
    },

    _keyDownHandler: function (e) {
        if (e.keyCode == 13)
            this._insertClickHandler(null);
        else if (e.keyCode == 27)
            this._cancelClickHandler(e);
    }
}

Telerik.Web.UI.Widgets.LinkManager.registerClass("Telerik.Web.UI.Widgets.LinkManager", Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);
