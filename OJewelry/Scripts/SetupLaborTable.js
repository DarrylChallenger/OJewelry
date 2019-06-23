
$(function () {
    //console.log("Calling SetRows!");
    $("body").SetRows({
        tablePrefix: "Labors",
        dataStructName: "Labors",
        newRow:
            `<div class="row LaborsTableRowContainer">` +
                '<div class="TableRowContainer ltbordered">' +
                    `<input name="State" class="LaborsState" type="hidden" value="Dirty" data-val-required="The State field is required." data-val="true">` +
                    `<div class="row LaborsTableRowData">` +
                        '<input name="Id" id="Labors_0__Id" type="hidden" value="0" data-val-required="The Id field is required." data-val="true" data-val-number="The field Id must be a number.">' +
                        '<input name="CompanyID" id="Labors_0__CompanyID" type="hidden" value="0" data-val="true" data-val-number="The field CompanyID must be a number.">' +
                        '<div>' +
                        '   <input name="Name" class="requiredifnotremoved col-md-3 text-box single-line valid" id="Labors_0__Name" type="text" value="" data-val-required="The Labor field is required. " data-val="true" data-val-length-max="100" data-val-length="The field Labor must be a string with a maximum length of 100. ">' +
                        '</div>' +
                        '<div>' +
                        '   <input name="pph" class="col-md-2 text-box single-line" id="Labors_0__pph" type="text" value="" data-val="true" data-val-number="The field $/PC must be a number. ">' +
                        '</div>' +
                        '<div>' +
                        '   <input name="ppp" class="col-md-2 text-box single-line" id="Labors_0__ppp" type="text" value="" data-val="true" data-val-number="The field $/HR must be a number. ">' +
                        '</div>' +
                        '<div>' +
                        `   <select name="VendorId" id="Labors_0__VendorId" data-val-required="The Vendor field is required. "data-val-number="The Vendor field is required. " data-val="true" class="col-md-3">
                                <option id="getoptions" value="LaborTableVendors"></option>
                            </select>` +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>',
        valRow: `<div class="row LaborsTableRowValidation">` +
            '<span name="Name" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="Labors[0].Name"></span>' +
            '<span name="pph" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="Labors[0].pph"></span>' +
            '<span name="ppp" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="Labors[0].ppp"></span>' +
            '<span name="VendorId" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="Labors[0].VendorId"></span>' +
            '</div> ',
        formName: 'LaborsForm'
    });
});
