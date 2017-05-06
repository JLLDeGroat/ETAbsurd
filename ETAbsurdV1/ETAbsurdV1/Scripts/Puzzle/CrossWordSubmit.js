var validcrossword = true;
$('#Create-CrossWord').click(function () {

    $('.single-box').each(function () {
        if ($(this).attr('class') == 'single-box invalid-box') {
            validcrossword = false;
            console.log("win");
        }
    })
    
    if (validcrossword = true) {
        $('#Cross-word-Confirmation').fadeIn();

        $('#Cross-word-Confirmation').append(
                "<h3 class='h-speak'>Crossword Data</h3>"
                + "<div class=''>"
                + "<p>" + crossWordData.X + " boxes in length</p>"
                + "<p>" + crossWordData.Y + " boxes in height</p>"
                + "</div>"
                + "<h3 class='h-speak'>Questions</h3>"
            );
        for(var i = 0; i < crossWordData.Q.length;i++){
            thisq = crossWordData.Q[i];
            $('#Cross-word-Confirmation').append(
                    "<div class=''>"
                    + "<p>Number: " + thisq.Number + "</p>"
                    + "<p>Direction: " + thisq.Direction + "</p>"
                    + "<p>Clue: " + thisq.Clue + "</p>"
                    + "<p>Word: " + thisq.Word + "</p>"
                    + "</div>"
                );
        }
    }

})

function submitCrossWordToServer() {
    $.post("/Puzzles/CrossWordSubmitted", { CrossWord: crossWordData, }, function (data) {
        //>>>>???
        if (data != "false") {
            $.post("~/Puzzles/CrossWord", { Id: data, }, function (data) {
                window.location.href = "/Puzzles/CrossWord/" + data;
            })
        }
    })
}