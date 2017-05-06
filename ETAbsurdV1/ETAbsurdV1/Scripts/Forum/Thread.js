$('#Post-Submit').click(function () {
    if ($('#Post').val().length > 1) {
        $('#Post-Submit').css('display', 'none');
        $('.post-submit-load').css('display', 'block');
    }
});

voteFail = false;
function voteClicked(id, vote, type, y) {
    //console.log(id, vote, type);    
    $.post("/Forum/Vote", { Id: id, Vote: vote, Type: type, }, function (data) {
        if (data == "true") {
            console.log("win");
            var x = $(y).siblings(".vote-section");
            if (vote == "up") {
                var current = parseInt($(x).html());
                var newCurrent = parseint(current + 1);
                $(x).html(newCurrent);
            }
            else if (vote == "down") {
                var current = parseInt($(x).html());
                var newCurrent = parseint(current - 1);
                $(x).html(newCurrent);
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

function QuoteClicked(x, y, z) {
    console.log(y, z, x);
    $("html, body").animate({ scrollTop: $(document).height() }, "slow");
    var elm = '<div class="quote-section">'
        + '<p>' + y + ' - ' + z + '</p>'
        + '<p>' + x + '</p>'
        '</div>';
        $('#Quote-Box').html(elm);
        $('#Quote').val(y + ': -   ' + z + ',' + x);
}

