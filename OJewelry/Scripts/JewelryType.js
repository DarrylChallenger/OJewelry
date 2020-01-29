function bUseLaborTableClicked() {
    if ($("#bUseLaborTable").is(":checked") === true) {
        $("#noLTIWarning").removeClass("hidden");
        console.log("true");
        if ($("#FinishingCost").val().length === 0) {
            $("#FinishingCost").val("0.00");
            $("#FinishingCost").blur();
            // If there are no LTIs show message indicating that LTI must be created before creating a style
        }
    } else {
        $("#noLTIWarning").addClass("hidden");
        console.log("false");
    }
    $("#FinishingCost").toggleClass("nocursor");
}

$(function () {
    $("#FinishingCost").focus(function () {
        if ($("#bUseLaborTable").is(":checked") === true) {
            $(this).blur();
        }
    });

});