define(["sitecore"], function(Sitecore) {
    var InsertAnchorDialog = Sitecore.Definitions.App.extend({
        initialized: function () {

            this.updateOkButton();

            this.Text.on("change", function () {
                this.updateOkButton();
            }, this);

        },

        updateOkButton: function () {

            var text = this.Text.get("text");

            if (text)
                this.InsetAnchor.set("isEnabled", true);
            else
                this.InsetAnchor.set("isEnabled", false);
        }

    });

    return InsertAnchorDialog;
});