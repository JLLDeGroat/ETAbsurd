night = false;

function NightMode() {
    if (night == false) {
        night = true;
        $('.form-control').each(function () {
            $(this).css('background-color', '#333333');
            $(this).css('color', 'white');
            $(this).css('border', '1px solid #7fffd4');
        })
    }
    else {
        night = false;
        $('.form-control').each(function () {
            $(this).css('background-color', 'white');
            $(this).css('color', 'black');
            $(this).css('border', '1px solid #cccccc');
        })
    }    
}

//HELP
var guideVisible = false;
$('.help-button').click(function () {
    if (guideVisible == false) {
        $('.blog-guide').css('display', 'block');
        $('.blog-guide').removeClass('spawn-out');
        setTimeout(function () { guideVisible = true; }, 1000)
    }
})
$(document).click(function () {
    if (guideVisible == true) {
        $('.blog-guide').addClass('spawn-out');
        $('.blog-guide').fadeOut('slow');
        guideVisible = false;
    }

})





//SCROLLING
$(window).scroll(function () {
    if ($(window).scrollTop() >= 52) {
        $('.help-button').fadeOut("fast");
    }
    if ($(window).scrollTop() <= 51) {
        $('.help-button').fadeIn("slow");
    }
});



//IMAGES

function fileSelected(input) {
    ThisId = $(input).attr('id');
    var count = document.getElementById(ThisId).files.length;
    for (var index = 0; index < count; index++) {
        var file = document.getElementById(ThisId).files[index];
        $('#Image' + $(input).attr('id').split('-')[1]).val("|{Uploaded}|" + file.name);
    }
    uploadFile(ThisId);
}
function uploadFile(input) {
    var fd = new FormData();
    var count = document.getElementById(input).files.length;
    for (var index = 0; index < count; index++) {
        var file = document.getElementById(input).files[index];
        fd.append('myFile', file);
    }
    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", uploadProgress, false);
    xhr.addEventListener("load", uploadComplete(input), false);
    xhr.addEventListener("error", uploadFailed, false);
    xhr.addEventListener("abort", uploadCanceled, false);
    xhr.open("POST", "/Blog/UploadImage");
    xhr.send(fd);
}

function uploadProgress(evt) {
    if (evt.lengthComputable) {
        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
        document.getElementById('uploadPercentage').innerHTML = percentComplete.toString();
    }
    else {
        document.getElementById('progress').innerHTML = 'Upload error!';
    }
}
function uploadComplete(evt) { 
    setTimeout(function () {
        document.getElementById('uploadPercentage').innerHTML = "Uploaded";
    }, 1500);
}
function uploadFailed(evt) {
    alert("Error sending file...");
}
function uploadCanceled(evt) {
    alert("Upload cancelled by the user or network error!");
}

