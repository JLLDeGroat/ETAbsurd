
window.onload = function () {
    var gridX = $('#Grid-Size').html().split('X')[0];
    $('.single-box').css('width', getLength(gridX) + "px");
    $('.single-box').css('height', getLength(gridX) + "px");
    $('.box-letter').css('line-height', getLength(gridX) + "px");
    $('.box-letter').css('font-size', parseInt(getLength(gridX)) / 4 + "px");
    
}
function getLength(x) {
    return ((parseInt($('.cross-word-box').css('width')) - 12)/ parseInt(x));
}

var CrossWordInput = {
    Position: 0,
    Letter: ""
}
$('.box-letter').click(function () {
    $("#Enter-Letter-Box").removeClass('hdn');
    $('.letter-input').focus();

    CrossWordInput.Position = $(this).attr('id').split('-')[2];
})

$('.letter-input').keyup(function () {
    CrossWordInput.Letter = $(this).val();
    $('#Enter-Letter-Box').addClass('hdn');

    $('#Box-Letter-' + CrossWordInput.Position).html(CrossWordInput.Letter.toUpperCase().substring(0,1));

    //Reset
    CrossWordInput = {
        Position: 0,
        Letter: ""
    }
    $('.letter-input').val('');
})


function getAnswers() {
    var id = "";
    var letter = "";
    var CW = window.location.href.split('/');
    CW = CW[CW.length - 1];
    $('.box-letter').each(function () {
        if ($(this).html() != "") {
            id += $(this).attr('id').split('-')[2] + ",";
            letter += $(this).html() + ",";
        }
    });
    console.log(id);
    console.log(letter);
    console.log(CW);
    $.post("/Puzzles/CrossWordAnswers", { Positions: id, Characters: letter, Id: CW,}, function (data) {
        console.log(data);
        var positionsCorrect = data.split(',');
        for (var i = 0; i < positionsCorrect.length; i++) {
            $('#Box-Letter-' + positionsCorrect[i]).parent('.single-box').addClass('correct');
            setTimeout(function () {
                $('#Box-Letter-' + positionsCorrect[i]).parent('.single-box').removeClass('correct');
            }, 3000);
        }
    })
}