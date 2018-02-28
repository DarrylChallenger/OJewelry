/* activate and deactivate controls based on radio button selection */

// Show/gide controls based on dropdown value
$(function () {
    resetControls();
    console.log("Page is ready");
});

function resetControls() {
    console.log("resetControls");
    var sel = $("#ComponentTypeId option:selected").text();
    var ctl;
    if (sel === "Stones") {
        document.getElementById("ecVendorGroup").style.display = "block";
        document.getElementById("ecPriceGroup").style.display = "none";
        document.getElementById("ecPPHGroup").style.display = "none";
        document.getElementById("ecPPPGroup").style.display = "none";
        document.getElementById("ecMetalMetalGroup").style.display = "none";
        document.getElementById("ecMetalLaborGroup").style.display = "none";
        document.getElementById("ecCtwtGroup").style.display = "block";
        document.getElementById("ecStoneSizeGroup").style.display = "block";
        document.getElementById("ecStonePPCGroup").style.display = "block";
        document.getElementById("ecFindingsMetalGroup").style.display = "none";
    }
    if (sel === "Findings") {
        document.getElementById("ecVendorGroup").style.display = "block";
        document.getElementById("ecPriceGroup").style.display = "block";
        document.getElementById("ecPPHGroup").style.display = "none";
        document.getElementById("ecPPPGroup").style.display = "none";
        document.getElementById("ecMetalMetalGroup").style.display = "block";
        document.getElementById("ecMetalLaborGroup").style.display = "none";
        document.getElementById("ecCtwtGroup").style.display = "none";
        document.getElementById("ecStoneSizeGroup").style.display = "none";
        document.getElementById("ecStonePPCGroup").style.display = "none";
        document.getElementById("ecFindingsMetalGroup").style.display = "none";
    }
    if (sel !== "Stones" && sel !== "Findings") {
        console.log("Illegal componentent type: " + sel);
        document.getElementById("ecVendorGroup").style.display = "none";
        document.getElementById("ecPriceGroup").style.display = "none";
        document.getElementById("ecPPHGroup").style.display = "none";
        document.getElementById("ecPPPGroup").style.display = "none";
        document.getElementById("ecMetalMetalGroup").style.display = "none";
        document.getElementById("ecMetalLaborGroup").style.display = "none";
        document.getElementById("ecCtwtGroup").style.display = "none";
        document.getElementById("ecStoneSizeGroup").style.display = "none";
        document.getElementById("ecStonePPCGroup").style.display = "none";
        document.getElementById("ecFindingsMetalGroup").style.display = "none";
    }
}

function showControl() {
}

function hideControl() {
}
