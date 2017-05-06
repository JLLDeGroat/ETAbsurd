$(window).load(function () {
    var factionName = $('#Faction-Panel-Title').html();
    $.post("/Faction/ControlPanelPopulation", { FactionName: factionName, Id: 0, }, function (data) {
        $('#User-Box-Content').html(data);
        html = "";
        for (i = 0; i < data.length; i++) {
            html += "<div class='col-sm-2 col-xs-6 faction-singleuser spawn-in' onclick='Relocate(this, " + data[i].Username + ")'>"
                 + "<div class='faction-user-avatar-box' >"
                 + "<img id='" + data[i].Username + "' src='" + data[i].Avatar + "' alt='' class='faction-user-avatar-image spawn-in' />"
                 + "</div>"
                 + "<div class='user-name-box'><p class='faction-username'>" + data[i].Username + "</p></div>"
                 + "</div>";

            $('#User-Box-Content').append(html);
        }


        $.post("/Faction/ControlPanelPopulation", { FactionName: factionName, Id: 1, }, function (data2) {
            var html2 = "";
            for (i = 0; i < data2.length; i++) {
                console.log(data2[i].AuthorAvatar);
                if (data2[i].AuthorAvatar == null | data2[i].AuthorAvatar == undefined) {
                    data2[i].AuthorAvatar == FactionImage(factionName);
                }
                html2 = "<div class='col-md-4 spawn-in faction-thread-post'>"
                    + "<img class='faction-user-avatar-image spawn-in' src='" + data2[i].AuthorAvatar + "' />"
                    + "<div class=''>" + data2[i].Body + "</div>"
                    + "</div>";
                $('#Thread-Box-Content').append(html2);
            }
            
            $.post("/Faction/ControlPanelPopulation", { FactionName: factionName, Id: 2, }, function (data3) {
                if (data3 != null) {
                    for (i = 0; i < data3.length; i++) {
                        console.log(data3);
                        html3 = "<div class='col-md-4 spawn-in faction-post'>"
                           + "<img class='faction-user-avatar-image spawn-in' src='" + data3[i].AuthorAvatar + "' />"
                           + "<div class=''>" + data3[i].Body + "</div>"
                           + "</div>";
                        $('#Post-Box-Content').append(html3);
                    }
                }
            })
            
        })
    })



})


function FactionImage(x) {
    var y = window.location.protocol + "//" + window.location.host + "/";
    if (x == "Theology") {
        y = y + "Theology.jpg";
    }
    else if (x == "Narcassism") {
        y = y + "Narcassism";
    }
    else if (x == "Agnostic") {
        y = y + "Agnostic";
    }
    else if (x == "Nihilism") {
        y = y + "Nihilism";
    }
    else if (x == "PostModernism") {
        y = y + "PostModernism";
    }
    else if (x == "Solipsism") {
        y = y + "Solipsism";
    }
    return y
}


function Relocate(x, y) {
    console.log($(x));
    if($(x).hasClass('faction-singleuser')){
        window.location.pathname = "Account/ViewProfile?UserName=" + y;
    }
    if ($(x).hasClass('faction-thread-post')) {

    }
    if ($(x).hasClass('faction-post')) {

    }   
}