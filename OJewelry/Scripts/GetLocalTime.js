function GetLocalTime(companyId, hr, id) {
    if (id === undefined) id = "ExportLink";
    var s = `${hr}?CompanyId=${companyId}&sCurrDate=${new Date(Date.now()).toLocaleString()}`;
    console.log(`s:[${s}], id: [${id}]`);
    $(`.${id}`).attr("href", s);
    return true;
}