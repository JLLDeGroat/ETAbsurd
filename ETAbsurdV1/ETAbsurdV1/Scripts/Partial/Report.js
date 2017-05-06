$('.report-row').perfectScrollbar();

function ReportClicked(x, y) {
    console.log(x,y);
   
    //x is author y is Body
    $('.report-type').html("Report");
    $('.report-title').html(x);
    $('.report-body').html(y);
    
    
    $('.report-details').html(getDetails("Report"));
    $('.report-drop-down').html(getDropDown("Report"));
    $('.report-text-box').html(getTextBox("Report"));

    $('.report-row').css('display', 'block');
    $('.report-row').removeClass('spawn-out');
}

$('.report-close').click(function () {
    $('.report-row').addClass(' spawn-out');
    setTimeout(function () {
        $('.report-row').css('display', 'none');
    }, 1000);
})

function BlogReportClicked(x,y){
    $('.report-type').html("Report");
    $('.report-title').html(x);
    $('.report-body').html(y);


    $('.report-details').html(getDetails("Report"));
    $('.report-drop-down').html(getDropDown("Report"));
    $('.report-text-box').html(getTextBox("Report"));

    $('.report-row').css('display', 'block');
    $('.report-row').removeClass('spawn-out');
}


function getDetails(x) {
    var details = "";
    if (x == "Report") {
        details = "<ul><li>Reporting a user for posting content that is unnecessarily aggressive or incites hate is encouraged, it is just meaningless to go about the internet in this way. Users will have their account wiped from here.</li>"
            + "<li>Reporting a user for spam will have the postings deleted, it is not prudent to destroy a user for a potential double post or over advertisement of ones self.</li>"
            + "<li>Invalid Reports, usually are due to personal vendettas when views are strongly opposed... These reports are pointless and you will be asked to stop being so petty. Please confront the opposing viewpoint constructively"
            + " and logically, intense emotions are not whats needed here.</li>"
            + "<li>I ask that you justify your report, no witch hunting here.</li></ul>";
    }

    return details;
}

function getDropDown(x){
    var dropDown = "";
    if (x == "Report") {
        dropDown = "<select id='Report-Select' class='form-control' required>"
                    + "<option value=''></option>"
                    + "<option value='Hate/Racism'>Hate/Racism</option>"
                    + "<option value='Destructive'>Destructive or not constructive</option>"
                    + "<option value='Irrelevant'>Irrelevant</option>"
                    + "<option value='Dangerous'>Dangerous</option>"
                    + "<option value='Other'>Other</option>"
                    + "</select>";
    }
    return dropDown;
}

function getTextBox(x){
    var textBox = "";
    if (x == "Report") {
        textBox = "<input type='text' class='form-control' Id='Report-Text'"
                   + "pattern='.{8,}'   required title='8 characters minimum' />";
    }
    return textBox;
}


function reportSubmit(){
    var Select = $('#Report-Select').val();
    var Text = $('#Report-Text').val();

    if ($('.report-body').html() == "Puzzle") {
        //Reporting a Puzzle
        $.post('/Puzzle/ReportPost', { Reason: Select, Justification: Text, Accused: $('.report-title').html(), Id: $('.thread-title').html(), }, function (data) {

            $('.report-row').addClass(' spawn-out');
            setTimeout(function () {
                $('.report-row').css('display', 'none');
            }, 1000);
            $('#vote-message').html(data);
            $('.vote-message-box').css('display', 'block');
            voteFail = true;
            setTimeout(function () {
                $('.vote-message-box').css('display', 'none');
                voteFail = false;
            }, 2000);

        })
    }

    $.post('/Forum/ReportPost', { Reason: Select, Justification: Text, Accused: $('.report-title').html(), Id: $('.thread-title').html(), }, function (data) {
        
        $('.report-row').addClass(' spawn-out');
        setTimeout(function () {
            $('.report-row').css('display', 'none');
        }, 1000);
        $('#vote-message').html(data);
        $('.vote-message-box').css('display', 'block');
        voteFail = true;
        setTimeout(function () {
            $('.vote-message-box').css('display', 'none');
            voteFail = false;
        }, 2000);

    })
}