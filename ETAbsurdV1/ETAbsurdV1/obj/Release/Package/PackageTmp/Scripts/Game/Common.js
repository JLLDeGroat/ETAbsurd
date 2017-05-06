//Loading
function StartLoad() {
    $('.progress').css('width', "5%");
    $('.progress').animate({
        width: "100%",
    }, 3000, function () {
        $('.loader').css('border-left', '1px solid #0E8C3A');
        $('.loader').css('border-top', '1px solid #0E8C3A');
        $('.progress').addClass('progress-loaded');

        LoadPart();
    });
}
function LoadPart() {
    $('#load-box').animate({
        opacity: 1,
    }, 1500, function () {
        $('#load-box').animate({
            opacity: 0,
        }, 1500, function () {
            $('#load-box').fadeOut();
        });
    });
}


//Text Write
(function ($) {
    $.fn.writeText = function (content) {
        var contentArray = content.split(""),
            current = 0,
            elem = this;
        setInterval(function () {
            if (current < contentArray.length) {
                elem.text(elem.text() + contentArray[current++]);
            }
            else if(current >= contentArray.length)
            {
                return;
            }
        }, 50);
    };

})(jQuery);