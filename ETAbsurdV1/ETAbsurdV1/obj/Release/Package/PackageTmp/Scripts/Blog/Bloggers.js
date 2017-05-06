function FollowBlog(x, UserName) {
    $(x).siblings('.post-submit-load').css('display', 'block');
    $(x).addClass('hdn-follow-btn');
    $.post("/Blog/FollowBlogger", { UserName: UserName, }, function (data) {
        if (data == "Redirect") {
            window.location.href = "/Login/Login?ReturnUrl=/Blog/Bloggers";
            console.log("nope");
        }
        else if (data == "Followed") {
            $('.post-submit-load').css('display', 'none');
            $(x).siblings().removeClass('hdn-follow-btn');

            $('#vote-message').html("You are now following " + UserName);
            $('.vote-message-box').css('display', 'block');
            voteFail = true;
            setTimeout(function () {
                $('.vote-message-box').css('display', 'none');
                voteFail = false;
            }, 2000);
        }
    })
}

function UnfollowBlog(x, UserName) {
    $(x).siblings('.post-submit-load').css('display', 'block');
    $(x).addClass('hdn-follow-btn');
    $.post("/Blog/UnfollowBlogger", { UserName: UserName, }, function (data) {
        if (data == "Unfollowed") {
            $('.post-submit-load').css('display', 'none');
            $(x).siblings().removeClass('hdn-follow-btn');

            $('#vote-message').html("You have stopped following " + UserName);
            $('.vote-message-box').css('display', 'block');
            voteFail = true;
            setTimeout(function () {
                $('.vote-message-box').css('display', 'none');
                voteFail = false;
            }, 2000);
        }
    })
}