
function AddComponentRow(type, index)
{
    // " + len +  is the row that was pressed
    console.log("Add " + type + "[" + index + "]")
    idHeaderBtn = type + "AddBtn";
    idRowBtn = type + "AddBtn_" + index;

    // Remove current '+' on the currently selected row
    if (index === -1) {
        // hide the '+' on the header...
        $("#" + idHeaderBtn).addClass("hidden")
    }
    else { // ... or the button
        $("#" + idRowBtn).addClass("hidden")
    }

    stateClassName = type + "State"
    stateClass = "." + stateClassName
    len = $(stateClass).length

    // get dropdpwns for vendors, metals, stones, findings; reset components values on select; totals

    /* Castings */
    var castingsltbordered = '      <div id="CastingsRow_' + len + '" class="CastingsRow">                              <div class="row ltbordered"> \
        <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id= "Castings_' + len + '__Id" name= "Castings[' + len + '].Id" type= "hidden" value= "" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn "> \
                    <div class="col-sm-6 "> \
                        <button type="button" id="CastingsAddBtn_' + len + '" class="btn btn-default '+ type +'AddBtn" onclick="AddComponentRow(\'Castings\', ' + len + ')"> \
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
            <div class="col-sm-1"></div>\
            <select class="col-sm-2" data-val="true" data-val-number="The field VendorId must be a number." data-val-required="The VendorId field is required." id="Castings_' + len + '__VendorId" name="Castings[' + len + '].VendorId">\
                <option selected="selected" value="1">V1</option>\
                <option value="2">V2</option>\
            </select>\
            <select class="col-sm-1" data-val="true" data-val-number="The field MetalCodeId must be a number." data-val-required="The MetalCodeId field is required." id="Castings_' + len + '__MetalCodeId" name="Castings[' + len + '].MetalCodeId">\
                <option value="1">G     </option>\
                <option value="2">S     </option>\
                <option selected="selected" value="3">P     </option>\
            </select>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Price must be a number." id="Castings_' + len + '__Price" name="Castings[' + len + '].Price" type="text" value="0.00" />\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Labor must be a number." id="Castings_' + len + '__Labor" name="Castings[' + len + '].Labor" type="text" value="0.00" />\
            <input class="col-sm-1 " data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Castings_' + len + '__Qty" name="Castings[' + len + '].Qty" type="text" value="0" />\
            <div id="CastingsRowTotalValue_' + len + ' class="col-sm-2 CastingsRowTotal ">0.00</div>\
                              </div>      \
                                    <div class="row">\
        <!--Validations Here-->\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Name" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Price" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Labor" data-valmsg-replace="true"></span>\
            <span class="field-validation-valid text-danger" data-valmsg-for="Castings[' + len + '].Qty" data-valmsg-replace="true"></span>\
                                    </div ></div >'

    /* Stones */
    var stonesltbordered = '<div id="StonesRow_' + len + '"  class="StonesRow">                                <div class="row ltbordered">\
        <input data- val="true" data- val - number="The field Id must be a number." data- val - required="The Id field is required." id= "Stones_0__Id" name= "Stones[0].Id" type= "hidden" value= "1" />\
            <input id="Stones_0__Name" name="Stones[0].Name" type="hidden" value="Duvel Hops" /> <!--should be dropdown -->\
                <input data-val="true" data-val-number="The field ComponentTypeId must be a number." data-val-required="The ComponentTypeId field is required." id="Stones_0__ComponentTypeId" name="Stones[0].ComponentTypeId" type="hidden" value="2" />\
                    <div class="col-sm-1 ">\
                    <div class="row StyleComponentsRowHeaderBtn ">\
                        <div class="col-sm-6 ">\
                            <button type="button" id="StonesAddBtn_' + len + '" class="btn btn-default ' + type +'AddBtn" onclick="AddComponentRow(\'Stones\', ' + len + ')">\
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
                <select class="col-sm-2" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="Stones_0__Comp_Id" name="Stones[0].Comp.Id">\
                    <option selected="selected" value="1">Duvel Hops</option>\
                    <option value="2">Duvel Barley</option>\
                    <option value="1009">junkStone</option>\
                </select>\
                <div class="col-sm-1 "></div>\
                <div class="col-sm-2 ">V2 </div>\
                <div class="col-sm-1 ">2 </div>\
                <div class="col-sm-1 "> </div>\
                <div class="col-sm-1 ">0.0000 </div>\
                <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Stones_0__Qty" name="Stones[0].Qty" type="number" value="0" />\
                <div id="StonesRowTotalValue_0" class="col-sm-2 StonesRowTotal ">0.00</div>\
                                </div ></div>'

    /* Findings */
    findingsltbordered = '<div id="FindingsRow_' + len + '"  class="FindingsRow">                                 <div class="row ltbordered">\
        <input data- val="true" data- val - number="The field Id must be a number." data- val - required="The Id field is required." id= "Findings_' + len + '__Id" name= "Findings[' + len + '].Id" type= "hidden" value= "4" />\
            <input id="Findings_' + len + '__Name" name="Findings[' + len + '].Name" type="hidden" value="Duvel Yeast 1" />\
            <input data-val="true" data-val-number="The field ComponentTypeId must be a number." data-val-required="The ComponentTypeId field is required." id="Findings_' + len + '__ComponentTypeId" name="Findings[' + len + '].ComponentTypeId" type="hidden" value="3" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="FindingsAddBtn_' + len + '" class="btn btn-default ' + type +'AddBtn" onclick="AddComponentRow(\'Findings\', ' + len + ')">\
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
            <select class="col-sm-2" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="Findings_' + len + '__Comp_Id" name="Findings[' + len + '].Comp.Id">\
                <option selected="selected" value="4">Duvel Yeast 1</option>\
                <option value="6">Duvel Yeast 2</option>\
                <option value="1010">junkFinding</option>\
                <option value="2008">aTestFinding</option>\
            </select>\
            <div class="col-sm-2 "></div>\
            <div class="col-sm-2 ">V2 </div>\
            <div class="col-sm-1">G     </div>\
            <div class="col-sm-1">0.0000</div>\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Quantity must be a number." data-val-required="The Quantity field is required." id="Findings_' + len + '__Qty" name="Findings[' + len + '].Qty" type="number" value="0" />\
            <div id="FindingsRowTotalValue_' + len + ' class="col-sm-2 FindingsRowTotal ">0.00</div>\
                                </div ></div > '

    /* Labors */
    laborsltbordered = '<div id="LaborsRow_' + len + '" class="LaborsRow">                                <div class="row ltbordered">\
        <input data- val="true" data- val - number="The field Id must be a number." data- val - required="The Id field is required." id= "Labors_' + len + '__Id" name= "Labors[' + len + '].Id" type= "hidden" value= "1" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="LaborsAddBtn_' + len + '" class="btn btn-default ' + type +'AddBtn" onclick="AddComponentRow(\'Labors\', ' + len + ')">\
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
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Hour must be a number." id="Labors_' + len + '__PPH" name="Labors[' + len + '].PPH" type="text" value="0.00" />\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Piece must be a number." id="Labors_' + len + '__PPP" name="Labors[' + len + '].PPP" type="text" value="0.00" />\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Quantity must be a number." id="Labors_' + len + '__Qty" name="Labors[' + len + '].Qty" type="number" value="0" />\
            <div id="LaborsRowTotalValue_' + len + ' class="col-sm-2 LaborsRowTotal">0.00</div>\
                                </div > </div > '

    /* Miscs */
    miscsltbordered = '<div id="MiscsRow_' + len + '"  class="MiscsRow">                                <div class="row ltbordered">\
        <input data- val="true" data- val - number="The field Id must be a number." data- val - required="The Id field is required." id= "Miscs_' + len + '__Id" name= "Miscs[' + len + '].Id" type= "hidden" value= "1" />\
            <div class="col-sm-1 ">\
                <div class="row StyleComponentsRowHeaderBtn ">\
                    <div class="col-sm-6 ">\
                        <button type="button" id="MiscsAddBtn_' + len + '" class="btn btn-default ' + type +'AddBtn" onclick="AddComponentRow(\'Miscs\', ' + len + ')">\
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
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field $/Piece must be a number." id="Miscs_' + len + '__PPP" name="Miscs[' + len + '].PPP" type="text" value="0.00" />\
            <input class="col-sm-1 text-box single-line" data-val="true" data-val-number="The field Quantity must be a number." id="Miscs_' + len + '__Qty" name="Miscs[' + len + '].Qty" type="number" value="0.00" />\
            <div id="MiscsRowTotalValue_' + len + ' class="col-sm-2 MiscsRowTotal">0.00</div>\
    </div > </div>'

    idTotalName = type + "Total"
    idTotal = "#" + idTotalName
    console.log("class:" + stateClass + ", id:" + idTotal)
    classhtml = $(stateClass).html()
    var hiddenState = "<input name='" + type + "[' + len + '].SVMState' id='" + type + "_' + len + '__SVMState' type='hidden' value='Added' data-val-required='The SVMState field is required.' data-val='true'>"
    var newState = $("<div class='" + stateClassName + "'></div>").append(hiddenState) //.append hiddden state input
    if (type == "Castings") {
        ltbordered = castingsltbordered.replace(/INDEX/g, len)
    }
    if (type == "Stones") {
        ltbordered = stonesltbordered.replace(/INDEX/g, len)
    }
    if (type == "Findings") {
        ltbordered = findingsltbordered.replace(/INDEX/g, len)
    }
    if (type == "Labors") {
        ltbordered = laborsltbordered.replace(/INDEX/g, len)
    }
    if (type == "Miscs") {
        ltbordered = miscsltbordered.replace(/INDEX/g, len)
    }
    console.log(ltbordered)
    //castingsltbordered.find("INDEX").replaceAll().len
    var str = newState.add(ltbordered)
    //$(idTotal).before(newState).before(newRow)
    $(idTotal).before(str)
    //console.log($("#CastingsRow_INDEX:contains('INDEX')").text(len))
}

function RemoveComponentRow(type, i)
{
    console.log("Type:"+type)
    console.log("Remove " + type + "[" + i + "]")

    var str = "#" + type + "Row_" + i
    idHeaderBtn = type + "AddBtn";
    hide = $(str).addClass("hidden")
    btnClass = "." + type + "AddBtn"
    rowClass = "." + type + "Row"
    styleClass = ".style" + type 
    
    console.log(str, btnClass)
    console.log(hide)
    
    // Put the '+' back, find last the !Deleted row. If none, show the header
    // find the  visible rows with a visible add btn
    str = $(styleClass).find(rowClass).not(".hidden").find(btnClass).not(".hidden")
    // if there are none, put the '+' on the last visible btn
    console.log("str", str)
    if (str.length == 0) {
        // set the last visible row, if any...
        var rem = $(rowClass).not(".hidden").last().find(btnClass)
        if (rem.length != 0) {
            rem.removeClass("hidden")
        } else {
        // ... if no visible row, set the header
            var rem = $("#" + idHeaderBtn)
            console.log("rem", rem)
            rem.removeClass("hidden")
        }
    }
}

function CalcTotals()
{
    console.log("Calc Totals")
    // Iterate through each type and compute the row total
    // Iterate thru each total to get the grand total
}


