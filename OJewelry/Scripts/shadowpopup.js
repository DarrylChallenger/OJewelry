$(window).load(function () {
    $(".trigger_ojPopup").click(function () {
        ojPopupDraw();
        $('.ojPopup').show();
    });
    $('.ojPopup').click(function () {
        $('.ojPopup').hide();
    });
    $('.popupCloseButton').click(function () {
        $('.ojPopup').hide();
    });
});