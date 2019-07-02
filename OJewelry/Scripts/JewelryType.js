function bUseLaborTableClicked() {
    if ($("#bUseLaborTable").is(":checked") === true) {
        if ($("#FinishingCost").val().length === 0) {
            $("#FinishingCost").val("0.00");
            $("#FinishingCost").blur();
        }
        if ($("#PackagingCost").val().length === 0) {
            $("#PackagingCost").val("0.00");
            $("#PackagingCost").blur();
        }
    }
    $("#FinishingCost").toggleClass("nocursor");
    $("#PackagingCost").toggleClass("nocursor");
}

$(function () {
    $("#PackagingCost").focus(function () {
        if ($("#bUseLaborTable").is(":checked") === true) {
            $(this).blur();
            console.log('*');
        }
    });
    $("#FinishingCost").focus(function () {
        if ($("#bUseLaborTable").is(":checked") === true) {
            $(this).blur();
            console.log('*');
        }
    });

});