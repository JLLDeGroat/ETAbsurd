voteFail = false;
function voteClicked(id, vote, y) {
    //console.log(id, vote, type);    
    $.post("/Puzzles/Vote", { Id: id, Vote: vote, }, function (data) {
        if (data == "true") {
            console.log("win");
            var x = $(y).siblings(".vote-section");
            if (vote == "up") {
                $(x).html(parseInt($(x).html() + 1));
            }
            else if (vote == "down") {
                $(x).html(parseInt($(x).html() - 1));
            }
        }
        else if (data == "false") {
            if (voteFail == false) {
                $('#vote-message').html("you have already voted on this before.");
                $('.vote-message-box').css('display', 'block');
                voteFail = true;
                setTimeout(function () {
                    $('.vote-message-box').css('display', 'none');
                    voteFail = false;
                }, 2000);
            }
        }
        else if (data == "SelfVote") {
            if (voteFail == false) {
                $('#vote-message').html("you can not vote for yourself.");
                $('.vote-message-box').css('display', 'block');
                voteFail = true;
                setTimeout(function () {
                    $('.vote-message-box').css('display', 'none');
                    voteFail = false;

                }, 2000);
            }
        }
        else {
            if (voteFail == false) {
                $('.vote-message-box').css('display', 'block');
                $('#vote-message').html("please log in before passing judgement.");
                voteFail = true;
                setTimeout(function () {
                    $('.vote-message-box').css('display', 'none');
                    voteFail = false;
                }, 2000);
            }
        }
    });
}


html = "";
var Id = window.location.href.slice(-1);
$.post("/Puzzles/GetComments", { dataType: 'json', contentType: 'application/json; charset=utf-8', Id: Id, }, function (data) {
    console.log(data);
    if(data.length != 0){
        for (i = 0; i < data.length; i++) {
            $('.post-submit-load').css('display', 'none');

            html += "<div class='this-comment'>"
                + "<div class='row'>"
                + "<div class='col-sm-3'><p class='comment-author'>" + data[i].Author + "</p></div>"
                + "<div class='col-sm-9'><p class='comment-body'>" + data[i].Body + "</p></div>"
                + "</div>"
                + "<div class='row'>"
                + "<div class='col-xs-12'><p class='comment-date'>" + new Date(parseInt(data[i].Date.substr(6))).toString().substr(0, 24) + "</p></div>"
                + "</div></div>";             
        }
        $('#Comment-Box').html(html);
    }
    else {
        $('.post-submit-load').css('display', 'none');
        html += "<h3 class='h-speak'>There are no Comments yet</h3>";
        $('#Comment-Box').html(html);
    }
})


function SubmitComment(x, y, z) {
    $(z).css('display', 'none');
    $('.blog-submit-load').css('display', 'block');
    $.post("/Puzzles/PuzzleComment", { Comment: x, Id: y, }, function (data) {
        if (data == "Comment Must be longer than 6 characters") {
            $('#vote-message').html(data);
        }
        else if (data == "Please log in to Comment") {
            $('#vote-message').html(data);
        }
        else {
            $('#vote-message').html("Comment Made");
        }        
        $('.vote-message-box').css('display', 'block');
        voteFail = true;
        setTimeout(function () {
            $('.vote-message-box').css('display', 'none');
            voteFail = false;
        }, 2000);
        $(z).css('display', 'block');
        $('.blog-submit-load').css('display', 'none');
        //reset
        $('#Comment-Text').val('');
        //get new comment        
        $('.post-submit-load').css('display', 'none');

        html = "<div class='this-comment'>"
            + "<div class='row'>"
            + "<div class='col-sm-3'><p class='comment-author'>" + data.Author + "</p></div>"
            + "<div class='col-sm-9'><p class='comment-body'>" + data.Body + "</p></div>"
            + "</div>"
            + "<div class='row'>"
            + "<div class='col-xs-12'><p class='comment-date'>" + new Date(parseInt(data.Date.substr(6))).toString().substr(0, 24) + "</p></div>"
            + "</div></div>";

        $('#Comment-Box').prepend(html);

    })
}


var voteFail = false;
var CanAnswer = true;
$('#Answer-Button').click(function () {
    if (CanAnswer == true) {
        var answer = $('#Answer-Text').val();
        var Id = window.location.href.slice(-1);
        $("/Puzzles/AnswerPuzzle", { Answer: answer, Id: id, }, function (data) {
            if (data == "success") {

            }
            else {
                if (voteFail == false) {
                    $('#vote-message').html("That was incorrect. Unfortunately.");
                    $('.vote-message-box').css('display', 'block');
                    voteFail = true;
                    setTimeout(function () {
                        $('.vote-message-box').css('display', 'none');
                        voteFail = false;
                    }, 2000);
                }
            }
        })
        ForceWait();
    }
})

function ForceWait() {
    $('#Answer-Button').val("Wait 9 seconds");
    //user to wait 9 seconds
    var CanAnswer = false;
    setTimeout(function () {
        CanAnswer = true;
        $('#Answer-Button').val("Submit");
    }, 9000);
}