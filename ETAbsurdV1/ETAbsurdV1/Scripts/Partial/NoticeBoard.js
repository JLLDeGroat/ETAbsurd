function MoreInfo(phylosophy) {
    $('.notice-box').css('display', 'block');
    $.post("/Faction/FactionDescription", { FactionName: phylosophy, }, function (data) {
        $('#Notice-Title').html(phylosophy);
        $('#Notice-Body').html(data);
    })
}

$('.notice-box').click(function () {
    $('.notice-box').addClass('spawn-out');
    $('.notice-box').removeClass('spawn-in');
    setTimeout(function () {
        $('.notice-box').css('display', 'none');
        $('.notice-box').addClass('spawn-in');
        $('.notice-box').removeClass('spawn-out');
    }, 1000);
})


function Decide(Faction) {
    $('.notice-box-confirmation').css('display', 'block');
    $('#Notice-Confirm').html(Faction);
}

function ConfirmDecision(Faction) {
    console.log(Faction);    
    $.post("/Faction/FactionDecisionConfirmation", { FactionName: Faction }, function () {
        document.location.href = "/Faction/ControlPanel";
    })   
}