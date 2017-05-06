//have box height also width
$('.loader-box').css('height', $('.loader-box').css('width'));
//Have loader centralised


window.onload = StartLoad();
var progression = 0;
var gameData = {
    X: 10, Y: 10, Z: 10,
    Instruction: "",
    Information: "",
    Error: null,
};

var canClick = false;

$('#StartGame').click(function () {    
    $('.start-game-box').fadeOut(start);
    function start() {
        $('.game-setting').fadeIn(startText);
    }    
})
function startText() {
    $(".made-text").typed({
        strings: ["Initialising..."],
        typeSpeed: 100,
        showCursor: false,
        callback: function () {
            setTimeout(initialising, 1000);
        },
    });
    function initialising() {
        $(".made-text").typed({
            strings: ["Initialised"],
            typeSpeed: 100,
            showCursor: false,
            callback: function () {
                setTimeout(initialised, 1500);
            }
        });
    }
    function initialised() {
        canClick = true;
        $('.made-text').typed({
            strings: ["click to continue"],
            typeSpeed: 80,
            showCursor: false,
            callback: function () {
                
            }
        })
    }

}

$('.game-setting').click(function () {
    if (canClick == true) {
        canClick = false;
        $('.made-text').html('');
        beginStory();
    }
})

function beginStory() {
    $.post("/Game/GetResult", { GameData: JSON.stringify(gameData), }, function (data) {
        $('.made-text').typed({
            strings: [data.Information],
            typeSpeed: 10,
            showCursor: false,
            callback: function () {
                $('.game-controls').fadeIn();
                $('.control-block').fadeIn();
                $('.item-block').fadeIn(); 
            }
        })
    })
}

var processing = false;
$(document).keypress(function (e) {
    if (e.which == 13 & processing == false) {
        processing = true;

        $('#load-box').fadeIn();
        $('.made-text').html('');
        $('.error-text').html('');
        $('.separator').html('');
        gameData.Instruction = $('#control-text').val();
        $.post("/Game/GetResult", { GameData: JSON.stringify(gameData), }, function (data) {
            processing = false;

            $('#load-box').fadeOut();
            $('#control-text').val('');
            $('#control-text').focus();

            if (data.Error != null) {                
                $('.error-text').typed({
                    strings: [data.Error],
                    typeSpeed: 20,
                    showCursor: false,
                    callback: function () {
                        $('.separator').html("<br/><br/>");
                        $('.made-text').typed({
                            strings: [data.Information],
                            typeSpeed: 20,
                            showCursor: false,
                            callback: function () {

                            }
                        })
                    }
                })
            }
            else {
                $('.error-text').typed({
                    strings: [""],
                    typeSpeed: 20,
                    showCursor: false,
                })
                $('.made-text').typed({
                    strings: [data.Information],
                    typeSpeed: 20,
                    showCursor: false,
                    callback: function () {

                    }
                })
            }            
            //update data
            gameData.Information = data.Information;
            gameData.X = data.X;
            gameData.Y = data.Y;
            gameData.Z = data.Z;
        })
    }
});