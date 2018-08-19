

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('.StyleImage')
                .attr('src', e.target.result)
                //.width("200px")
                //.height("16em");
               .width($(".StyleRightHeader.width"))
               .height($(".StyleRightHeader.height"));
        };
        reader.readAsDataURL(input.files[0]);
    };
}

$("#PostedImageFile").change(function () {
    readURL(this);
});