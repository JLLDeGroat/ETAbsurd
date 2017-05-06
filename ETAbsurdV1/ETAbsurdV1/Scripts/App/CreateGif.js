var ThisGif = {
    Name: "", Speed: 20,
}

//IMAGES
function fileSelected(input) {
    ThisId = $(input).attr('id');
    uploadFile(ThisId);
    /*
    ***************************FOR EVER SHOWING IMAGES UPLOADED
    var count = document.getElementById(ThisId).files.length;
    for (var index = 0; index < count; index++) {
        var file = document.getElementById(ThisId).files[index];
        $('#Image' + $(input).attr('id').split('-')[1]).val("|{Uploaded}|" + file.name);
    }
    */    
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
    xhr.open("POST", "/App/UploadGifImage");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            populateGifImage(xhr.responseText);
        }
    }
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
function uploadComplete(input, evt) {
    
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

//After images uploaded
function populateGifImage(img) {
    img = img.substring(2, img.length - 1);
    $('#Gif-Display').attr('src', img);
    $('#Gif-Option-Box').removeClass('hdn');
    $('#Gif-Save-Box').removeClass('hdn');
    $('#Gif-Display-Box').removeClass('hdn');
    new Dragdealer('Frame-Speed-Slider', {
        animationCallback: function (x, y) {
            $('.handle').text(Math.round((x * 100) + 5) + "ms");
        }
    });
    ThisGif.Name = img;
}

//Slider
function updateGif()
{
    var fd = new FormData();
    var ms = $('#Gif-MS').html().replace('ms', '');
    ThisGif.Speed = ms;
    fd.append('MS', ms);
    var count = document.getElementById('Gif-To-Upload').files.length;
    for (var index = 0; index < count; index++) {
        var file = document.getElementById('Gif-To-Upload').files[index];
        fd.append('myFile', file);
    }
    var xhr = new XMLHttpRequest();
    //xhr.upload.addEventListener("progress", uploadProgress, false);
    //xhr.addEventListener("load", uploadComplete(input), false);
    //xhr.addEventListener("error", uploadFailed, false);
    //xhr.addEventListener("abort", uploadCanceled, false);
    xhr.open("POST", "/App/ModifyGifImage");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            rePopulateGifImage(xhr.responseText);
        }
    }
    xhr.send(fd);
    
    
}

function rePopulateGifImage(img) {
    img = img.substring(2, img.length - 1);
    $('#Gif-Display').attr('src', img);
    $('#Gif-Display-Box').html("<img id='Gif-Display' src='" + img + "' class='app-gif-img' alt=''/>")
    new Dragdealer('Frame-Speed-Slider', {
        animationCallback: function (x, y) {
            $('.handle').text(Math.round((x * 100) + 5) + "ms");
        }
    });
    ThisGif.Name = img;
}

//finish and save
var saving = false;
function saveGif() {
    if (saving == false) {
        saving = true;
        var fd = new FormData();
        var ms = $('#Gif-MS').html().replace('ms', '');
        ThisGif.Speed = ms;
        fd.append('MS', ms);
        var count = document.getElementById('Gif-To-Upload').files.length;
        for (var index = 0; index < count; index++) {
            var file = document.getElementById('Gif-To-Upload').files[index];
            fd.append('myFile', file);
        }
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/App/SaveGif");
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                $('#vote-message').html("Gif Created.");
                $('.vote-message-box').css('display', 'block');
                voteFail = true;
                setTimeout(function () {
                    $('.vote-message-box').css('display', 'none');
                    voteFail = false;
                }, 2000);
                window.location.href = "/Account/ControlPanel";
            }
        }
        xhr.send(fd);
    }
}