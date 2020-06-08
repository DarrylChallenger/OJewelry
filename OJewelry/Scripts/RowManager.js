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
async function AddRow(index) {
    //console.log("AddRow", index);
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
    var contentRowClass = px + "TableRowContent";
    var state = "." + px + "State";

    var companyId = $("#CompanyId");
    //console.log(`nr: ${nr}`);
    // hide the '+' btn
    $("." + addBtnClass).addClass("hidden");

    // insert new row (find last row by class)
    if (index === -1)
    {
        // add after header
        $("#" + tableClass).last().append(nr);
   } else {
        // add after last row
        $("#" + tableClass).last().append(nr);
    }

    // Search for "<getoptions>" id; call backend with value. Replace element with the set of returned options
    var getops = $(nr).find("#getoptions");
    // console.log(`getops : ${JSON.stringify(getops)}, ${getops.val()}`);
    // console.log(`***: ${JSON.stringify($("#getoptions"))} `);
    // $(".ltbordered").last().css("border-color", "yellow");
    if (getops.val()) {
        try {
            $("#getoptions").last().html(`Retrieving options for ${getops.val()}`);
            $(".ltbordered").last().css("border-color", "blue");
            let response = await fetch('/api/DropdownApi?companyId=' + companyId.val() + '&dropdown=' + getops.val());
            let options_string = await response.json();
            let options = JSON.parse(options_string);
            if (options) {
                $(".ltbordered").last().css("border-color", "#cccccc");
                for (let opt of options) {
                    $("#getoptions").before(`<option value='${opt.Value !== "0" ? opt.Value : ""}'>${opt.Text}</option>`);
                    //console.log(`added id[${opt.Value !== 0 ? opt.Value : ""}], val[${opt.Text}], getops:${JSON.stringify(getops)}`);
                }
                $("#getoptions").remove();
            }
            //$(".ltbordered").last().css("border-color", "orange");
        } catch (e) {
            console.error(`DropdownApi: Error retrieving options for ${getops.val()}`, e);
            //alert(`DropdownApi: Error retrieving options for ${getops.val()}`);
            $("#getoptions").last().html(`DropdownApi: Error retrieving options for ${getops.val()}`);
            $(".ltbordered").last().css("border-color", "red");

        }
    }

    // wrap it...
    var newRow = $(rowClass).last();
    //console.log(`vr: ${vr}`);
    WrapRow(px, newRow);
    // Insert validation after TableRowContainer
    $("." + tableRowClass).last().append(vr);

    // state = "Added"
    //console.log("tlc=", tableRowClass);
    $("." + tableRowClass).last().find("." + px + "State").val("Added");

    // Show the button
    $("." + addBtnClass).last().removeClass("hidden");

    // add id, onclick handler to btns
    var newIndex = $("." + tableRowClass).length - 1;
    //console.log("newIndex:", newIndex);
    $("." + addBtnClass).last().attr("id", addBtnClass + "_" + newIndex).attr("onclick", "AddRow(" + newIndex + ")");
    $("." + delBtnClass).last().attr("id", delBtnClass + "_" + newIndex).attr("onclick", "DelRow(" + newIndex + ")");
    // Add correct name/id to each child in row
    $.each(newRow.find(":input"), function (i, value) {
        name = value.getAttribute("name");
        value.setAttribute("name", ds + "[" + newIndex + "]." + name);
        value.setAttribute("id", ds + "_" + newIndex + "__" + name);
    });
    name = $(newRow.parents("." + tableRowClass)).find(state).attr("name");
    $(newRow.parents("." + tableRowClass)).find(state).attr("name", ds + "[" + newIndex + "]." + name).attr("id", ds + "_" + newIndex + "__" + name);
    // set names/values validation
    valRows = $("." + valRowContent).last().children();
    $.each(valRows, function (i, value) {
        name = value.getAttribute("name");
        value.setAttribute("data-valmsg-for", ds + "[" + newIndex + "]." + name);
    });

    resetValidation(px);
    //console.log("AddRow Done");
}

/// OnDel
function DelRow(index) {
    //console.log("DelRow", index);
    var px = sessionStorage.getItem("DCTS.tablePrefix");
    //var ds = sessionStorage.getItem("DCTS." + px + ".dataStructName");
    //var contentRowClass = px + "TableRowContent";
    var tableRowClass = px + "TableRowContainer";
    var addBtnClass = px + "AddBtn";
    var state = "." + px + "State";

    //console.log("old container:", $($("." + tableRowClass)[index]));
    //console.log("old children:", $($("." + tableRowClass)[index]).children());
    //console.log("old state:", $($("." + tableRowClass)[index]).children(state));
    //console.log("old state val:", $($("." + tableRowClass)[index]).children(state).val());
    if ($($("." + tableRowClass)[index]).find(state).val() === "Added") {
        $($("." + tableRowClass)[index]).find(state).val("Unadded");
    } else {
        $($("." + tableRowClass)[index]).find(state).val("Deleted");
    }
    //console.log("new state:" + $($("." + tableRowClass)[index]).children(state).val());
    // hide Container
    var container = $($("." + tableRowClass)[index]);
    container.addClass("hidden");

    // Show the button (last btn not in hidden container)
    //$("." + addBtnClass).parents($("." + tableRowClass))
    if ($("." + tableRowClass + ":not(.hidden)").last().find("." + addBtnClass).removeClass("hidden").length === 0) {
        $("." + addBtnClass).first().removeClass("hidden");
    } else {
        $("." + tableRowClass + ":not(.hidden)").last().find("." + addBtnClass).removeClass("hidden");
    }

    //console.log("DelRow Done");
}

function WrapHeader(headerId) {
    //console.log("WrapHeader", headerId);
    var px = sessionStorage.getItem("DCTS.tablePrefix");

    // Wrap header in a row
    addBtn = AddBtnData(px + "AddBtn");
    $(headerId)
        .wrapAll('<div class="row"></div>')
        .before('<div class="col-sm-1 ">' + addBtn + "</div>")
        .wrapAll(`<div class="col-sm-10 ${px}Header"></div>`);
    //console.log("WrapHeader Done");
}

function WrapRow(px, rows, bltBordered= "") {
    //console.log("WrapRow", rows);
    var addBtnClass = px + "AddBtn";
    var delBtnClass = px + "DelBtn";
    var contentRowClass = px + "TableRowContent";

    var addBtn = AddBtnData(addBtnClass);
    var delBtn = DelBtnData(delBtnClass);

    //console.log("rows.length:", $(rows).length);
    $.each($(rows), function () {
        var element = $(this);
        element.wrapAll(`<div class="row ${contentRowClass} ${bltBordered}"></div>`)
            .before('<div class="col-sm-1 ">' + addBtn + "</div>")
            .after("<div class=col-sm-1>" + delBtn + "</div>")
            .wrapAll(`<div class="col-sm-10 ${px}Table"></div>`);
    });
    
    //console.log("WrapRow Done");
}

function WrapRows(px) {
    //console.log("WrapRows");
    var rowClass = "." + px + "TableRowData";
    var tableRowClass = "." + px + "TableRowContainer";

    var addBtnClass = px + "AddBtn";
    var delBtnClass = px + "DelBtn";

    //console.log("rowClass b4 loop:", $(rowClass));

    WrapRow(px, rowClass);
    //display '+' in last row
    //lastRow = $("." + addBtnClass).last().removeClass("hidden");
    visibleRowCount = $(tableRowClass).not(".hidden").length;

    //console.log($("." + addBtnClass).length);
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

    if (visibleRowCount !== 0) {
        rem = $(tableRowClass).not(".hidden").last().find("." + addBtnClass).removeClass("hidden");
    } else {
        var bt = "#" + addBtnClass + "_-1";
        rem = $(bt).removeClass("hidden");
        //console.log(`bt: [${bt}], visibleRowCount`, visibleRowCount);
    }

    //console.log("WrapRows Done");
}

(function ($) {
    $.fn.SetRows = function (options) {
        //console.log("SetRows starts...");

        var px = options.tablePrefix;
        var ds = options.dataStructName;
        var nr = options.newRow;
        var vr = options.valRow;
        var fn = options.formName;

        sessionStorage.setItem("DCTS.tablePrefix", px);
        sessionStorage.setItem("DCTS." + px + ".dataStructName", ds);
        sessionStorage.setItem("DCTS." + px + ".newRow", nr);
        sessionStorage.setItem("DCTS." + px + ".valRow", vr);
        sessionStorage.setItem("DCTS." + px + ".fromName", fn);

        var headerId = "#" + px + "TableHeader";

        WrapHeader(headerId);
        // find existing rows, put "+", "X" on all rows
        WrapRows(px);
        //console.log("SetRows done.");
    };
})(jQuery);

$(function () { 
    //console.log("Ready called.");
    var px = sessionStorage.getItem("DCTS.tablePrefix");
    var tableRowClass = "." + px + "TableRowContainer";
    var stateClass = "." + px + "State";

    $.validator.addMethod("requiredifnotremoved", function (value, element) {
        var state = $(element).parents(tableRowClass).find(stateClass).val();
        var e = $(element).parents(tableRowClass);
        //console.log(`tableRowClass: ${tableRowClass}, stateClass: ${stateClass} validating element: [${e}], state:[${state}], value: [${value}]`); 
        //console.log("element",e);
        if (state === "Deleted" || state === "Unadded") {
            //console.log(`id ${e.attr("id")} ignored`);
            return true;
        }
        return rt = $.validator.methods.required.call(this, value, element);
    });
    //console.log("Ready done.");
});



function resetValidation(px)
{
    formName = sessionStorage.getItem("DCTS." + px + ".fromName");
    var form = $("#" + formName);
    $(form).removeData("validator")             // Added by jQuery Validate
        .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
    $.validator.unobtrusive.parse(form);
}

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

