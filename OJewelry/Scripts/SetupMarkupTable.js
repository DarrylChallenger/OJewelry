
$(function () {
    //console.log("Calling SetRows!");
    $("body").SetRows({
        tablePrefix: "Markups",
        dataStructName: "markups",
        newRow:
            `<div class="MarkupsTableRowContainer">` +
            '<div class="TableRowContainer ltbordered">' +
            `<input name="State" class="MarkupsState" type="hidden" value="Dirty" data-val-required="The State field is required." data-val="true">` +
            `<div class="row MarkupsTableRowData">` +
            '<input name="Id" id="Markups_0__Id" type="hidden" value="0" data-val-required="The Id field is required." data-val="true" data-val-number="The field Id must be a number.">' +
            '<input name="CompanyID" id="Markups_0__CompanyID" type="hidden" value="0" data-val="true" data-val-number="The field CompanyID must be a number.">' +
            '<div>' +
            `   <input name="title" class="requiredifnotremoved col-md-3 text-box single-line" id="title" type="text" data-val-required="Please enter a Title. " data-val="true" value="">` +
            '</div>' +
            '<div>' +
            '   <input name="multiplier" class="requiredifnotremoved col-md-2 text-box single-line" id="multiplier" type="text" value="0" data-val-required="The Multiplier (%) field is required." data-val-number="The field Multiplier must be a number." data-val="true">' +
            '</div>' +
            '<div>' +
            '   <input name="ratio" class="requiredifnotremoved col-md-2 text-box single-line" id="ratio" type="text" value="0" data-val-required="The Markup (%) field is required." data-val-number="The field Markup must be a number." data-val="true">' +
            '</div>' +
            '<div>' +
            '   <input name="margin" class="requiredifnotremoved col-md-2 text-box single-line" id="margin" type="text" value="0" data-val-required="The Margin (%) field is required." data-val-number="The field Margin must be a number." data-val="true">' +
            '</div>' +
            '<div>' +
            '   <input name="Addend" class="requiredifnotremoved col-md-2 text-box single-line" id="Addend" type="text" value="0" data-val-required="The Addend ($) field is required." data-val-number="The field Addend must be a number." data-val="true">' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>',
        valRow: `<div class="row MarkupsTableRowValidation">` +
            '<span name="title" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="title"></span>' +
            '<span name="multiplier" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="multiplier"></span>' +
            '<span name="ratio" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for=ratio"></span>' +
            '<span name="margin" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for=ratio"></span>' +
            '<span name="Addend" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for=ratio"></span>' +
            '</div> ',
        formName: 'MarkupsForm'
    });
});
