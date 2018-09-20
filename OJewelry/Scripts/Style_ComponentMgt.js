
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
    //len = index;
    // reset components values on select; totals
    // move ltbordered values into function
    // fix ltbordered values to match CostComponentPartial

    idBreakName = type + "Break";
    idBreak = "#" + idBreakName;

    btnPos = $("#DelBtnPos").attr("BtnPos");
    leftDelBtn = '';
    rightDelBtn = '';

    if (btnPos === "Right") {
        rightDelBtn = '<div class="col-sm-1"> <div class="row">\
                    <div style= "float:right" >\
                            <button type="button" class="btn btn-default" onclick="RemoveComponentRow(\'' + type + '\', ' + len + ')"> \
                                <span class="glyphicon glyphicon-remove"></span> \
                            </button>\
                    </div>\
                </div></div>';
    } else {
        leftDelBtn = '<button type="button" class="btn btn-default" onclick="RemoveComponentRow(\'' + type + '\', ' + len + ')"> \
                            <span class="glyphicon glyphicon-remove"></span> \
                        </button>';
    }
    classhtml = $(stateClass).html();
    var hiddenState = "<input name='" + type + "[" + len + "].SVMState' id='" + type + "_" + len + "__SVMState' type='hidden' value='Added' data-val-required='The SVMState field is required.' data-val='true'>";
    var newState = $("<div class='" + stateClassName + "'></div>").append(hiddenState);
    // handle dropdown data, id, name (etc), add validation in code
    if (type === "Castings") {
        castingsltbordered = getCastingsHTML(type, len);
        var jsVendors = $("#jsVendors").clone();
        jsVendors.find("#jsvINDEX")
            .attr("name", 'Castings[' + len + '].VendorId')
            .attr("id", 'Castings_' + len + '__VendorId')
            .attr("data-val", "true")
            .attr("data-val-number", "The field VendorId must be a number.")
            .attr("data-val-required", "Please choose a Vendor.");
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
    if (type === "Stones") { // also add a labor setting with the stone row #
        // Stone
        stonesltbordered = getStonesHTML(type, len);
        var jsStones = $("#jsStones").clone();
        jsStones.find("#jssINDEX")
            .attr("name", 'Stones[' + len + '].Name')
            .attr("id", 'Stones_' + len + '__Name')
            .attr("data-val", "true")
            //.attr("data-val-number", "")//"The Id field must be a number.")
            .attr("data-val-required", "Please select a stone. ")
            .attr("onchange", "StoneChanged('" + len + "')");

        //Shape
        var jsShapes = $("#jsShapes").clone();
        jsShapes.find("#jsshINDEX")
            .attr("name", 'Stones[' + len + '].ShId')
            .attr("id", 'Stones_' + len + '__ShId')
            .attr("data-val", "true")
            .attr("data-val-required", "Please select a stone shape. ")
            .attr("onchange", "StoneChanged('" + len + "')")
            ;

        // Size
        var jsSizes = $("#jsSizes").clone();
        jsSizes.find("#jsszINDEX")
            .attr("name", 'Stones[' + len + '].SzId')
            .attr("id", 'Stones_' + len + '__SzId')
            .attr("data-val", "true")
            .attr("data-val-required", "Please select a stone size. ")
            .attr("onchange", "StoneSizeChanged('" + len + "')");
        
        ltbordered = stonesltbordered.replace("JSSTONES", jsStones.html()+jsShapes.html()+jsSizes.html());
    }
    if (type === "Findings") { 
        findingsltbordered = getFindingsHTML(type, len);
        var jsFindings = $("#jsFindings").clone();
        jsFindings.find("#jsfINDEX")
            .attr("name", 'Findings[' + len + '].Id')
            .attr("id", 'Findings_' + len + '__Id')
            .attr("data-val", "true")
            //.attr("data-val-number", "The Id field must be a number.")
            .attr("data-val-required", "Please select a finding.")
            .attr("onchange", "FindingChanged('" + len + "')");
        ltbordered = findingsltbordered.replace("JSFINDINGS", jsFindings.html());
    }
    if (type === "Labors") {
        laborsltbordered = getLaborsHTML(type, len);
        ltbordered = laborsltbordered.replace(/INDEX/g, len);
    }
    if (type === "Miscs") {
        miscsltbordered = getMiscsHTML(type, len);
        ltbordered = miscsltbordered.replace(/INDEX/g, len);
    }
    //console.log(ltbordered)
    var str = newState.add(ltbordered);
    // add after last row or header
    $(idBreak).before(str);
    if (type === "Stones") {
        StoneChanged(len);
        AddStoneSettingRowHTML(len);
    }
    if (type === "Findings") {
        FindingChanged(len);
    }
    /* reset validation */
    var form = $("#StylesForm");
    $(form).removeData("validator")             // Added by jQuery Validate
        .removeData("unobtrusiveValidation");   // Added by jQuery Unobtrusive Validation
    $.validator.unobtrusive.parse(form);
}


function RemoveComponentRow(type, i)
{

    var rowId = "#" + type + "Row_" + i;
    idHeaderBtn = type + "AddBtn";
    hide = $(rowId).addClass("hidden");
    btnClass = "." + type + "AddBtn";
    rowClass = "." + type + "Row";
    styleClass = ".style" + type;
    thisState = "#" + type + "_" + i + "__SVMState";
    //console.log(str, btnClass)

    curState = $(thisState).attr("value");
    if (curState === "Added") {
        // physically remove the row (added rows are not in DB, so don't mark them as deleted)
        $(thisState).attr("value", "Unadded");
    } else {
        if (curState === "Fixed")
        {
            return;
        }
        // change the state to deleted
        $(thisState).attr("value", "Deleted");
    }
    // Put the '+' back, find last the !Deleted row. If none, show in the header
    // find the  visible rows with a visible add btn
    visibleRowCount = $(styleClass).find(rowClass).not(".hidden").length;
    str = $(styleClass).find(rowClass).not(".hidden").find(btnClass).not(".hidden");
    visibleBtnCount = $(rowClass).not(".hidden").last().length;
    //console.log("visibleBtnCount", visibleBtnCount);
    // if there are none, put the '+' on the last visible btn
    if (visibleRowCount !== 0) {
        // set the last visible row, if any...
        rem = $(rowClass).not(".hidden").last().find(btnClass).removeClass("hidden");
    } else {
        // ... otherwise show the header
        $("#" + idHeaderBtn).removeClass("hidden");
    }
    if (type === "Stones") {
        RemoveStoneSettingRow(i);
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
        total = +$("#" + type + "_" + rowId + "__Price").val();
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

function CalcStonesSettingsRow(stoneRow, price, qty) {
   // Totals are in element after the row data
    var total = $("#StoneSetting_" + stoneRow).next();
    total.text((price * qty).toFixed(2));
    CalcSubtotals("Labors");
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
    $("#GrandTotal").html(total.toFixed(2));
    $("#GrandTotal2").html(total.toFixed(2));
    // Iterate thru each total to get the grand total
}

function SetFinishingCost(finishingVal) {
    if ($("#Labors_0__Name").val() === "FINISHING LABOR" && $("#Labors_0__SVMState").val() === "Fixed") {
        $(".finishingPPP").val(finishingVal.toFixed(2));
        CalcRowTotal("Labors", 0);
    }
}

function SetPackagingCost(packagingVal) {
    if ($("#Miscs_0__Name").val() === "PACKAGING" && $("#Miscs_0__SVMState").val() === "Fixed") {
        $(".miscsPPP").val(packagingVal.toFixed(2));
        CalcRowTotal("Miscs", 0);
    }
}

function JewelryTypeChanged(companyId) {
    fetch('/api/AssemblyCostsApi?companyId=' + companyId)
        .then(function (response) {
            return response.json();
        })
        .then(function (cdJSON) {
            // unpack CostData
            var jt = $("#JewelryTypeId :selected").text();
            var costData = JSON.parse(cdJSON);
            var finishingVal = costData.finishingCosts[jt];
            var packagingVal = costData.packagingCosts[jt];
            SetFinishingCost(finishingVal);
            SetPackagingCost(packagingVal);
        });
}

function StoneChanged(i) {
    // pass the stone, shape, and size to StoneMantchingController. Process the result or handle not found 
    var stoneCtl = $("#Stones_" + i + "__Name");
    var shapeCtl = $("#Stones_" + i + "__ShId");
    var sizeCtl = $("#Stones_" + i + "__SzId");
    
    var stone = stoneCtl.val();
    var shape = shapeCtl.val();
    var size =  sizeCtl.val();
    var companyid = $("#CompanyId").val();
    fetch('/api/StoneMatchingApi?companyId=' + companyid + '&stone=' + stone + '&shape=' + shape + '&size=' + size)
        .then(function (response) {
            if (response.ok) {
                return response.json();
            } else {
                // Put the controls in warning mode
                stoneCtl.addClass("badStone");
                shapeCtl.addClass("badStone");
                sizeCtl.addClass("badStone");
                stName = "Setting for stone " + (parseInt(i) + 1);
                $("#StoneSettingName_" + i).val(stName);
                $("#Stones_" + i + "__Price").val("0.00");
                var qty = $("#Stones_" + i + "__Qty").val();

                CalcStonesSettingsRow(i, 0, qty);
            }
        })
        .then(function(stonedata) {
            // unpack stonedata
            // pack the ctwt, vwndor, and price fields
            var stn = JSON.parse(stonedata);
            //console.log(stn);
            $("#Stones_" + i + "__CtWt").val(stn.CtWt);
            $("#Stones_" + i + "__VendorName").val(stn.VendorName);
            $("#Stones_" + i + "__Price").val(stn.Price.toFixed(2));
            CalcRowTotal("Stones", i);
            // Put the controls in OK mode
            stoneCtl.removeClass("badStone");
            shapeCtl.removeClass("badStone");
            sizeCtl.removeClass("badStone");
            UpdateStoneSettingRow(i);
        });
}

function StoneSizeChanged(stoneRow) {
    // Get the assembly costs extract the settings costs
    fetch('/api/AssemblyCostsApi?companyId=' + $("#CompanyId").val())
        .then(function (response) {
            return response.json();
        })
        .then(function (cdJSON) {
            // unpack CostData
            var costData = JSON.parse(cdJSON);
            settingVal = costData.settingsCosts[$("#Stones_" + stoneRow + "__SzId").val()];
            if (settingVal === undefined) {
                settingVal = 0
            }

            $("#StoneSettingPrice_" + stoneRow).val(settingVal.toFixed(2));
            price = settingVal;
        });
    StoneChanged(stoneRow);
}

function StoneQtyChanged(i) {
    CalcRowTotal("Stones", i);
    UpdateStoneSettingRow(i);
}

function FindingChanged(i) {
    selected = $("#Findings_" + i + "__Id > option:selected");
    oldval = selected.val();
    if (oldval !== "") {
        dataRow = $("#FindingsData").find("#" + oldval);
        $("#Findings_" + i + "__VendorName").val(dataRow.find(".VendorName").attr("value"));
        $("#Findings_" + i + "__Weight").val(dataRow.find(".Weight").attr("value"));
        $("#Findings_" + i + "__Price").val(dataRow.find(".Price").attr("value"));
        $("#" + "Findings" + "_" + i + "__Id option[value='']").attr("disabled", "disabled");
    }
    CalcRowTotal("Findings", i);
}

function UpdateStoneSettingRow(stoneRow) {
    // find labor with data-stonerow = stonerow
    var settingRowName = "StoneSetting_" + stoneRow;
    var target = $("#" + settingRowName);
    // if it dowsn't exist, create it - AddStoneSettingRowHTML

    var price, qty;
    var name, shape, size;
    // updates
    // Name - some function of stone name, shape, size
    name = $("#Stones_" + stoneRow + "__Name").val();
    shape = $("#Stones_" + stoneRow + "__ShId").val();
    size = $("#Stones_" + stoneRow + "__SzId").val();

    var stName;
    if (name === "" || shape === "" || size === "") {
        stName = "Setting for stone " + (parseInt(stoneRow) + 1);
    } else {
        stName = "Setting for " + name + "-" + shape + "-" + size;
    }
    $("#StoneSettingName_" + stoneRow).val(stName);

    // Qty = stone QTY
    qty = $("#Stones_" + stoneRow + "__Qty").val();
    $("#StoneSettingQty_" + stoneRow).val(qty);

    price = $("#StoneSettingPrice_" + stoneRow).val();
    CalcStonesSettingsRow(stoneRow, price, qty);

}

function AddStoneSettingRowHTML(stoneRow) {
    ltbordered = getStoneSettingsHTML("Stones", len);
    $("#LaborsTotal").before(ltbordered);

    // PPP - get from API
    fetch('/api/AssemblyCostsApi?companyId=' + $("#CompanyId").val())
        .then(function (response) {
            return response.json();
        })
        .then(function (cdJSON) {
            // unpack CostData
            var jt = $("#JewelryTypeId :selected").text();
            var costData = JSON.parse(cdJSON);
            settingVal = costData.settingsCosts[$("#Stones_" + stoneRow + "__SzId").val()];
            if (settingVal === undefined) {
                settingVal = 0;
            }
            $("#StoneSettingPrice_" + stoneRow).val(settingVal.toFixed(2));
            price = settingVal;
            UpdateStoneSettingRow(stoneRow);
        });


}

function RemoveStoneSettingRow(stoneRow) {
    // find labor for stonerow & hide the enclosing ltbordered
    var target = $("#StoneSetting_" + stoneRow).parent();
    if (target.hasClass("ltbordered")) {
        target.addClass("hidden");
    }
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
                    </div> '
                + leftDelBtn +
                '</div> \
            </div>\
            <input class="col-sm-2 text-box single-line requiredifnotremoved"  placeholder="Name" data-val-required="The Metal Style name field is required." id="Castings_' + len + '__Name" name="Castings[' + len + '].Name" type="text" value="" />\
            <div class="col-sm-1">\
            </div >\
            JSVENDORS\
            JSMETALS\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Price must be a number." id="Castings_' + len + '__Price" name="Castings[' + len + '].Price" type="text" value="0.00" disabled="disabled" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Labor must be a number." id="Castings_' + len + '__Labor" name="Castings[' + len + '].Labor" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Castings_' + len + '__Qty" name="Castings[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="CastingsRowTotalValue_' + len + '" class="col-sm-1 CastingsRowTotal ">0.00</div>\
            ' + rightDelBtn + '\
            </div>\
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
    <div id="StonesRow_' + len + '"  class="StonesRow ">\
        <div class="row ltbordered">\
            <input data-val="true" data-val-number="The field Id must be a number." id= "Stones_' + len + '__Id" name= "Stones[' + len + '].Id" type= "hidden" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="StonesAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Stones\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>'
                + leftDelBtn +    
                '</div>\
            </div>\
            JSSTONES\
            <input class="col-sm-1 text-box single-line locked" disabled = "disabled" data-val="true" data-val-number="The Caret Weight must be a number." id="Stones_' + len + '__CtWt" name="Stones[' + len + '].Ctwt" type="text" value="" \"/>\
            <input class="col-sm-2 text-box single-line locked" disabled = "disabled" data-val="true" data-val-required="The Vendor field is required." id="Stones_' + len + '__VendorName" name="Stones[' + len + '].VendorName" type="text" value="" />\
            <input class="col-sm-1 text-box single-line locked" disabled = "disabled" data-val="true" data-val-number="The Price field must be a number." id="Stones_' + len + '__Price" name="Stones[' + len + '].Price" type="text" value="0.00" <!--onblur="CalcRowTotal(\'' + type + '\', ' + len + ')-->\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Stones_' + len + '__Qty" name="Stones[' + len + '].Qty" type="text" value="1" onblur="StoneQtyChanged(' + len + ')\"/>\
            <div id="StonesRowTotalValue_' + len + '" class="col-sm-1 StonesRowTotal ">0.00</div>\
            ' + rightDelBtn + '\
           </div>\
           <div class="row">\
           <!--Validations Here-->\
               <span class="field-validation-valid text-danger" data-valmsg-for="Stones[' + len + '].Id" data-valmsg-replace="true"></span>\
               <span class="field-validation-valid text-danger" data-valmsg-for="Stones[' + len + '].Name" data-valmsg-replace="true"></span>\
               <span class="field-validation-valid text-danger" data-valmsg-for="Stones[' + len + '].ShId" data-valmsg-replace="true"></span>\
               <span class="field-validation-valid text-danger" data-valmsg-for="Stones[' + len + '].SzId" data-valmsg-replace="true"></span>\
               <span class="field-validation-valid text-danger" data-valmsg-for="Stones[' + len + '].Qty" data-valmsg-replace="true"></span>\
           </div>\
        </div>\
    </div>';
}

function getFindingsHTML(type, len) {
    return '\
    <div id="FindingsRow_' + len + '"  class="FindingsRow">\
        <div class="row ltbordered">\
            <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The scId field is required." id= "Findings_' + len + '__scId" name= "Findings[' + len + '].scId" type= "hidden" value= "-1" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="FindingsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Findings\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>'
                + leftDelBtn +                
                '</div >\
            </div>\
            JSFINDINGS\
            <div class="col-sm-2 "></div>\
            <input class="col-sm-2 text-box single-line locked" disabled = "disabled" data-val="true" data-val-required="The Vendor field is required." id="Findings_' + len + '__VendorName" name="Findings[' + len + '].VendorName" type="text" value="" />\
            <input class="col-sm-1 text-box single-line locked" disabled = "disabled" data-val="true" data-val-required="The field Weight must be a number." id="Findings_' + len + '__Weight" name="Findings[' + len + '].Weight" type="text" value="" />\
            <input class="col-sm-1 text-box single-line locked" disabled = "disabled" data-val="true" data-val-number="The Price must be a number." id="Findings_' + len + '__Price" name="Findings[' + len + '].Price" type="text" value="0.00" <!--onblur="CalcRowTotal(\'' + type + '\', ' + len + ')-->\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Findings_' + len + '__Qty" name="Findings[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="FindingsRowTotalValue_' + len + '" class="col-sm-1 FindingsRowTotal ">0.00</div>\
            ' + rightDelBtn + '\
            </div>\
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Findings[' + len + '].Id" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Findings[' + len + '].Qty" data-valmsg-replace="true"></span>\
        </div >\
        </div >\
    </div > ';
}

function getLaborsHTML(type, len) {
    return '\
    <div id="LaborsRow_' + len + '" class="LaborsRow">\
        <div class="row ltbordered">\
            <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id= "Labors_' + len + '__Id" name= "Labors[' + len + '].Id" type= "hidden" value= "0" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="LaborsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Labors\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>'
        + leftDelBtn +
        '</div>\
            </div>\
            <input class="col-sm-2 text-box single-line requiredifnotremoved" placeholder="Name"  id="Labors_' + len + '__Name" name="Labors[' + len + '].Name" type="text" value="" />\
            <input class="col-sm-2 text-box single-line" id="Labors_' + len + '__Desc" name="Labors[' + len + '].Desc" type="text" value="" />\
            <div class="col-sm-2 "></div>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Hour must be a number." id="Labors_' + len + '__PPH" name="Labors[' + len + '].PPH" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Piece must be a number." id="Labors_' + len + '__PPP" name="Labors[' + len + '].PPP" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Labors_' + len + '__Qty" name="Labors[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="LaborsRowTotalValue_' + len + '" class="col-sm-1 LaborsRowTotal">0.00</div>\
            ' + rightDelBtn + '\
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

function getStoneSettingsHTML(type, len) {
    return '\
        <div class="row ltbordered">\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="LaborsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn hidden" onclick="AddComponentRow(\'Labors\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>'
                    + leftDelBtn +
                '</div>\
            </div>\
            <div id=StoneSetting_'+ len + '>\
                <input disabled="" class="col-sm-4 text-box single-line locked" id="StoneSettingName_' + len + '" type="text" value="Setting" />\
                <div class="col-sm-2 "></div>\
                <div class="col-sm-1 "></div>\
                <input disabled="" class="col-sm-1 text-box single-line locked" id="StoneSettingPrice_' + len + '" type="text" value="0.00" />\
                <input disabled="" class="col-sm-1 text-box single-line locked" id="StoneSettingQty_' + len + '" type="text" value="0" />\
            </div>\
            <div id="LaborsRowTotalValue_' + len + '" class="col-sm-1 LaborsRowTotal" name="StoneSettingRowTotalValue_' + len + '">0.00</div>\
        </div >\
';
}

function getMiscsHTML(type, len) {
    return '\
    <div id="MiscsRow_' + len + '"  class="MiscsRow">\
        <div class="row ltbordered">\
            <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id= "Miscs_' + len + '__Id" name= "Miscs[' + len + '].Id" type= "hidden" value= "0" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="MiscsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'Miscs\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>'
                + leftDelBtn +
                '</div>\
            </div>\
            <\input class="col-sm-2 text-box single-line requiredifnotremoved" placeholder="Name"  id="Miscs_' + len + '__Name" name="Miscs[' + len + '].Name" type="text" value="" />\
            <input class="col-sm-2 text-box single-line" id="Miscs_' + len + '__Desc" name="Miscs[' + len + '].Desc" type="text" value="" />\
            <div class="col-sm-3 "></div>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Piece must be a number." id="Miscs_' + len + '__PPP" name="Miscs[' + len + '].PPP" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Miscs_' + len + '__Qty" name="Miscs[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="MiscsRowTotalValue_' + len + '" class="col-sm-1 MiscsRowTotal">0.00</div>\
            ' + rightDelBtn + '\
         </div > \
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Miscs[' + len + '].Name" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Miscs[' + len + '].PPP" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Miscs[' + len + '].Qty" data-valmsg-replace="true"></span>\
        </div >\
    </div>';
}

function setAddBtn(type)
{
    var VisibleRows = $("." + type + "State :first-child[value|='Dirty'], ." +
        type + "State :first-child[value|='Added'], ." +
        type + "State :first-child[value|='Fixed']");
    var VisibleRowCount = VisibleRows.length;
    if (VisibleRowCount === 0) {
        // unhide the header '+' btn
        $("." + type + "AddBtn").first().removeClass("hidden");
    } else {
        // unhide the last row unhidden header '+' btn
        VisibleRows.last().parent().next().find("." + type + "AddBtn").removeClass("hidden");
    }
}

$(function () { // 
    setAddBtn("Castings");
    setAddBtn("Stones");
    setAddBtn("Findings");
    setAddBtn("Labors");
    setAddBtn("Miscs");
    CalcSubtotals("Castings");
    CalcSubtotals("Stones");
    CalcSubtotals("Findings");
    CalcSubtotals("Labors");
    CalcSubtotals("Miscs");
    CalcTotals();

    $.validator.addMethod("requiredifnotremoved", function (value, element) { //--- does this get called?
        var elementId = $(element).attr("id");
        if (elementId === "jssINDEX" || elementId === "jsshINDEX" || elementId === "jsszINDEX" || elementId === "jsfINDEX") {
            return true;
        }
        if ($(element).hasClass("input-validation-error")) {
            $("#vMsg").attr("data-msg", $(element).attr(""));
        }
        var target = $(element).parent().parent().prev().children();
        var state = $(target).val();
        //console.log($(element).attr("id") + " " + state);
        if (state === "Deleted" || state === "Unadded") {
            return true;
        }
        var rt = $.validator.methods.required.call(this, value, element);

        return rt;
    }, $("#vMsg").attr("data-msg"));
}); // Set button, subtotals



