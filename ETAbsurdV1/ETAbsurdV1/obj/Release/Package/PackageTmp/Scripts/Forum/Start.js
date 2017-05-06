//Advance
var advancedActive = false;
$('#Advanced').click(function () {
    if (advancedActive == false) {
        makeActive(advancedActive);       
        advancedActive = true;
    }
    else if (advancedActive == true) {
        makeActive(advancedActive);
        advancedActive = false;
    }
})

function makeActive(a) {
    console.log(a);
    if (a == false) {
        $('.advanced-option').each(function () {
            $(this).css('display', 'block');
            $(this).animate({
                height: 80,
                opacity: 1,
            }, 250, function () { });
        })
    }
    else if (a == true) {
        $('.advanced-option').each(function () {
            $(this).animate({
                height: 0,
                opacity: 0,
            }, 500, function () {
                $(this).css('display', 'none');
            })
        });
    }
}

//Category
$('#CategoryList').change(function () {
    $('#Category').val(this.value);
})



/*Image*/
function fileSelected(input) {
    var count = document.getElementById('fileToUpload').files.length;
    for (var index = 0; index < count; index++) {
        var file = document.getElementById('fileToUpload').files[index];
        $('#Avatar').val("|{Uploading}|" + file.name);
    }
    uploadFile();
}
function uploadFile() {
    var fd = new FormData();
    var count = document.getElementById('fileToUpload').files.length;
    for (var index = 0; index < count; index++) {
        var file = document.getElementById('fileToUpload').files[index];
        fd.append('myFile', file);
    }
    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", uploadProgress, false);
    xhr.addEventListener("load", uploadComplete, false);
    xhr.addEventListener("error", uploadFailed, false);
    xhr.addEventListener("abort", uploadCanceled, false);
    xhr.open("POST", "/Account/UploadThreadImage");
    xhr.send(fd);
}

function uploadProgress(evt) {
    if (evt.lengthComputable) {
        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
        document.getElementById('uploadPercentage').innerHTML = percentComplete.toString() + '%';
    }
    else {
        document.getElementById('progress').innerHTML = 'Upload error!';
    }
}
function uploadComplete(evt) {
    document.getElementById('uploadPercentage').innerHTML = "Complete";
    $('#Image').val($('#Image').val().replace("|{Uploading}|", "|{Uploaded}|"));
}
function uploadFailed(evt) {
    alert("Error sending file...");
}
function uploadCanceled(evt) {
    alert("Upload cancelled by the user or network error!");
}
