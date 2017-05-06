var fontarray = ['Georgia, serif', '"Palatino Linotype", "Book Antiqua", Palatino, serif', '"Times New Roman", Times, serif', 'Arial, Helvetica, sans-serif',
    '"Arial Black", Gadget, sans-serif', '"Comic Sans MS", cursive, sans-serif', 'Impact, Charcoal, sans-serif', '"Lucida Sans Unicode", "Lucida Grande", sans-serif',
    'Tahoma, Geneva, sans-serif', '"Trebuchet MS", Helvetica, sans-serif', 'Verdana, Geneva, sans-serif', '"Courier New", Courier, monospace','"Lucida Console", Monaco, monospace'
]
window.onload = function () {
    $(':input[type=text]').val('');
    $('textarea').val('');

    var fontlist = "<option></option>";
    for (i = 0; i < fontarray.length; i++) {
        fontlist += "<option value='" + fontarray[i] + "' style='font-family:" + fontarray[i] + ";'>" + fontarray[i] + "</option>";
    }
    $('#font-family-select').html(fontlist);
    //Embed Images
    $('.gif-img').each(function (x, y) {
        console.log(
            document.location.origin + $(y).attr('src'),
            $(y).siblings('.embed-gif-text')
            );
        $(y).siblings('.embed-gif-text').val('<img src="' + document.location.origin + $(y).attr('src') + '" style="max-width:100%;display:block;margin:auto;"/>');
    });
}
/*
$(window).resize(function () {
    var userimage = $('.user-image-box').css('width');
    $('.user-image-box').css('height', userimage);
})
*/
//Text color & family
$('#Font').keyup(function () {
    $('.user-test-font-family').css('font-family', this.value);
})
$('#font-family-select').change(function () {
    $('#Font').val($('#font-family-select option:selected').text());
    $('.user-test-font-family').css('font-family', this.value);
})
$('#TextColor').keyup(function () {
    $('.user-test-font-color').css('color', this.value);
})

//Update N_BOX AND OTHERS
function ShowUpdateSection(x) {
    var Clicked = "#" + x.id + "Section";
    if ($(x).html() != "Nah, changed my mind.") {
        $(Clicked).addClass('hidden-update-clicked');
        $(Clicked).animate({
            height: 185,
            opacity: 1,            
        }, 500, function () {

        });
        $(x).html("Nah, changed my mind.");        
    }
    else {
        $(Clicked).animate({
            height: 0,
            opacity: 0,
        }, 500, function () {
            $(Clicked).removeClass('hidden-update-clicked');
            console.log("Removed");
        });
        $(x).html("Change");        
    }
}

//IMAGE UPLOAD
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
    xhr.open("POST", "/Account/UploadImage");
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
    $('#Avatar').val($('#Avatar').val().replace("|{Uploading}|", "|{Uploaded}|"));
}
function uploadFailed(evt) {
    alert("Error sending file...");
}
function uploadCanceled(evt) {
    alert("Upload cancelled by the user or network error!");
}

//Reputation Colors
$('.stat-line').each(function (x, y) {
    // y[2] is reputation
    if (x == 2) {
        //childNode 1 is the actual stat with rep count on
        var reputationdiv = y.childNodes[1];
        var reputation = parseFloat(y.childNodes[1].innerHTML);

        var defaultColour = 255;
        if (reputation < 0) {
            var scaleRepColour = parseInt(reputation) * 3;
            //plus because math
            var newColour = defaultColour + scaleRepColour;
            $(reputationdiv).css('color', 'rgb(255, ' + newColour + ', ' + newColour + ')');
        }
        else if (reputation > 0) {
            $(reputationdiv).css('color', 'blue');
            var scaleRepColour = parseInt(reputation) * 3;
            var newColour = defaultColour - scaleRepColour;
            $(reputationdiv).css('color', 'rgb(255,  '+ newColour + ', 255)');
        }
    }
    //x[15] is karma percantage
    if (x == 15) {
        var karmadiv = y.childNodes[1];
        var karma = parseFloat(y.childNodes[1].innerHTML);
        var defaultColour = 255;
        if (karma < 50) {           
            karma = 100 - karma;           
            var newColour = defaultColour - parseInt(karma * 1.8);
            $(karmadiv).css('color', 'rgb(255, 255, ' + newColour + ')');

            //Change Div to reflect Renegade
            $(karmadiv).html(karma + "R");
        }
        else if (karma > 50) {           
            var newColour = defaultColour - parseInt(karma * 1.8);
            if (newColour < 0) {
                newColour = 0;
            }
            $(karmadiv).css('color', 'rgb(255,  255 ,' + newColour + ')');

            //Change Div to reflect Paragon
            $(karmadiv).html(karma + "P");
        }
    }


})


