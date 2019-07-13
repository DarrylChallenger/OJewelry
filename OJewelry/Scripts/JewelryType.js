function bUseLaborTableClicked() {
    if ($("#bUseLaborTable").is(":checked") === true) {
        if ($("#FinishingCost").val().length === 0) {
            $("#FinishingCost").val("0.00");
            $("#FinishingCost").blur();
        }
    }
    $("#FinishingCost").toggleClass("nocursor");
}

$(function () {
    $("#FinishingCost").focus(function () {
        if ($("#bUseLaborTable").is(":checked") === true) {
            $(this).blur();
            console.log('*');
        }
    });

});