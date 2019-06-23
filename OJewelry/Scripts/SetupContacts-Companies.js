
$(function () {
    //console.log("Calling SetRows!");
    $("body").SetRows({
        tablePrefix: "Contacts",
        dataStructName: "clients",
        newRow: '<div class="form-group ContactsTableRowContainer">' +

            '<input name="State" class="ContactsState" type="hidden" value="Dirty" data-val-required="The State field is required." data-val="true">' +

            '<div class="row ContactsTableRowData">' +
            '<input name="Id" id="clients_0__Id" type="hidden" value="0" data-val-required="The Id field is required." data-val="true" data-val-number="The field Id must be a number.">' +
            '<input name="CompanyID" id="clients_0__CompanyID" type="hidden" value="" data-val="true" data-val-number="The field CompanyID must be a number.">' +
            '<div>' +
            '   <input name="Name" class="form-control col-md-3 text-box single-line requiredifnotremoved" id="clients_0__Name" type="text" value="" data-val-required="The Name field is required. " data-val-length-max="50" data-val-length="The field Name must be a string with a maximum length of 50. " data-val="true">' +
            '</div>' +
            '<div>' +
            '   <input name="JobTitle" class="form-control col-md-3 text-box single-line" id="clients_0__JobTitle" type="text" value="" data-val-length-max="50" data-val-length="The field Job Title must be a string with a maximum length of 50. " data-val="true">' +
            '</div>' +
            '<div>' +
            '   <input name="Phone" class="form-control col-md-3 text-box single-line" id="clients_0__Phone", onblur="FormatPhoneNumber(this)" type="tel" value="" data-val="true" data-val-regex-pattern="^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$" data-val-regex="Invalid Phone number" data-val-phone="The Phone field is not a valid phone number.">' +
            '</div>' +
            '<div>' +
            '   <input name="Email" class="form-control col-md-3 text-box single-line" id="clients_0__Email" type="email" value="" data-val-length-max="50" data-val-length="The field Email must be a string with a maximum length of 50." data-val="true">' +
            '</div>' +
            '</div>' +
            '</div>',
        valRow: '<div class="row ContactsTableRowValidation">' +
            '<span name="Name" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].Name"></span>' +
            '<span name="JobTitle" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].JobTitle"></span>' +
            '<span name="Phone" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].Phone"></span>' +
            '<span name="Email" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].Email"></span>' +
            '</div> ',
        formName: 'CompaniesForm'
    });
});
