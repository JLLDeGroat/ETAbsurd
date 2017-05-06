$(window).scroll(function () {
    if ($(window).scrollTop() >= 52) {
        $('.navbar').fadeOut("fast");
    }
    if ($(window).scrollTop() <= 51) {
        $('.navbar').fadeIn("slow");
    }
});
