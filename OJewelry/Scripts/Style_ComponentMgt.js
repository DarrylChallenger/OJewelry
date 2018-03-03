﻿
function AddComponentRow(type, index)
{
    // " + len +  is the row that was pressed
    idHeaderBtn = type + "AddBtn";
    idRowBtn = type + "AddBtn_" + index;

    // Remove current '+' on the currently selected row
    if (index === -1) {
        // hide the '+' on the header...
        $("#" + idHeaderBtn).addClass("hidden");
    }
    else { // ... or the button
        $("#" + idRowBtn).addClass("hidden");
    }

    stateClassName = type + "State";
    stateClass = "." + stateClassName;
    len = $(stateClass).length;

    // reset components values on select; totals
    // move ltbordered values into function
    // fix ltbordered values to match CostComponentPartial
    castingsltbordered = getCastingsHTML(type, len);
    stonesltbordered = getStonesHTML(type, len);
    findingsltbordered = getFindingsHTML(type, len);
    laborsltbordered = getLaborsHTML(type, len);
    miscsltbordered = getMiscsHTML(type, len);

    idTotalName = type + "Total";
    idTotal = "#" + idTotalName;
    classhtml = $(stateClass).html();
    var hiddenState = "<input name='" + type + "[" + len + "].SVMState' id='" + type + "_" + len + "__SVMState' type='hidden' value='Added' data-val-required='The SVMState field is required.' data-val='true'>";
    var newState = $("<div class='" + stateClassName + "'></div>").append(hiddenState);
    // handle dropdown data, id, name (etc), add validation in code
    if (type === "Castings") {
        
        var jsVendors = $("#jsVendors").clone();
        jsVendors.find("#jsvINDEX")
            .attr("name", 'Castings[' + len + '].VendorId')
            .attr("id", 'Castings_' + len + '__VendorId')
            .attr("data-val", "true")
            .attr("data-val-number", "The field VendorId must be a number.")
            .attr("data-val-required", "The VendorId field is required.");
        //  < select class="col-sm-1" data- val="true" data- val - number="The field MetalCodeId must be a number." data- val - required="The MetalCodeId field is required." id= "Castings_' + len + '__MetalCodeId" name= "Castings[' + len + '].MetalCodeId" >\

        var jsMetals = $("#jsMetals").clone();
        jsMetals.find("#jsmINDEX")
            .attr("name", 'Castings[' + len + '].MetalCodeId')
            .attr("id", 'Castings_' + len + '__MetalCodeId')
            .attr("data-val", "true")
            .attr("data-val-number", "The field MetalCodeId must be a number.")
            .attr("data-val-required", "The MetalCodeId field is required.");
        ltbordered = castingsltbordered.replace("JSVENDORS", jsVendors.html()).replace("JSMETALS", jsMetals.html());
    }
    if (type === "Stones") { 
        var jsStones = $("#jsStones").clone();
        jsStones.find("#jssINDEX")
            .attr("name", 'Stones[' + len + '].Id')
            .attr("id", 'Stones_' + len + '__Id')
            .attr("data-val", "true")
            .attr("data-val-number", "The Id field (Stones[" + len + "].Id) must be a number.")
            .attr("data-val-required", "The Id field (Stones[" + len + "].Id) is required.")
            .attr("onchange", "StoneChanged('" + len + "')");
        ltbordered = stonesltbordered.replace("JSSTONES", jsStones.html());
    }
    if (type === "Findings") { 
        var jsFindings = $("#jsFindings").clone();
        jsFindings.find("#jsfINDEX")
            .attr("name", 'Findings[' + len + '].Id')
            .attr("id", 'Findings_' + len + '__Id')
            .attr("data-val", "true")
            .attr("data-val-number", "The Id field (Findings[" + len + "].Id) must be a number.")
            .attr("data-val-required", "The Id field (Findings[" + len + "].Id) is required.")
            .attr("onchange", "FindingChanged('" + len + "')");
        ltbordered = findingsltbordered.replace("JSFINDINGS", jsFindings.html());
    }
    if (type === "Labors") {
        ltbordered = laborsltbordered.replace(/INDEX/g, len);
    }
    if (type === "Miscs") {
        ltbordered = miscsltbordered.replace(/INDEX/g, len);
    }
    //console.log(ltbordered)
    var str = newState.add(ltbordered);
    $(idTotal).before(str);
    /* reset validation */
    var form = $("#StylesForm");
    $(form).removeData("validator")             // Added by jQuery Validate
        .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
    $.validator.unobtrusive.parse(form);
    if (type === "Stones") {
        StoneChanged(len);
    }
    if (type === "Findings") {
        FindingChanged(len);
    }
}

function RemoveComponentRow(type, i)
{
    console.log("Remove " + type + "[" + i + "]");

    var rowId = "#" + type + "Row_" + i;
    idHeaderBtn = type + "AddBtn";
    hide = $(rowId).addClass("hidden");
    btnClass = "." + type + "AddBtn";
    rowClass = "." + type + "Row";
    styleClass = ".style" + type;
    thisState = "#" + type + "_" + i + "__SVMState";
    //console.log(str, btnClass)
    console.log(hide);
    console.log(thisState);
    curState = $(thisState).attr("value");
    if (curState === "Added") {
        // physically remove the row (added rows are not in DB, so don't mark them as deleted)
        $(thisState).attr("value", "Clean");
    } else {
        // change the state to deleted
        $(thisState).attr("value", "Deleted");
    }
    // Put the '+' back, find last the !Deleted row. If none, show the header
    // find the  visible rows with a visible add btn
    visibleRowCount = $(styleClass).find(rowClass).not(".hidden").length;
    console.log("vrc", visibleRowCount);
    str = $(styleClass).find(rowClass).not(".hidden").find(btnClass).not(".hidden");
    visibleBtnCount = $(rowClass).not(".hidden").last().length;
    console.log("vbc", visibleBtnCount);
    // if there are none, put the '+' on the last visible btn
    if (visibleRowCount !== 0) {
        // set the last visible row, if any...
        rem = $(rowClass).not(".hidden").last().find(btnClass).removeClass("hidden");
    } else {
        // ... otherwise show the header
        $("#" + idHeaderBtn).removeClass("hidden");
    }
}

function CalcRowTotal(type, rowId)
{
    var ry, qty;
    //console.log("Calc " + type + " row " + rowId + " totals");
    // Iterate through row and compute the row total
    if (type === "Castings")
    {
        // (Price + Labor) * qty
        total = +$("#" + type + "_" + rowId + "__Price").val() + +$("#" + type + "_" + rowId + "__Labor").val();
        qty = total * $("#" + type + "_" + rowId + "__Qty").val();
        rv = $("#" + type + "RowTotalValue_" + rowId).text(qty.toFixed(2));
    }
    if (type === "Stones")
    {
        // (PPC * qty)
        total = +$("#" + type + "_" + rowId + "__PPC").val();
        qty = total * $("#" + type + "_" + rowId + "__Qty").val();
        rv = $("#" + type + "RowTotalValue_" + rowId).text(qty.toFixed(2));
    }
    if (type === "Findings")
    {
        // (Price * qty)
        total = +$("#" + type + "_" + rowId + "__Price").val();
        qty = total * $("#" + type + "_" + rowId + "__Qty").val();
        rv = $("#" + type + "RowTotalValue_" + rowId).text(qty.toFixed(2));
    }
    if (type === "Labors")
    {
        // (PPH + PPP) * qty
        total = +$("#" + type + "_" + rowId + "__PPH").val() + +$("#" + type + "_" + rowId + "__PPP").val();
        qty = total * $("#" + type + "_" + rowId + "__Qty").val();
        rv = $("#" + type + "RowTotalValue_" + rowId).text(qty.toFixed(2));
    }
    if (type === "Miscs")
    {
        // PPP * qty
        total = +$("#" + type + "_" + rowId + "__PPP").val();
        qty = total * $("#" + type + "_" + rowId + "__Qty").val();
        if (isNaN(qty)) qty = 0;
        rv = $("#" + type + "RowTotalValue_" + rowId).text(qty.toFixed(2));
    }
    CalcSubtotals(type);
}

function CalcSubtotals(type) {
    // Iterate through row totals and compute the type total 
    var rows = $("." + type + "RowTotal");
    var total = +0;
    rows.each(function () {
        rv = +$(this).html();
        total = +total + rv;
    });
    if (isNaN(total)) total = 0;
    $("#" + type + "TotalValue").html(total.toFixed(2));
    CalcTotals();
}

function CalcTotals()
{
    //console.log("Calc totals");
    total = 0;
    total = total + +$("#CastingsTotalValue").html() +
        +$("#StonesTotalValue").html() +
        +$("#FindingsTotalValue").html() +
        +$("#LaborsTotalValue").html() +
        +$("#MiscsTotalValue").html();
    if (isNaN(total)) total = 0;
    $("#GrandTotal").html(total.toFixed(2))
    // Iterate thru each total to get the grand total
}

function StoneChanged(i) {
    selected = $("#Stones_" + i + "__Id > option:selected");
    oldval = selected.val();
    dataRow = $("#StonesData").find("#" + oldval);
    $("#Stones_" + i + "__VendorName").val(dataRow.find(".VendorName").attr("value"));
    $("#Stones_" + i + "__CtWt").val(dataRow.find(".CtWt").attr("value"));
    $("#Stones_" + i + "__Size").val(dataRow.find(".Size").attr("value"));
    $("#Stones_" + i + "__PPC").val(dataRow.find(".PPC").attr("value"));
    CalcRowTotal("Stones", i);
}

function FindingChanged(i) {
    selected = $("#Findings_" + i + "__Id > option:selected");
    oldval = selected.val();
    dataRow = $("#FindingsData").find("#" + oldval);
    $("#Findings_" + i + "__VendorName").val(dataRow.find(".VendorName").attr("value"));
    $("#Findings_" + i + "__Metal").val(dataRow.find(".Metal").attr("value"));
    $("#Findings_" + i + "__Price").val(dataRow.find(".Price").attr("value"));
    CalcRowTotal("Findings", i);
}

function getCastingsHTML(type, len) {
    return '\
    <div id= "CastingsRow_' + len + '" class="CastingsRow" > \
        <div class="row ltbordered" > \
            <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id= "Castings_' + len + '__Id" name= "Castings[' + len + '].Id" type= "hidden" value= "'+ 0 + '" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn "> \
                    <div class="col-sm-6 "> \
                        <button type="button" id="CastingsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Castings\', ' + len + ')"> \
                            <span class="glyphicon glyphicon-plus"></span> \
                        </button> \
                    </div> \
                    <div class="col-sm-6"> \
                        <button type="button" class="btn btn-default" onclick="RemoveComponentRow(\'Castings\', ' + len + ')"> \
                            <span class="glyphicon glyphicon-remove"></span> \
                        </button> \
                    </div> <!-- make into button and handle in js/ajax?  --> \
                </div> \
            </div>\
            <input class="col-sm-2 text-box single-line" data-val="true" data-val-required="The Name field is required." id="Castings_' + len + '__Name" name="Castings[' + len + '].Name" type="text" value="" />\
            <div class="col-sm-1">\
            </div >\
            JSVENDORS\
            JSMETALS\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Price must be a number." id="Castings_' + len + '__Price" name="Castings[' + len + '].Price" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Labor must be a number." id="Castings_' + len + '__Labor" name="Castings[' + len + '].Labor" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Castings_' + len + '__Qty" name="Castings[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="CastingsRowTotalValue_' + len + '" class="col-sm-2 CastingsRowTotal ">0.00</div>\
        </div>      \
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Name" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Price" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Labor" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Qty" data-valmsg-replace="true"></span>\
        </div >\
    </div >';
}

function getStonesHTML(type, len) {
    return '\
    <div id="StonesRow_' + len + '"  class="StonesRow">\
        <div class="row ltbordered">\
            <input data- val="true" data- val - number="The field Id must be a number." data- val - required="The Id field is required." scid= "Stones_' + len + '__Id" name= "Stones[' + len + '].scId" type= "hidden" value= "1" />\
            <!--<input id="Stones_' + len + '__Name" name="Stones[' + len +'].Name" type="hidden" value="Duvel Hops" /> < -- should be dropdown -- >\
            <input data-val="true" data-val-number="The field ComponentTypeId must be a number." data-val-required="The ComponentTypeId field is required." id="Stones_' + len + '__ComponentTypeId" name="Stones[' + len + '].ComponentTypeId" type="hidden" value="2" /> -->\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="StonesAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Stones\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>\
                    <div class="col-sm-6">\
                        <button type="button" class="btn btn-default" onclick="RemoveComponentRow(\'Stones\', ' + len + ')">\
                            <span class="glyphicon glyphicon-remove"></span>\
                        </button>\
                    </div> <!-- make into button and handle in js/ajax?  --> \
                </div>\
            </div>\
            JSSTONES\
            <div class="col-sm-1 "></div>\
            <input class="col-sm-2 text-box single-line locked" data-val="true" data-val-required="The Vendor field is required." id="Stones_' + len + '__VendorName" name="Stones[' + len + '].VendorName" type="text" value="" />\
            <input class="col-sm-1 text-box single-line locked" data-val="true" data-val-number="The Caret Weight must be a number." id="Stones_' + len + '__CtWt" name="Stones[' + len + '].Ctwt" type="text" value="0" \"/>\
            <input class="col-sm-1 text-box single-line locked" data-val="true" data-val-number="The Size must be a number." id="Stones_' + len + '__Size" name="Stones[' + len + '].Size" type="text" value="0" \"/>\
            <input class="col-sm-1 text-box single-line locked" data-val="true" data-val-number="The Price/Piece must be a number." id="Stones_' + len + '__PPC" name="Stones[' + len + '].PPC" type="text" value="0.00" <!--onblur="CalcRowTotal(\'' + type + '\', ' + len + ')-->\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Stones_' + len + '__Qty" name="Stones[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="StonesRowTotalValue_' + len + '" class="col-sm-2 StonesRowTotal ">0.00\
            </div>\
        </div >\
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Stones[' + len + '].Qty" data-valmsg-replace="true"></span>\
        </div >\
    </div>';
}

function getFindingsHTML(type, len) {
    return '\
    <div id="FindingsRow_' + len + '"  class="FindingsRow">\
        <div class="row ltbordered">\
            <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id= "Findings_' + len + '__scId" name= "Findings[' + len + '].scId" type= "hidden" value= "4" />\
            <!-- <input id="Findings_' + len + '__Name" name="Findings[' + len + '].Name" type="hidden" value="Duvel Yeast 1" />\
            <input data-val="true" data-val-number="The field ComponentTypeId must be a number." data-val-required="The ComponentTypeId field is required." id="Findings_' + len + '__ComponentTypeId" name="Findings[' + len + '].ComponentTypeId" type="hidden" value="3" /> -->\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="FindingsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Findings\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>\
                    <div class="col-sm-6">\
                        <button type="button" class="btn btn-default" onclick="RemoveComponentRow(\'Findings\', ' + len + ')">\
                            <span class="glyphicon glyphicon-remove"></span>\
                        </button>\
                    </div> <!-- make into button and handle in js/ajax?  -->\
                </div>\
            </div>\
            JSFINDINGS\
            <div class="col-sm-2 "></div>\
            <input class="col-sm-2 text-box single-line locked" data-val="true" data-val-required="The Vendor field is required." id="Findings_' + len + '__VendorName" name="Findings[' + len + '].VendorName" type="text" value="" />\
            <input class="col-sm-1 text-box single-line locked" data-val="true" data-val-required="The Metal is required." id="Findings_' + len + '__Metal" name="Findings[' + len + '].Metal" type="text" value="" />\
            <input class="col-sm-1 text-box single-line locked" data-val="true" data-val-number="The Price must be a number." id="Findings_' + len + '__Price" name="Findings[' + len + '].Price" type="text" value="0.00" <!--onblur="CalcRowTotal(\'' + type + '\', ' + len + ')-->\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Findings_' + len + '__Qty" name="Findings[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="FindingsRowTotalValue_' + len + '" class="col-sm-2 FindingsRowTotal ">0.00\
            </div>\
        </div >\
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Findings[' + len + '].Qty" data-valmsg-replace="true"></span>\
        </div >\
    </div > ';
}

function getLaborsHTML(type, len) {
    return '\
    <div id="LaborsRow_' + len + '" class="LaborsRow">\
        <div class="row ltbordered">\
            <input data- val="true" data- val - number="The field Id must be a number." data- val - required="The Id field is required." id= "Labors_' + len + '__Id" name= "Labors[' + len + '].Id" type= "hidden" value= "1" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="LaborsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Labors\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>\
                    <div class="col-sm-6">\
                        <button type="button" class="btn btn-default" onclick="RemoveComponentRow(\'Labors\', ' + len + ')">\
                            <span class="glyphicon glyphicon-remove"></span>\
                        </button>\
                    </div> <!-- make into button and handle in js/ajax?  -->\
                </div>\
            </div>\
            <input class="col-sm-2 text-box single-line" data-val="true" data-val-required="The Name field is required." id="Labors_' + len + '__Name" name="Labors[' + len + '].Name" type="text" value="" />\
            <input class="col-sm-2 text-box single-line" id="Labors_' + len + '__Desc" name="Labors[' + len + '].Desc" type="text" value="" />\
            <div class="col-sm-2 "></div>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Hour must be a number." id="Labors_' + len + '__PPH" name="Labors[' + len + '].PPH" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Piece must be a number." id="Labors_' + len + '__PPP" name="Labors[' + len + '].PPP" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Labors_' + len + '__Qty" name="Labors[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="LaborsRowTotalValue_' + len + '" class="col-sm-2 LaborsRowTotal">0.00\
            </div>\
        </div >\
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Labors[' + len + '].Name" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Labors[' + len + '].PPH" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Labors[' + len + '].PPP" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Labors[' + len + '].Qty" data-valmsg-replace="true"></span>\
        </div >\
    </div > ';
}

function getMiscsHTML(type, len) {
    return '\
    <div id="MiscsRow_' + len + '"  class="MiscsRow">\
        <div class="row ltbordered">\
            <input data- val="true" data- val - number="The field Id must be a number." data- val - required="The Id field is required." id= "Miscs_' + len + '__Id" name= "Miscs[' + len + '].Id" type= "hidden" value= "1" />\
                <div class="col-sm-1 ">\
                    <div class="row StyleComponentsRowHeaderBtn ">\
                        <div class="col-sm-6 ">\
                            <button type="button" id="MiscsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Miscs\', ' + len + ')">\
                                <span class="glyphicon glyphicon-plus"></span>\
                            </button>\
                        </div>\
                        <div class="col-sm-6">\
                            <button type="button" class="btn btn-default" onclick="RemoveComponentRow(\'Miscs\', ' + len + ')">\
                                <span class="glyphicon glyphicon-remove"></span>\
                            </button>\
                        </div> <!-- make into button and handle in js/ajax?  -->\
                </div>\
            </div>\
            <input class="col-sm-2 text-box single-line" data-val="true" data-val-required="The Name field is required." id="Miscs_' + len + '__Name" name="Miscs[' + len + '].Name" type="text" value="" />\
            <input class="col-sm-2 text-box single-line" id="Miscs_' + len + '__Desc" name="Miscs[' + len + '].Desc" type="text" value="" />\
            <div class="col-sm-3 "></div>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Piece must be a number." id="Miscs_' + len + '__PPP" name="Miscs[' + len + '].PPP" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Miscs_' + len + '__Qty" name="Miscs[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="MiscsRowTotalValue_' + len + '" class="col-sm-2 MiscsRowTotal">0.00</div>\
        </div > \
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Miscs[' + len + '].Name" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Miscs[' + len + '].PPP" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Miscs[' + len + '].Qty" data-valmsg-replace="true"></span>\
        </div >\
    </div>';
}
