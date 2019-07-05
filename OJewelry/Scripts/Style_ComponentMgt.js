
async function AddComponentRow(type, index)
{
    //console.log(`type: ${type}`);
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
    var ltbordered;
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
    var hiddenState = "<input name='" + type + "[" + len + "].State' id='" + type + "_" + len + "__State' type='hidden' value='Added' data-val-required='The State field is required.' data-val='true'>";
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
            .attr("data-val-required", "The MetalCodeId field is required.")
            .attr("onchange", "CalcMetalPrice(" + len + ")");

        var jsMetalUnits = $("#jsMetalUnits").clone();
        jsMetalUnits.find("#jsuINDEX")
            .addClass("requiredifnotremoved")
            .attr("name", 'Castings[' + len + '].MetalWtUnitId')
            .attr("id", 'Castings_' + len + '__MetalWtUnitId')
            .attr("data-val", "true")
            //.attr("data-val-number", "The field MetalWeightUnitId must be a number.")
            .attr("data-val-required", "The Metal Weight Unit field is required.")
            .attr("onchange", "CalcMetalPrice(" + len + ")");
        ltbordered = castingsltbordered.replace("JSVENDORS", jsVendors.html()).replace("JSMETALS", jsMetals.html()).replace("JSMETALUNITS", jsMetalUnits.html());
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
    if (type === "LaborItems") {
        laboritemsltbordered = getLaborItemsHTML(type, len);
        //console.log(`laboritemsltbordered: [${JSON.stringify(laboritemsltbordered)}]`);
        //console.log(`laboritemsltbordered: [${laboritemsltbordered}]`);
        var dropdown = await PopulateDropdowns(laboritemsltbordered); 
        //console.log(`*** dropdown: [${dropdown}]`);
        //console.log(`laboritemsltbordered2: [${laboritemsltbordered}]`);
        // replace #getoption with dropdown
        //ltbordered = laboritemsltbordered;
        //ltbordered = $(laboritemsltbordered).find("#getoptions").parent().replaceWith(`${dropdown}`);
        var t1 = $(laboritemsltbordered).find("#getoptions").parent();
        //console.log(`t1: [${t1.html()}]`);
        ltbordered = laboritemsltbordered.replace(t1.html(), dropdown);
        //console.log(`t2: [${t2}]`);
        //console.log(`li-ltbordered: [${$(ltbordered).html()}]`);
    }
    if (type === "Miscs") {
        miscsltbordered = getMiscsHTML(type, len);
        ltbordered = miscsltbordered.replace(/INDEX/g, len);
    }
    var str = newState.add(ltbordered);
    // add after last row or header ***
    //console.log(`adding row: ${JSON.stringify(str)}`);
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
    styleClass = ".style" + type; if (type === "LaborItems") { styleClass = ".styleLabors"; }
    thisState = "#" + type + "_" + i + "__State";
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

async function PopulateDropdowns(nr) {
    //console.log(`PopulateDropdowns: nr: ${JSON.stringify(nr)}`);
    // Search for "<getoptions>" id; call backend with value. Replace element with the set of returned options
    var getops = $(nr).find("#getoptions");
    //console.log(`getops : ${JSON.stringify(getops)}, ${getops.val()}`);
    const companyId = $("#CompanyId");
    var drp = '';
    if (getops.val()) {
        await fetch('/api/DropdownApi?companyId=' + companyId.val() + '&dropdown=' + getops.val())
        .then(function (response) {
            if (response.ok) {
                return response.json();
            } else {
                // Replace choose with msg indicating there are no vendors
                console.error(`No ${getops.val()} found for company ${companyId.val()}`);
                return null;
            }
        }).then(function (options_string) {
            // unpack options
            let options = JSON.parse(options_string);
            //console.log(`options: ${JSON.stringify(options)}`);
            if (options) {
                for (var opt of options) {
                    drp+=`<option value='${opt.Value !== "0" ? opt.Value : ""}'>${opt.Text}</option>`;
                    //console.log(`added id[${opt.Value !== 0 ? opt.Value : ""}], val[${opt.Text}], getops:${JSON.stringify(getops)}`);
                }
                //console.log(`final drp: ${JSON.stringify(drp)}`);
            }
        }).catch(function (e) {
            console.error(`DropdownApi: Error retrieving options for ${getops.val()}`, e);
            //UpdateStoneSettingRow(i, 0, false);
        });
    }
    // console.log(`outside drp: ${JSON.stringify(drp)}`);
    return drp;
}

function CalcRowTotal(type, rowId)
{
    var rv, qty;
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
    if (type === "LaborItems") { 
        // (pph + ppp) * qty
        total = +$("#" + type + "_" + rowId + "__pph").val() + +$("#" + type + "_" + rowId + "__ppp").val();
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

function CalcMetalPrice(i) {

    var unitMultiplier;
    var weight = $("#Castings_" + i + "__MetalWeight").val();
    var unitCode = $("#Castings_" + i + "__MetalWtUnitId :selected").html().trim();
    if (unitCode === "DWT") {
        unitMultiplier = 1;
    }
    else {
        unitMultiplier = 1.555;
    }
    var metalCode = $("#Castings_" + i + "__MetalCodeId :selected").html();
    var metalMarketPrice = 1; //cd.metalMarketPrice[MetalCode]
    var metalMultiplier = 1;
    var price = 0;

    var companyid = $("#CompanyId").val();

    // Get the assembly costs extract the metal costs
    fetch('/api/AssemblyCostsApi?companyId=' + $("#CompanyId").val())
        .then(function (response) {
            if (response.ok) {
                return response.json();
            } else {
                $("#Castings_" + i + "__Price").val(price);
                CalcRowTotal("Castings", i);
            }
        })
        .then(function (cdJSON) {
            // unpack CostData
            var costData = JSON.parse(cdJSON);
            metalMarketPrice = costData.metalMarketPrice[metalCode];
            metalMultiplier = costData.metalMultiplier[metalCode];
            //price = metalMarketPrice * metalMultiplier * weight * unitMultiplier; old formula
            price = metalMultiplier * weight * unitMultiplier;
            $("#Castings_" + i + "__Price").val(price.toFixed(2));
            CalcRowTotal("Castings", i);
        }).catch(function (e) {
            console.log("Error retrieving assembly cost data (cmp)", e);
        });
}

function CalcStonesSettingsRow(stoneRow, price, qty) {
   // Totals are in element after the row data
    var total = $("#StoneSetting_" + stoneRow).next();
    total.text((price * qty).toFixed(2));
    CalcSubtotals("Labors");
}

function CalcSubtotals(type) {
    //console.log(`CalcSubtotals type : ${type}`);
    // Iterate through row totals and compute the type total 
    var rows = $("." + type + "RowTotal");
    var total = +0;
    let rv;
    rows.each(function () {
        rv = +$(this).html();
        total = +total + rv;
    });
    //console.log(`total: ${total}`);
    if (isNaN(total)) total = 0;
    if (type === "LaborItems" || type === "Labors") {
        //$("#" + type + "SectionSubtotal").html(total.toFixed(2));
        if ($(Style_JewelryType_bUseLaborTable).val() !== "true") {
            //$("#LaborsTotalValue").html($("#LaborsSectionSubtotal").html());
            $("#LaborsTotalValue").html(total.toFixed(2));
        } else {
            //$("#LaborsTotalValue").html($("#LaborItemsSectionSubtotal").html());
            $("#LaborsTotalValue").html(total.toFixed(2));
        }
    } else {
        $("#" + type + "TotalValue").html(total.toFixed(2));
    }
    CalcTotals();
}

function CalcTotals()
{
    //console.log(`Calc totals bUseLT: ${ $("#Style_JewelryType_bUseLaborTable").val() === "true" ? true : false}`);
    let total = +0;
    let lbrtl = $("#Style_JewelryType_bUseLaborTable").val() === "true" ? $("#LaborItemsSectionSubtotal").html() : $("#LaborsSectionSubtotal").html();
    //console.log(`lbrtl: ${lbrtl}`);
    total = total + +$("#CastingsTotalValue").html() +
        +$("#StonesTotalValue").html() +
        +$("#FindingsTotalValue").html() +
        +$("#LaborsTotalValue").html() +
        +$("#MiscsTotalValue").html();
//    console.log(`total: ${total}`);
    if (isNaN(total)) total = 0;
    //console.log(`total2: ${total}`);
    $("#GrandTotal").html(total.toFixed(2));
    $("#GrandTotal2").html(total.toFixed(2));
    // Iterate thru each total to get the grand total
}

function SetFinishingCost(finishingVal) {
    if ($("#Labors_0__Name").val() === "FINISHING LABOR" && $("#Labors_0__State").val() === "Fixed") {
        $(".finishingPPP").val(finishingVal.toFixed(2));
        CalcRowTotal("Labors", 0);
    }
}

function SetPackagingCost(packagingVal) {
    //console.log(`packagingVal: ${packagingVal}`);
    if ($("#Miscs_0__Name").val() === "PACKAGING" && $("#Miscs_0__State").val() === "Fixed") {
        $(".miscsPPP").val(packagingVal.toFixed(2));
        CalcRowTotal("Miscs", 0);
    }
}

async function LaborItemsDropdownChanged(rowId) {
    let selectedItemId = $(`#LaborItems_${rowId}__laborItemId`).val();
    //console.log(`rowId: ${rowId}, selectedItemId: ${selectedItemId}`);
    if (selectedItemId) {
        const response = await fetch('/api/LaborItemsApi?id=' + selectedItemId);
        const laborItemString = await response.json();
        //console.log(`laborItemString: ${laborItemString}`);
        const laborItem = JSON.parse(laborItemString);
        //console.log(`laborItem: ${JSON.stringify(laborItem)}`);
        $(`#LaborItems_${rowId}__pph`).val(laborItem.pph);
        $(`#LaborItems_${rowId}__ppp`).val(laborItem.ppp);
        $(`#LaborItems_${rowId}__VendorName`).val(laborItem.Vendor);
        CalcRowTotal("LaborItems", rowId);
    }
}

// Rework this: Always call jtApi. If !bUseLT, get assembly costs.
function JewelryTypeChanged() { 
    var jtid = $("#Style_JewelryTypeId :selected").val();
    fetch('/api/JewelryTypesApi?id=' + jtid)
    .then(function (response) {
        return response.json();
    })
        .then(function (jewelryTypeJSON) {
        var packagingVal;
        const jewelryType = JSON.parse(jewelryTypeJSON);
        //console.log(`jewelryType: ${JSON.stringify(jewelryType)}`);
        var jt = $("#Style_JewelryTypeId :selected").text();
        if (jewelryType.bUseLaborTable === false) {
            // toggle .HideLabors
            $(`.StyleLaborItemsSection`).hide();
            $(`.StyleLaborsSection`).show();
            //$(`.StyleLaborItemsSection`).css("opacity", ".4");
            //$(`.StyleLaborsSection`).css("opacity", "1");

            $("#Style_JewelryType_bUseLaborTable").val(false);
            //console.log(`jewelryType.bUseLaborTable: ${jewelryType.bUseLaborTable}`);
            //console.log(`jt: [${jt}]`);
            var finishingVal = jewelryType.costData.finishingCosts[jt];
            packagingVal = jewelryType.costData.packagingCosts[jt];

            SetFinishingCost(finishingVal);
            SetPackagingCost(packagingVal);
            CalcSubtotals("Labors");
        } else {
            // toggle .HideLaborItems
            $(`.StyleLaborsSection`).hide();
            $(`.StyleLaborItemsSection`).show();
            //$(`.StyleLaborsSection`).css("opacity", ".4");
            //$(`.StyleLaborItemsSection`).css("opacity", "1");

            $("#Style_JewelryType_bUseLaborTable").val(true);
            //console.log(`jewelryType.bUseLaborTable: ${jewelryType.bUseLaborTable}`);
            packagingVal = jewelryType.costData.packagingCosts[jt];
            SetPackagingCost(packagingVal);
            CalcSubtotals("LaborItems");
        }
    }).catch(function (e) {
        console.log("Error retrieving jewelry type data (jtc)", e);
    });

}

function StoneChanged(i) {
    // pass the stone, shape, and size to StoneMatchingController. Process the result or handle not found 
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
                //console.log("Put the controls in warning mode");
                stoneCtl.addClass("badStone");
                shapeCtl.addClass("badStone");
                sizeCtl.addClass("badStone");
                stName = "Invalid stone combination";//"Setting for stone " + (parseInt(i) + 1);
                $("#StoneSettingName_" + i).val(stName);
                $("div[name='StoneSettingRowTotalValue_" + i + "']").addClass("badTotal");
                $("#Stones_" + i + "__Price").val("0.00");
                var qty = $("#Stones_" + i + "__Qty").val();
                UpdateStoneSettingRow(i, 0, false);
                return null;
            }
        }).then(function (stonedata) {
            // unpack stonedata
            if (stonedata) {
                //console.log("Valid Combo result", stonedata);
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
                UpdateStoneSettingRow(i, stn.SettingCost, true);
            }
        }).catch(function (e) {
            console.log("Error retrieving stone matching data", e);
            UpdateStoneSettingRow(i, 0, false);
        });
}

function StoneSizeChanged(stoneRow) { 
    StoneChanged(stoneRow);  
}

function StoneQtyChanged(i) {
    CalcRowTotal("Stones", i);
    UpdateStoneSettingRow(i, null, null);
}

function UpdateStoneSettingRow(stoneRow, settingCost, bValidCombo) {
    // find labor with data-stonerow = stonerow
    var settingRowName = "StoneSetting_" + stoneRow;
    var target = $("#" + settingRowName);
    // if it dowsn't exist, create it - AddStoneSettingRowHTML
    //console.log(`Setting Cost: ${settingCost}`);
    var price, qty;
    var name, shape, size;
    // updates
    // Name - some function of stone name, shape, size
    name = $("#Stones_" + stoneRow + "__Name").val();
    shape = $("#Stones_" + stoneRow + "__ShId").val();
    size = $("#Stones_" + stoneRow + "__SzId").val();

    var stName = $("#StoneSettingName_" + stoneRow).val();
    // Ignore null bValidCombo
    if (bValidCombo === false) {
        stName = "Invalid stone combination";// + (parseInt(stoneRow) + 1); // change to message indicating invalid combo
        $("div[name='StoneSettingRowTotalValue_" + stoneRow + "']").addClass("badTotal");
    }
    if (bValidCombo === true) {
        stName = "Setting for " + name + "-" + shape + "-" + size;
        $("div[name='StoneSettingRowTotalValue_" + stoneRow + "']").removeClass("badTotal");
    }
    $("#StoneSettingName_" + stoneRow).val(stName);
    if (settingCost !== null) {
        $("#StoneSettingPrice_" + stoneRow).val(settingCost.toFixed(2));
    }
    // Qty = stone QTY
    qty = $("#Stones_" + stoneRow + "__Qty").val();
    $("#StoneSettingQty_" + stoneRow).val(qty);

    price = $("#StoneSettingPrice_" + stoneRow).val();
    CalcStonesSettingsRow(stoneRow, price, qty);

}

function AddStoneSettingRowHTML(stoneRow) {
    ltbordered = getStoneSettingsHTML("Stones", len);
    $("#LaborsTotal").before(ltbordered);
    // Don't need this -new rows will always have PPP = 0
    settingVal = 0;
    $("#StoneSettingPrice_" + stoneRow).val(settingVal.toFixed(2));
    price = settingVal;
    UpdateStoneSettingRow(stoneRow, 0, false);
}

function RemoveStoneSettingRow(stoneRow) {
    // find labor for stonerow & hide the enclosing ltbordered
    var target = $("#StoneSetting_" + stoneRow).parent();
    if (target.hasClass("ltbordered")) {
        target.addClass("hidden");
    }
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
                    </div>'
                + leftDelBtn +
                '</div> \
            </div>\
            <input class="col-sm-2 text-box single-line requiredifnotremoved" data-val="true" placeholder="Name" data-val-required="The Metal Style Name field is required." id="Castings_' + len + '__Name" name="Castings[' + len + '].Name" type="text" value="" />\
            JSVENDORS\
            <input class="col-sm-1 text-box single-line requiredifnotremoved" data-val="true" data-val-number="The field Weight field must be a number."  data-val-required="The Weight field is required." id="Castings_' + len + '__MetalWeight" name="Castings[' + len + '].MetalWeight" type="text" value="0.00" onblur="CalcMetalPrice(' + len + ')\"/>\
            JSMETALUNITS\
            JSMETALS\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Price must be a number." id="Castings_' + len + '__Price" name="Castings[' + len + '].Price" type="text" value="0.00" disabled="disabled" onblur="CalcRowTotal(\'' + type + '\, ' + len + ')\"/>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Labor must be a number." id="Castings_' + len + '__Labor" name="Castings[' + len + '].Labor" type="text" value="0.00" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <input class="col-sm-1 text-box single-line requiredifnotremoved" data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Castings_' + len + '__Qty" name="Castings[' + len + '].Qty" type="text" value="0" onblur="CalcMetalPrice(' + len + ')\"/>\
            <div id="CastingsRowTotalValue_' + len + '" class="col-sm-1 CastingsRowTotal ">0.00</div>\
            ' + rightDelBtn + '\
            </div>\
        <div class="row">\
            <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Name" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].MetalWeight" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].MetalWtUnitId" data-valmsg-replace="true"></span>\
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
            <input class="col-sm-1 text-box single-line locked" disabled = "disabled" data-val="true" data-val-number="The Karat Weight must be a number." id="Stones_' + len + '__CtWt" name="Stones[' + len + '].Ctwt" type="text" value="" \"/>\
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
            <input class="col-sm-3 text-box single-line requiredifnotremoved" placeholder="Name" data-val="true" data-val-required="The Labors Name field is required." id="Labors_' + len + '__Name" name="Labors[' + len + '].Name" type="text" value="" />\
            <input class="col-sm-3 text-box single-line" id="Labors_' + len + '__Desc" name="Labors[' + len + '].Desc" type="text" value="" />\
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

function getLaborItemsHTML(type, len) {
    return '\
    <div id="LaborItemsRow_' + len + '" class="LaborItemsRow">\
        <div class="row ltbordered">\
            <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id= "LaborItems_' + len + '__Id" name= "LaborItems[' + len + '].Id" type= "hidden" value= "0" />\
            <input data-val="true" data-val-number="The field linkId must be a number." data-val-required="The linkId field is required." id= "LaborItems_' + len + '__linkId" name= "LaborItems[' + len + '].linkId" type= "hidden" value= "0" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="LaborItemsAddBtn_' + len + '" class="btn btn-default ' + type + 'AddBtn" onclick="AddComponentRow(\'LaborItems\', ' + len + ')">\
                            <span class="glyphicon glyphicon-plus"></span>\
                        </button>\
                    </div>'
        + leftDelBtn +
        '</div>\
            </div>\
            <!--<input class="col-sm-3 text-box single-line requiredifnotremoved" placeholder="Name" data-val="true" data-val-required="The Labors Name field is required." id="LaborItems_' + len + '__Name" name="LaborItems[' + len + '].Name" type="text" value="" />-->\
            <select name="LaborItems[' + len + '].laborItemId" id="LaborItems_' + len + '__laborItemId" data-val-required="Please select a Labor. " data-val-number="The Labor field is required. " data-val="true" class="col-sm-3" onchange="LaborItemsDropdownChanged(' + len + ')">\
                <option id="getoptions" value="LaborTableItems">LOAD OPTIONS HERE</option>\
            </select>\
            <input class="locked col-sm-1 text-box single-line" disabled="disabled" data-val="true" data-val-number="The field $/Hour must be a number." id="LaborItems_' + len + '__pph" name="LaborItems[' + len + '].pph" type="text" value="" />\
            <input class="locked col-sm-1 text-box single-line" disabled="disabled" data-val="true" data-val-number="The field $/Piece must be a number." id="LaborItems_' + len + '__ppp" name="LaborItems[' + len + '].ppp" type="text" value="" />\
            <input class="locked col-sm-2 text-box single-line" disabled="disabled" data-val="true" data-val-number="The field $/Hour must be a number." id="LaborItems_' + len + '__VendorName" name="LaborItems[' + len + '].VendorName" type="text" value="" />\
            <input class="hidden" id="LaborItems_' + len + '__Name" name="LaborItems[' + len + '].Name" type="text" value="FILLER" />\
            <div class="col-sm-1"></div>\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="LaborItems_' + len + '__Qty" name="LaborItems[' + len + '].Qty" type="text" value="0" onblur="CalcRowTotal(\'' + type + '\', ' + len + ')\"/>\
            <div id="LaborItemsRowTotalValue_' + len + '" class="col-sm-1 LaborItemsRowTotal">0.00</div>\
            ' + rightDelBtn + '\
        </div >\
        <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="LaborItems[' + len + '].laborItemId" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="LaborItems[' + len + '].Qty" data-valmsg-replace="true"></span>\
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
            <\input class="col-sm-2 text-box single-line requiredifnotremoved" placeholder="Name" data-val="true" data-val-required="The Misc Name field is required." id="Miscs_' + len + '__Name" name="Miscs[' + len + '].Name" type="text" value="" />\
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

$(window).load(function () {
    JewelryTypeChanged();
});

$(function () { // 
    setAddBtn("Castings");
    setAddBtn("Stones");
    setAddBtn("Findings");
    setAddBtn("Labors");
    setAddBtn("LaborItems");
    setAddBtn("Miscs");
    CalcSubtotals("Castings");
    CalcSubtotals("Stones");
    CalcSubtotals("Findings");
    CalcSubtotals("Labors");
    CalcSubtotals("Miscs");
    CalcTotals();
    

    $.validator.addMethod("requiredifnotremoved", function (value, element) { //--- does this get called?
        var elementId = $(element).attr("id");
        //console.log(`validating ${elementId}`);
        if (elementId === "jssINDEX" || elementId === "jsshINDEX" || elementId === "jsszINDEX" || elementId === "jsfINDEX") {
            return true;
        }
        if ($(element).hasClass("input-validation-error")) {
            $("#vMsg").attr("data-msg", $(element).attr("data-val-required"));
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



