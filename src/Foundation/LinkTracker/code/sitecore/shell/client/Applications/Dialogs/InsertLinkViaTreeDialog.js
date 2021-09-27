define(["sitecore"], function(Sitecore) {
  var InsertLinkViaTreeDialog = Sitecore.Definitions.App.extend({
    initialized: function () {

      this.updateCustomUrl();

      this.Target.on("change", function () {
        this.updateCustomUrl();
      }, this);

      // work around an issue in combobox
      this.TargetsDataSource.on("change:items", function () {
        if (this.__firstTime1) {
          this.__firstTime1 = false;
          var local = this;
          var targets = this.TargetLoadedValue.get("text").split(',');
          var targetWindowValue = {};
          targets.forEach(function (target) {
            if (targetWindowValue == undefined || targetWindowValue.$displayName == undefined) {
              local.Target.set("selectedValue", target);
            }
            targetWindowValue = local.Target.get("selectedItem");
          });
        }
      }, this);


    //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
        this.GoalsDataSource.on("change:items", function () {
            if (this.__firstTime2) {
                this.__firstTime2 = false;

                var local = this;
                var newArr = local.Goal.viewModel.items();
                newArr.unshift({ $displayName: '', itemId: '' });
                local.Goal.viewModel.rebind(newArr, null, null, '$displayName', 'itemId' )

                var goalInput = this.GoalLoadedValue.get("text");
                local.Goal.set("selectedValue", (!goalInput ? '' : goalInput) );
            }
        }, this);


        this.EventsDataSource.on("change:items", function () {
            if (this.__firstTime3) {
                this.__firstTime3 = false;

                var local = this;
                var newArr = local.Event.viewModel.items();
                newArr.unshift({ $displayName: '', itemId: '' });
                local.Event.viewModel.rebind( newArr, null, null, '$displayName', 'itemId' )

                var eventInput = this.EventLoadedValue.get("text");
                local.Event.set("selectedValue", (!eventInput ? "" : eventInput));
            }
        }, this);
    //////////////////////////////////////////////////////// END CUSTOM BIT ////////////////////////////////////////////////////////

    },
      
    updateCustomUrl: function () {

      var emptyOptionID = "{A3C9DB39-1D1B-4AA1-8C68-7B9674D055EE}";
      var customUrlOptionID = "{07CF2A84-9C22-4E85-8F3F-C301AADF5218}";

      var targetWindowValue = this.Target.get("selectedItem");

      if (!targetWindowValue || targetWindowValue.itemId === emptyOptionID) {
        this.CustomUrl.set("isEnabled", false);
        return;
      }

      if (targetWindowValue.itemId === customUrlOptionID) {
        this.CustomUrl.set("isEnabled", true);
      } else {
        this.CustomUrl.set("isEnabled", false);
      }
    },

    insertInternalLinkResult: function () {
      var targetDisplayTextID = this.TextDescription,
      targetPathID = this.TreeView,
      targetPathProperty = "$path",
      targetQueryID = this.QueryString,
      targetAltTextID = this.AltText,
      targetStyleID = this.StyleClass,
      targetWindowID = this.Target,
      customUrlID = this.CustomUrl,
      targetControlID = this.TreeView,
      anchor = this.AnchorText,
      //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
      goal = this.Goal,
      pageevent = this.Event,
      //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
      selectedItemsPropertyName = "selectedNode",
          template = '<link text="<%=displayText%>" anchor="<%=anchor%>" linktype="internal" class="<%=styleClass%>" title="<%=alternateText%>" <%=target%> querystring="<%=queryString%>" id="<%=itemId%>" goalid="<%=goal%>" eventid="<%=pageevent%>" />',
      targetWindowValue,
      path,
      emptyOptionID = "{A3C9DB39-1D1B-4AA1-8C68-7B9674D055EE}",
      htmlEncode = function (str) {
        return str.toString().replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
      };

      if (!targetDisplayTextID) {
        console.log("Provide at least display text for your link");
        return false;
      }

      targetWindowValue = targetWindowID.get("selectedItem");

      if (!targetWindowValue || targetWindowValue.itemId === emptyOptionID) {
        targetWindowValue = "";
      } else {
        var targetWindow = targetWindowValue.itemName;

        switch (targetWindow) {
          case 'Active Browser':
            targetWindow = "";
            break;
          case 'New Browser':
            targetWindow = "_blank";
            break;
          case "Custom":
            targetWindow = customUrlID.get("text") + "|Custom";
            break;
        }

        targetWindowValue = "target=\"" + targetWindow + "\"";
      }

      if (targetPathID.get(selectedItemsPropertyName) &&
        "rawItem" in targetPathID.get(selectedItemsPropertyName) &&
        targetPathID.get(selectedItemsPropertyName).rawItem[targetPathProperty]) {
        path = targetPathID.get(selectedItemsPropertyName).rawItem[targetPathProperty];
      }

      var itemLink = _.template(template, {
        displayText: htmlEncode(targetDisplayTextID.get("text")),
        alternateText: htmlEncode(targetAltTextID.get("text")),
        itemId: targetControlID.get("selectedItemId"),
        queryString: htmlEncode(targetQueryID.get("text")),
        target: targetWindowValue,
        styleClass: htmlEncode(targetStyleID.get("text")),
        path: path,
        anchor: htmlEncode(anchor.get("text")),
        //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////
        goal: goal.get("selectedItemId"),
        pageevent: pageevent.get("selectedItemId")
        //////////////////////////////////////////////////////// START CUSTOM BIT ////////////////////////////////////////////////////////

      });

      return this.closeDialog(itemLink);
    },

    __firstTime1: true,
    __firstTime2: true,
    __firstTime3: true

  });

  return InsertLinkViaTreeDialog;
});