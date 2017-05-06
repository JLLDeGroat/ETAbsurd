$(".thread-box").mCustomScrollbar();
$(".blog-box").mCustomScrollbar();
$(".puzzle-box").mCustomScrollbar();
$('.crossword-box').mCustomScrollbar();

$('.karma-bar').each(function () {
    $(this).css('width', $(this).html());
})