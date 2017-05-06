$('.notice-box').perfectScrollbar();

$(window).load(function () {
    $.post("/Faction/UserDistribution", {}, function (d) {      
        d = d.split('|');       
        $('#Faction-User').children('.faction-1').css('width', d[0] + "%");
        $('#Faction-User').children('.faction-2').css('width', d[1] + "%");
        $('#Faction-User').children('.faction-3').css('width', d[2] + "%");
        $('#Faction-User').children('.faction-4').css('width', d[3] + "%");
        $('#Faction-User').children('.faction-5').css('width', d[4] + "%");
        $('#Faction-User').children('.faction-6').css('width', d[5] + "%");
    })
})