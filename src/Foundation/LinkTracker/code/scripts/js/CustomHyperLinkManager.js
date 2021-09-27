$(function () {
    // write code here
    var analyticsEventField = $("#analyticsEventFieldset");

    if (analyticsEventField.length) {
        $.ajax({
            url: '/sc/api/LoadGoals/GetGoals',
            success: function (data) {
                var allGoals = $.parseJSON(data.Data);
                for (var i = 0; i < allGoals.length; i++) {
                    var opt = new Option(allGoals[i].GoalTypeName, allGoals[i].GoalId);
                    $("#ApplyGoalDropDown").append(opt);
                }
            },
            error: function (data) {
                alert("Could not retrieve the goals", data.errorText);
            }
        });
    }
})