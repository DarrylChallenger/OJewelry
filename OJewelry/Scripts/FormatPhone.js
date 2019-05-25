
function FormatPhoneNumber(ele) {
    const e = `#${ele.id}`;
    const v = $(e).val();
    //console.log(JSON.stringify(v));

    if (!($(e).hasClass("input-validation-error")))
    {
        //console.log("*");
        $(e).val(v.replace(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/, '$1-$2-$3'));
        //$(e).val(v.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3'));
    }
}