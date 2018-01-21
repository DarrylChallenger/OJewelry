/* activate and deactivate controls based on radio button selection */

// en/disable controls based on radio button values
$(function () {
    console.log("Page is ready");
    setSendReturnControls();
});

// en/disable controls on click events
function setNewExistingControls() {
    console.log("setNewExistingControls");
    var radioButtons = document.getElementsByName("NewExistingPresenterRadio");
    //if the presenters dropdown is empty, select the New Presenters radio button
    if (document.getElementById("PresenterId").options.length === 0)
    {
        // check the new presenter radio, disable the existing
        console.log("Empty Presenter list");
        radioButtons[1].checked = true;
        radioButtons[0].disabled = true;
    }
    if (radioButtons[0].checked) {
        console.log("Existing Presenter checked.");
        // enable existing presenter controls (dropdown)
        enableChildren(document.getElementById("StyleMemoPresentersGroup"));
        // disable new presenter controls
        disableChildren(document.getElementById("StyleMemoExistingGroup"));
    }
    if (radioButtons[1].checked) {
        console.log("New Presenter checked.");
        // enable  new presenter controls
        enableChildren(document.getElementById("StyleMemoExistingGroup"));
        // disable existing presenter controls (dropdown)
        disableChildren(document.getElementById("StyleMemoPresentersGroup"));
    }
}

function setSendReturnControls() {
    console.log("setSendReturnControls");
    var radioButtons = document.getElementsByName("SendReturnMemoRadio");
    if (radioButtons[0].checked)
    {
        console.log("Memo checked.");
        // enable memo controls
        enableChildren(document.getElementById("StyleMemoMemoGroup"));
        // disable return controls
        disableChildren(document.getElementById("StyleMemoReturnGroup"));
        setNewExistingControls();
    }
    if (radioButtons[1].checked) {
        console.log("Return checked.");
        // enable return controls
        enableChildren(document.getElementById("StyleMemoReturnGroup"));
        // disable memo controls
        disableChildren(document.getElementById("StyleMemoMemoGroup"));
    }
}

function enableChildren(node)
{
    console.log(Date.now() + " Enable " + node.innerHTML);
    var allChildNodes = node.getElementsByTagName("*");
    for (var i = 0; i < allChildNodes.length; i++) {
        allChildNodes[i].disabled = false;
    }
}

function disableChildren(node)
{
    console.log(Date.now() + " Disable " + node.innerHTML);
    var allChildNodes = node.getElementsByTagName("*");
    for (var i = 0; i < allChildNodes.length; i++) {
        allChildNodes[i].disabled = true;
    }
}