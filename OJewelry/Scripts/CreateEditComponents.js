/* activate and deactivate controls based on radio button selection */

// Show/gide controls based on dropdown value
$(function () {
    resetControls();
    console.log("Page is ready");
});

function resetControls() {
    console.log("resetControls");
    var sel = document.getElementById("ComponentTypeId").selectedIndex;
    var ctl;
    switch (sel) {
        case 0: // Metal
            console.log("Metal");
            document.getElementById("ecVendorGroup").style.display = "block";
            document.getElementById("ecPriceGroup").style.display = "block";
            document.getElementById("ecPPHGroup").style.display = "none";
            document.getElementById("ecPPPGroup").style.display = "none";
            document.getElementById("ecMetalMetalGroup").style.display = "block";
            document.getElementById("ecMetalLaborGroup").style.display = "block";
            document.getElementById("ecCtwtGroup").style.display = "none";
            document.getElementById("ecStoneSizeGroup").style.display = "none";
            document.getElementById("ecStonePPCGroup").style.display = "none";
            document.getElementById("ecFindingsMetalGroup").style.display = "none";
            break;
        case 1: // Stones
            console.log("Stones");
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
           break;
        case 2: // Findings
            console.log("Findings");
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
            break;
        case 3: // Misc
            console.log("Misc");
            document.getElementById("ecVendorGroup").style.display = "none";
            document.getElementById("ecPriceGroup").style.display = "none";
            document.getElementById("ecPPHGroup").style.display = "block";
            document.getElementById("ecPPPGroup").style.display = "block";
            document.getElementById("ecMetalMetalGroup").style.display = "none";
            document.getElementById("ecMetalLaborGroup").style.display = "none";
            document.getElementById("ecCtwtGroup").style.display = "none";
            document.getElementById("ecStoneSizeGroup").style.display = "none";
            document.getElementById("ecStonePPCGroup").style.display = "none";
            document.getElementById("ecFindingsMetalGroup").style.display = "none";
            break;
        case 4: // Misc
            console.log("Misc");
            document.getElementById("ecVendorGroup").style.display = "none";
            document.getElementById("ecPriceGroup").style.display = "block";
            document.getElementById("ecPPHGroup").style.display = "none";
            document.getElementById("ecPPPGroup").style.display = "none";
            document.getElementById("ecMetalMetalGroup").style.display = "none";
            document.getElementById("ecMetalLaborGroup").style.display = "none";
            document.getElementById("ecCtwtGroup").style.display = "none";
            document.getElementById("ecStoneSizeGroup").style.display = "none";
            document.getElementById("ecStonePPCGroup").style.display = "none";
            document.getElementById("ecFindingsMetalGroup").style.display = "none";
            break;
}
    console.log(sel);
}

function showControl() {
}

function hideControl() {
}
