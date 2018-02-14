

function AddComponentRow(type, index)
{
    console.log("Add " + type + "[" + index + "]")
    str = $(".styleCastings").find("#CastingRowTotalValue_0").html()
    len = $(".styleCastings").find(".CastingRowTotal").length
    console.log("Casting:" + len )
    str = $(".styleFindings").find("#FindingRowTotalValue_0").html()
    len = $(".styleFindings").find(".FindingRowTotal").length 
    console.log("Finding:" + len )
    // for each type iterate thru to find total number of entries

}

function RemoveComponentRow(type, i)
{
    console.log("Remove " + type + "[" + i + "] - (#" + type + "Row_" + i + ")")
    $("#"+type+"Row_" + i).remove()
}

function CalcTotals()
{
    console.log("Calc Totals")
    // Iterate through each type and compute the row total
    // Iterate thru each total to get the grand total
}