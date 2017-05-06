var crossWordData = {
    X: 0, Y: 0, TotalSquares:0,
    SquareLength: 0, Instructions: 0,
    Q: [], Name: "",
};
function thisQuestion() {
    return {
        'Word': '',
        'Clue': '',
        'Direction': '',
        'Number': 0,
        'Position': 0,
        'WordLength': 0
    };
};

//Create Grid
$('#Grid-Select').change(function () {
    createGrid();
    $('#Create-CrossWord').fadeIn();
})
function createGrid(x) {
    var value = "10x10";    
    if (x != "auto") { value = $('#Grid-Select').val(); }
    $('.cross-word-box').html("");

    crossWordData.X = parseInt(value.split('x')[0]);
    crossWordData.Y = parseInt(value.split('x')[1]);
    crossWordData.TotalSquares = parseInt(crossWordData.X) * parseInt(crossWordData.Y);
    crossWordData.SquareLength = getLength(crossWordData.X);

    for (i = 0; i < crossWordData.TotalSquares; i++) {
        $('.cross-word-box').append("<div id='Box-Number-" + parseInt(i + 1) + "' class='single-box' onclick='placeWord(event, this)'>" + parseInt(i + 1) + "<p class='single-letter'></p></div>");
    }
    $('.single-box').css('width', crossWordData.SquareLength + "px");
    $('.single-box').css('height', crossWordData.SquareLength + "px");
    $('.single-letter').css('font-size', parseInt(getLength(crossWordData.X)) / 4 + "px");
    $('.single-letter').css('line-height', crossWordData.SquareLength + "px");
}
function getLength(x) {   
    return (parseInt($('.cross-word-box').css('width')) / parseInt(x));
}
//

function placeWord(e, box) {   
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
        $('.placement-ui-box').css({
            "position": "absolute",
            "left": x,
            "top": y
        });
    }
    else {
        //Mobile placement
        $('.placement-ui-box').css({
            "position": "fixed",
            "left": "40px",
            "top": "20px"
        });
    }
    $('.placement-ui-box').fadeIn();
    question = thisQuestion();
    question.Position = $(box).attr('id').split('-')[2];
    //Empty all boxes
    EmptyInputs();
}
function EmptyInputs() {
    $('#Clue').val("");
    $('#Word').val("");
    $('#Direction').val("");
    $('#Warning').html("");
}
$('#UI-Placement-Close').click(function () {
    $('.placement-ui-box').fadeOut();
   
});

$('#Insert').click(function () {
    var word = $('#Word').val();
    var clue = $('#Clue').val();
    var direction = $('#Direction').val();
    $('#Warning').html("");
    if (word == "" | clue == "" | direction == "") {
        $('#Warning').html("Please fill in the needed information");
        $('.placement-ui-box').scrollTop(0);
    }
    else {
        question.Clue = clue;
        question.Word = word;
        question.Direction = direction;
        question.WordLength = word.length;
        question.Number = crossWordData.Q.length;
        //Add to crossWordData
        crossWordData.Q.push(question);
        //finish insert functions
        removeInstruction();
        writeWord();
        WriteInstruction();       
    }
})

function removeInstruction() {
    $('.placement-ui-box').fadeOut();
}
function writeWord(){
    start = $('#Box-Number-' + question.Position);
    var boxspace = 1;
    if (question.Direction == "Down") {
        boxspace = getSpacing();
    }
    for (var i = 0; i < question.Word.length; i++) {
        if (i == 0) {
            var madeError = checkBoxForDuplicateLetter($('#Box-Number-' + parseInt(question.Position)), question.Word[i]);
            if (madeError == false) {
                $('#Box-Number-' + parseInt(question.Position)).children('.single-letter').html(question.Word[i]);
            }
            else {

            }
        }
        else {
            var next = parseInt(i * boxspace) + parseInt(question.Position);
            var madeError = checkBoxForDuplicateLetter($('#Box-Number-' + next), question.Word[i]);
            if (madeError == false) {
                $('#Box-Number-' + next).children('.single-letter').html(question.Word[i]);
            }
            else {

            }
        }
    }
}
function checkBoxForDuplicateLetter(thisBox, thisLetter) {
    if ($(thisBox).children('.single-letter').html().length > 0) {
        if ($(thisBox).children('p.single-letter').html() != thisLetter) {
            boxUsageChange(thisBox, 'invalid-box');
            $('#warning-message').html("Please sort out the conflicts before carrying on");
            return true;
        }
        else if ($(thisBox).children('p.single-letter').html() == thisLetter) {
            boxUsageChange(thisBox, 'valid-box');
            return true;
        }
    }
    return false
}
function getSpacing() {
    return crossWordData.X;
}
function WriteInstruction() {
    html = "<div id='Instruction-Number-" + question.Number + "'>"
        + "<p>" + question.Number + ". " + question.Direction + "</p>"
        + "<p>" + question.Clue + "</p> "
        + "<input type='button' class='form-control submit-button' value='change' id='Change-Instruction-" + question.Number + "' onclick='alter(event, this)'/>"
        + "</div>"
        + "<hr/>";
    $('#Instruction-List').append(html);
}



$('#Word').keyup(function (e) {
    //if not space
    if (e.which != 32) {
        $('.single-box').each(function () {
            if (!$('.single-box').hasClass('invalid-box')) {
                $('.single-box').removeClass('used-box');
            }
        });
        var boxspace = 1;
        if (question.Direction == "Down") {
            boxspace = getSpacing();
        }
        for (var i = 0; i < $('#Word').val().length; i++) {
            if (i == 0) {
                boxUsageChange('#Box-Number-' + parseInt(question.Position), 'used-box');
            }
            else {
                var next = parseInt(i * boxspace) + parseInt(question.Position);
                boxUsageChange('#Box-Number-' + next, 'used-box');
            }
        }
    }
    else {
        $(this).val($(this).val().replace(" ", ""));
    }
})


$('#Direction').change(function () {
    question.Direction = $('#Direction').val();
})



function boxUsageChange(toAdd, toThis){
    $(toAdd).attr('class', 'single-box ' + toThis);
}


$('#CrossWord-Name').keyup(function() {
    crossWordData.Name = $(this).val();
})