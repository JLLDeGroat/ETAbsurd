//Get Faction
$.post("/Login/NavigationFaction", {}, function (data) {
    if (data != "Fail") {
        $('#Faction-Navigation-Button').css('display', 'table-cell');
        $('#Faction-Navigation-Faction').html(data);
    }
    else {
        $('#Faction-Navigation-Button').remove();
    }
})