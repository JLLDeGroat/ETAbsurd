var toAlter = [];
function alter(e, x) {
    var numberToAlter = $(x).attr('id').split('-')[2];
    var x = e.pageX + 'px';
    var y = e.pageY + 'px';

    var fullWidth = $(document).width();
    var fullHeight = $(document).height();
    if (fullWidth > 650) {
        if (e.pageX + 300 >= fullWidth) {
            x = (e.pageX - 300) + 'px';
        }
        if (e.pageY + 300 >= fullHeight) {
            y = (e.pageY - 300) + 'px';
        }

        $('#UI-Alter-Box').css({
            "position": "absolute",
            "left": x,
            "top": y
        });
    }
    $('#UI-Alter-Box').css({
        "position": "fixed",
        "left": "40px",
        "top": "20px"
    });
    $('.alter-ui-box').fadeIn();


    for (i = 0; i < crossWordData.Q.length; i++) {
        if (numberToAlter == crossWordData.Q[i].Number) {
            toAlter = crossWordData.Q[i];
        }
    }
    console.log(toAlter);


    $('#AlterClue').val(toAlter.Clue);
    $('#AlterWord').val(toAlter.Word);
    $('#AlterDirection').val(toAlter.Direction);

    //delete the text already on the page thats to do with this altered object
    var boxspace = 1;
    if (toAlter.Direction == "Down") {
        boxspace = getSpacing();
    }
    for (var i = 0; i < toAlter.Word.length; i++) {
        if (i == 0) {
            if ($('#Box-Number-' + parseInt(toAlter.Position)).attr('class') != 'single-box invalid-box') {
                $('#Box-Number-' + parseInt(toAlter.Position)).children('.single-letter').html("");
                $('#Box-Number-' + parseInt(toAlter.Position)).css('background-color', 'white');
            }
        }
        else {
            var next = parseInt(i * boxspace) + parseInt(toAlter.Position);
            if ($('#Box-Number-' + next).attr('class') == 'single-box invalid-box') {
            }
            else {
                $('#Box-Number-' + next).children('.single-letter').html("");
                $('#Box-Number-' + next).css('background-color', 'white');;
            }
        }
    }
}

$('#AlterInsert').click(function () {
    var word = $('#AlterWord').val();
    var clue = $('#AlterClue').val();
    var direction = $('#AlterDirection').val();
    $('#Warning').html("");
    if (word == "" | clue == "" | direction == "") {
        $('#AlterWarning').html("Please fill in the needed information");
        $('.alter-ui-box').scrollTop(0);
    }
    else {
        for (var i = 0; i < crossWordData.Q.length; i++) {
            if (crossWordData.Q[i].Number == toAlter.Number) {
                crossWordData.Q[i].Clue = clue;
                crossWordData.Q[i].Word = word;
                crossWordData.Q[i].Direction = direction
                crossWordData.Q[i].WordLength = word.length;

                removeAlterInstruction();
                writeAlterWord(crossWordData.Q[i].Word);
                alterInstruction();
            }
        }
        //finish insert functions        
    }
})

function removeAlterInstruction() {
    $('.alter-ui-box').fadeOut();
}
function writeAlterWord(thisWord) {
    start = $('#Box-Number-' + toAlter.Position);
    var boxspace = 1;
    if (toAlter.Direction == "Down") {
        boxspace = getSpacing();
    }
    for (var i = 0; i < thisWord.length; i++) {
        console.log(thisWord);
        if (i == 0) {
            var madeError = checkBoxForDuplicateAlterLetter($('#Box-Number-' + parseInt(toAlter.Position)), thisWord[i]);
            if (madeError == false) {
                console.log("it writes this " + thisWord[i]);
                $('#Box-Number-' + parseInt(toAlter.Position)).children('.single-letter').html(thisWord[i]);
            }
            else {

            }
        }
        else {
            var next = parseInt(i * boxspace) + parseInt(toAlter.Position);
            var madeError = checkBoxForDuplicateAlterLetter($('#Box-Number-' + next), thisWord[i]);
            if (madeError == false) {
                $('#Box-Number-' + next).children('.single-letter').html(thisWord[i]);
            }
            else {

            }
        }
    }
}
function checkBoxForDuplicateAlterLetter(thisBox, thisLetter) {  
    if ($(thisBox).children('p.single-letter').html().length > 0) { 
        if ($(thisBox).children('p.single-letter').html() != thisLetter) {
            $(thisBox).addClass('invalid-box');
            $('#warning-message').html("Please sort out the conflicts before carrying on");
            return true;
        }
        else if ($(thisBox).children('p.single-letter').html() == thisLetter) {
            $(thisBox).addClass('valid-box');
            $(thisBox).removeClass('invalid-box');
            return true;
        }
    }  
    return false
    
}
function alterInstruction() {
    html = "<p id='Instruction-Number-" + toAlter.Number + "'>" + toAlter.Number + ". " + toAlter.Direction + "</p>"
        + "<p>" + question.Clue + "</p> "
        + "<input type='button' class='form-control submit-button' value='change' id='Change-Instruction-" + toAlter.Number + "' onclick='alter(event, this)'/>"
        + "<hr/>";
    $('#Instruction-Number-' + toAlter.Number).html(html);
}



$('#AlterWord').keyup(function (e) {
    //if not space
    if (e.which != 32) {
        $('.single-box').each(function () {
            if (!$('.single-box').hasClass('invalid-box')) {
                $('.single-box').removeClass('used-box');
            }
        });
        var boxspace = 1;
        if (toAlter.Direction == "Down") {
            boxspace = getSpacing();
        }
        for (var i = 0; i < $('#AlterWord').val().length; i++) {
            if (i == 0) {
                $('#Box-Number-' + parseInt(toAlter.Position)).addClass('used-box');
            }
            else {
                var next = parseInt(i * boxspace) + parseInt(toAlter.Position);
                $('#Box-Number-' + next).addClass('used-box');
            }
        }
    }
    else{
        $(this).val($(this).val().replace(" ", ""));
    }
})