define(["sitecore"], function(Sitecore) {
    var InsertEmailDialog = Sitecore.Definitions.App.extend({
        initialized: function () {

            this.updateOkButton();

            this.DisplayedText.on("change", function () {
                this.updateOkButton();
            }, this);

        },

        updateOkButton: function () {

            var text = this.DisplayedText.get("text");

            if (text)
                this.InsertEmailLink.set("isEnabled", true);
            else
                this.InsertEmailLink.set("isEnabled", false);
        }

    });

    return InsertEmailDialog;
});