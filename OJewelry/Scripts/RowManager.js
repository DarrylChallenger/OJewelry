/* Row Manager */
/* 
 * Wrap header and each data row in a row with a column on either side
 * Add the +/- buttons on reach existing row as appropriate
 * Each button should have a handler to Add/Remove row functions
 * Options include header, body, and validation rows, table id, Right/Left indicator
 * Row state will change as rows are added or deleted. State is stored as data on the [options.]TableRow
 * 
 */

/// OnAdd
function AddRow(index) {
    console.log("AddRow", index);
    var px = sessionStorage.getItem("DCTS.tablePrefix");
    var ds = sessionStorage.getItem("DCTS." + px + ".dataStructName");
    var nr = sessionStorage.getItem("DCTS." + px + ".newRow");
    var vr = sessionStorage.getItem("DCTS." + px + ".valRow");

    var rowClass = "." + px + "TableRowData";
    var tableClass = px + "TableData";
    var tableRowClass = px + "TableRowContainer";
    var addBtnClass = px + "AddBtn";
    var delBtnClass = px + "DelBtn";
    var valRowContent = px + "TableRowValidation";

    // hide the '+' btn
    $("." + addBtnClass).addClass("hidden");

    // insert new row (find last row by class)
    if (index === -1)
    {
        // add after header
    } else {
        // add after last row
        $("#" + tableClass).last().append(nr);
        //$("#" + tableClass).last().append('<div class="row ContactsTableRowData">hello</div>');
    }
    
    // wrap it...
    var newRow = $(rowClass).last();
    newRow.after(vr);
    console.log(newRow);
    WrapRow(px, newRow);
    
    // Show the button
    $("." + addBtnClass).last().removeClass("hidden");
    // add id, onclick handler to btns
    $("." + addBtnClass).last().attr("id", addBtnClass + "_" + (index + 1)).attr("onclick", "AddRow(" + (index + 1) + ")");
    $("." + delBtnClass).last().attr("id", delBtnClass + "_" + (index + 1)).attr("onclick", "DelRow(" + (index + 1) + ")");
    // Add correct name/id to each child in row
    $.each(newRow.find(":input"), function (i, value) {
        name = value.getAttribute("name");
        value.setAttribute("name", ds + "[" + (index + 1) + "]." + name);
        value.setAttribute("id", ds + "_" + (index + 1) + "__" + name);
    });
    // add validation
    valRows = $("." + valRowContent).last().children();
    console.log(valRows);
    $.each(valRows, function (i, value) {
        name = value.getAttribute("name");
        value.setAttribute("data-valmsg-for", ds + "[" + (index + 1) + "]." + name);
    });

    console.log("AddRow Done");
}

/// OnDel
function DelRow(index) {
    console.log("DelRow", index);
    var px = sessionStorage.getItem("DCTS.tablePrefix");
    var ds = sessionStorage.getItem("DCTS." + px + ".dataStructName");
    console.log("DelRow Done");
}

function WrapHeader(headerId) {
    //console.log("WrapHeader", headerId);
    var px = sessionStorage.getItem("DCTS.tablePrefix");

    // Wrap header in a row
    addBtn = AddBtnData(px + "AddBtn");
    $(headerId)
        .wrapAll('<div class="row"></div>')
        .before('<div class="col-sm-1 ">' + addBtn + "</div>")
        .wrapAll('<div class="col-sm-10 ContactHeaderS2"></div>');
    //console.log("WrapHeader Done");
}

function WrapRow(px, rows) {
    console.log("WrapRow", rows);
    var addBtnClass = px + "AddBtn";
    var delBtnClass = px + "DelBtn";
    var contentRowClass = px + "TableRowContent";

    var addBtn = AddBtnData(addBtnClass);
    var delBtn = DelBtnData(delBtnClass);

    $(rows)
        .wrapAll('<div class="row ' + contentRowClass + '"></div>')
        .before('<div class="col-sm-1 ">' + addBtn + "</div>")
        .after("<div class=col-sm-1>" + delBtn + "</div>")
        .wrapAll('<div class="col-sm-10 ContactTableS2"></div>');

    console.log("WrapRow Done");
}

function WrapRows(px) {
    //console.log("WrapRows");
    var rowClass = "." + px + "TableRowData";
    var addBtnClass = px + "AddBtn";
    var delBtnClass = px + "DelBtn";

    WrapRow(px, rowClass);
    /*
    $(rowClass)
        .wrapAll('<div class="row ' + outerRowClass + '"></div>')
        .before('<div class="col-sm-1 ">' + addBtn + "</div>")
        .after("<div class=col-sm-1>" + delBtn + "</div>")
        .wrapAll('<div class="col-sm-10 ContactTableS2"></div>');
    */
    //display '+' in last row
    lastRow = $("." + addBtnClass).last().removeClass("hidden");
    // set id and onclick for each "+" btn
    $.each($("." + addBtnClass), function (index, value) {
        this.setAttribute("id", px + "AddBtn_" + (index - 1));
        this.setAttribute("onclick", "AddRow(" + (index - 1) + ")");
    });
    // add 1 to delete btn index as there is no delete on header
    $.each($("." + delBtnClass), function (index, value) {
        this.setAttribute("id", px + "DelBtn_" + index);
        this.setAttribute("onclick", "DelRow(" + index + ")");
    });

    //console.log("WrapRows Done");
}

(function ($) {
    $.fn.SetRows = function (options) {
        //console.log("SetRows starts...");

        var px = options.tablePrefix;
        var ds = options.dataStructName;
        var nr = options.newRow;
        var vr = options.valRow;

        sessionStorage.setItem("DCTS.tablePrefix", px);
        sessionStorage.setItem("DCTS." + px + ".dataStructName", ds);
        sessionStorage.setItem("DCTS." + px + ".newRow", nr);
        sessionStorage.setItem("DCTS." + px + ".valRow", vr);

        var headerId = "#" + px + "TableHeader";

        WrapHeader(headerId);
        // find existing rows, put "+", "X" on all rows
        WrapRows(px);
        //console.log("SetRows done.");
    };
})(jQuery);

$(function () { // set name = field name in each cell;don't include id
    console.log("Ready called.");
    $("#ContactsTableHeader").SetRows({
        tablePrefix : "Contacts",
        dataStructName: "clients",
        newRow: '<div class="form-group ContactsTableRowContainer">' +
            '<div class="row XYZ ContactsTableRowData">' +
            '<input name="Id" id="clients_0__Id" type="hidden" value="0" data-val-required="The Id field is required." data-val="true" data-val-number="The field Id must be a number.">' +
            '<input name="CompanyID" id="clients_0__CompanyID" type="hidden" value="" data-val="true" data-val-number="The field CompanyID must be a number.">' +
            '<div>' +
            '   <input name="Name" class="form-control col-md-3 text-box single-line" id="clients_0__Name" type="text" value="" data-val-length-max="50" data-val-length="The field Name must be a string with a maximum length of 50." data-val="true">' +
            '</div>' +
            '<div>' +
            '   <input name="JobTitle" class="form-control col-md-3 text-box single-line" id="clients_0__JobTitle" type="text" value="" data-val-length-max="50" data-val-length="The field Job Title must be a string with a maximum length of 50." data-val="true">' +
            '</div>' +
            '<div>' +
            '   <input name="Phone" class="form-control col-md-3 text-box single-line" id="clients_0__Phone" type="tel" value="" data-val-length-max="10" data-val-length="The field Phone must be a string with a maximum length of 10." data-val="true" data-val-regex-pattern="^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$" data-val-regex="Invalid Phone number" data-val-phone="The Phone field is not a valid phone number.">' +
            '</div>' +
            '<div>' +
            '   <input name="Email" class="form-control col-md-3 text-box single-line" id="clients_0__Email" type="email" value="" data-val-length-max="50" data-val-length="The field Email must be a string with a maximum length of 50." data-val="true">' +
            '</div>' +
            '</div>' +
            '</div>',
        valRow: '<div class="row ContactsTableRowValidation">' +
            '<span name="Name" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].Name"></span>' +
            '<span name="Phone" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].Phone"></span>' +
            '<span name="Email" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].Email"></span>' +
            '<span name="JobTitle" class="field-validation-valid text-danger" data-valmsg-replace="true" data-valmsg-for="clients[0].JobTitle"></span>' +
        '</div> '
    });
    //console.log("Ready done.");
});

function AddBtnData(addBtnClass) {
    return '<button type="button" class="btn btn-default hidden ' + addBtnClass + '"> \
                            <span class="glyphicon glyphicon-plus"></span> \
                        </button>';
}

function DelBtnData(delBtnClass) {
    return '<button type="button" class="btn btn-default ' + delBtnClass + '"> \
                            <span class="glyphicon glyphicon-remove"></span> \
                        </button>';
}
