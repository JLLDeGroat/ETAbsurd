var bodystring = $('#Blog-Body').html();

$(window).load(function () {
    var replacementBody = bodystring.replace(/&lt;&lt;&lt;/g, '<img class="center-image" src="').replace(/&gt;&gt;&gt;/g, '"/>');
    /*$.post("/Blog/BlogImage", {Code: bodystring}, function (data) {

    })*/
    $('#Blog-Body').html(replacementBody);
})

function KarmaVote(id, type) {
    $.post("/Blog/KarmaVote", { Id: id, Type: type, }, function (data) {       
        $('#vote-message').html(data);
        $('.vote-message-box').css('display', 'block');
        voteFail = true;
        setTimeout(function () {
            $('.vote-message-box').css('display', 'none');
            voteFail = false;
        }, 2000);
    })
}


function SubmitComment(x, y, z) {
    console.log(x, y);
    $(z).css('display', 'none');
    $('.blog-submit-load').css('display', 'block');
    $.post("/Blog/BlogComment", { Comment: x, Id: y, }, function (data) {
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
        if(data == "Comment Made"){
        html = "<div class='this-comment'>"
            + "<div class='row'>"
            + "<div class='col-sm-3'><p class='comment-author'>" + data.Author + "</p></div>"
            + "<div class='col-sm-9'><p class='comment-body'>" + data.Body + "</p></div>"
            + "</div>"
            + "<div class='row'>"
            + "<div class='col-xs-12'><p class='comment-date'>" + new Date(parseInt(data.Date.substr(6))).toString().substr(0, 24) + "</p></div>"
            + "</div></div>";

        $('#Comment-Box').prepend(html);
        }
    })  
}


var CommentSection = $(".blog-comment-section").offset().top;
var commentLoaded = false;

$(window).scroll(function () {
    //plus 500 to ensure on smaller screens the comments will always load.
    commentLoaded = true;
    var Id = window.location.href.split('/');
    Id = Id[Id.length - 1];
    console.log(Id);
    html = "";
    $.post("/Blog/GetComments", { dataType: 'json', contentType: 'application/json; charset=utf-8', Id: Id, }, function (data) {
        if (data != "No Comments Available") {
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
            $('#Comment-Box').html("<h3 class='h-speak'>No Comments Yet</h3>");
        }
    })
});