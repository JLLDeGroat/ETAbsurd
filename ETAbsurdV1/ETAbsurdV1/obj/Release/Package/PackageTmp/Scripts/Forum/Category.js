//when window changes below 655px so that the text changes on votes

$(window).resize(function () {
    if ($(window).width() <= 655) {
        $('.vote-title').each(function () {
            var firstChar = this.innerHTML.substr(0, 1);
            this.innerHTML = firstChar;
        });
    }
});

$('#forum-search-refine-date').change(function () {
    $('#SearchDate').val(this.value);
})
var advancedActive = false;
$('#Advance-Refinement').click(function () {
    if (advancedActive == false) {
        makeActive(advancedActive);
        advancedActive = true;
    }
    else if (advancedActive == true) {
        makeActive(advancedActive);
        advancedActive = false;
    }
})

function makeActive(a) {
    console.log(a);
    if (a == false) {
        $('.advanced-option').each(function () {
            $(this).css('display', 'block');
            $(this).animate({
                height: 140,
                opacity: 1,
            }, 250, function () { });
        })
    }
    else if (a == true) {
        $('.advanced-option').each(function () {
            $(this).animate({
                height: 0,
                opacity: 0,
            }, 500, function () {
                $(this).css('display', 'none');
            })
        });
    }
}