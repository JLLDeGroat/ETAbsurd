//Large Menu boxes moused over
window.onload = function () {
    
}
var clicked = false;
$('.menu-box').click(function () {
    clicked = true;
    var mouseditem = this.childNodes[1];
    $(mouseditem).addClass(' mouse-over-main-button');
    $(mouseditem).removeClass('mouse-leave-main-button');
    
    var mouseditemfirstframe = this.childNodes[1].childNodes[1];
   
    $(mouseditemfirstframe).addClass(' main-menu-fade-out');
    $(mouseditemfirstframe).removeClass(' main-menu-fade-in');

    startVideo(this); 
})

$('.menu-box').mouseenter(function () {
    var mouseditemtext = this.childNodes[1].childNodes[1].childNodes[1].childNodes[1];
    $(mouseditemtext).removeClass('menu-box-title-backward');
    $(mouseditemtext).addClass('menu-box-title-forward');
})

$('.menu-box').mouseleave(function () {
    if (clicked == true) {
        clicked = false;
        var mouseditem = this.childNodes[1];

        $(mouseditem).removeClass('mouse-over-main-button');
        $(mouseditem).addClass(' mouse-leave-main-button');

        var mouseditemfirstframe = this.childNodes[1].childNodes[1];

        $(mouseditemfirstframe).removeClass(' main-menu-fade-out');
        $(mouseditemfirstframe).addClass(' main-menu-fade-in');

        stopVideo(this);

        //Add light shine back to div        
    }
    var mouseditemtext = this.childNodes[1].childNodes[1].childNodes[1].childNodes[1];
    $(mouseditemtext).removeClass('menu-box-title-forward');
    $(mouseditemtext).addClass('menu-box-title-backward')
})
//Stops Videos if any button within buttons pressed
$('.category-1, .category-2, .category-3').click(function (){
    $('.mini-video').each(function () {
        var mousedvideo = this;
        var videoURL = $(mousedvideo).prop('src');
        videoURL = videoURL.replace("&autoplay=1", "");
        $(mousedvideo).prop('src', '');
        $(mousedvideo).prop('src', videoURL);
    });
})

//Stops videos playing when browser back button pressed
//if not mobile
if ($(window).width() > 750) {
    $('.mini-video').each(function () {
        var mousedvideo = this;
        var videoURL = $(mousedvideo).prop('src');
        videoURL = videoURL.replace("&autoplay=1", "");
        $(mousedvideo).prop('src', '');
        $(mousedvideo).prop('src', videoURL);
    });
    var players = [];

    function startVideo(a) {
        var mousedvideo = a.childNodes[1].childNodes[3].childNodes[1].childNodes[11].childNodes[1];
        var videoURL = $(mousedvideo).prop('src');
        videoURL += "&autoplay=1";
        $(mousedvideo).prop('src', videoURL);
    }

    function stopVideo(a) {
        var mousedvideo = a.childNodes[1].childNodes[3].childNodes[1].childNodes[11].childNodes[1];
        var videoURL = $(mousedvideo).prop('src');
        videoURL = videoURL.replace("&autoplay=1", "");
        $(mousedvideo).prop('src', '');
        $(mousedvideo).prop('src', videoURL);
    }
    //menu items
    $('.menu-button').mouseenter(function () {
        var mouseditem = this.id.split('-')[1];
        if (mouseditem == "1") {
            $('.category-2 ,.category-3').addClass('blur-when-hovered');
            $('.category-2 ,.category-3').removeClass('unblur-when-hovered');
        }
        else if (mouseditem == "2") {
            $('.category-1 ,.category-3').addClass('blur-when-hovered');
            $('.category-1 ,.category-3').removeClass('unblur-when-hovered');
        }
        else {
            $('.category-1 ,.category-2').addClass('blur-when-hovered');
            $('.category-1 ,.category-2').removeClass('unblur-when-hovered');
        }
    })
    $('.menu-button').mouseleave(function () {
        var mouseditem = this.id.split('-')[1];
        if (mouseditem == "1") {
            $('.category-2 ,.category-3').removeClass('blur-when-hovered');
            $('.category-2 ,.category-3').addClass('unblur-when-hovered');
        }
        else if (mouseditem == "2") {
            $('.category-1 ,.category-3').removeClass('blur-when-hovered');
            $('.category-1 ,.category-3').addClass('unblur-when-hovered');
        }
        else {
            $('.category-1 ,.category-2').removeClass('blur-when-hovered');
            $('.category-1 ,.category-2').addClass('unblur-when-hovered');
        }
    })
}
else {
    $('.video-holder').css('display', 'none');
}

//Message Box'

var comps = document.getElementsByClassName("comp");
$(comps).each(function () {
    var elewidth = $(this).css('width');
    $(this).css('height', elewidth);
})
$(window).resize(function () {
    $(comps).each(function () {
        var elewidth = $(this).css('width');
        $(this).css('height', elewidth);
    })
});

