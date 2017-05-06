
if (window.innerWidth >= 980) {
    $('.game-icon').each(function () {
        var width = $(this).css('width');
        $(this).css('height', width);
    })
}
else {
    $('.game-icon').each(function () {        
        $(this).css('height', "50px");
    })
}


//Clicks

$('#RD').click(function () {
    window.open('/Game/Rd');
})