
var Advice = true;
function HideAdvice(x) {
    $('.advice').each(function () {
        if (Advice == true) {
            $(this).fadeOut();
        }
        else {
            $(this).fadeIn();
        }
    })
    if (Advice == true) {
        Advice = false;
        $('#Advice-Btn').val("Show");
    }
    else {
        Advice = true;
        $('#Advice-Btn').val("Hide");
    }
}