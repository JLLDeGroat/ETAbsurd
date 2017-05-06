window.onload = function () {
    $.post("/Puzzles/CreateCategories", {}, function (data) {
        //remove nulls
        data = jQuery.grep(data, function (n, i) {
            return (n !== "" && n != null);
        });
        $('.post-submit-load').remove();
        populateDropDown(data);
        $('#Cat-Dropdown').removeClass('hidden');
        $('#Category').autocomplete({
            source: data
        });
    })
}
function populateDropDown(x) {
    html = "";
    for (i = 0; i < x.length; i++) {
        $('#Cat-Dropdown').append($('<option>', { value: x[i], text: x[i] }));
    }
}
$('#Cat-Dropdown').change(function () {
    $('#Chosen-Category').html($(this).val()); //Not the same
    $('#Category').val($(this).val());
    $('#ChosenCategory').val($(this).val());
    getExplanation($(this).val());
    $('.submit-button').removeClass('hidden');
})

//Get The top item from dropdown bar
//Change title depending on this

function getExplanation(Category) {
    var html = "";
    $.post("/Puzzles/Description", { Key: Category, }, function (data) {
        if (data == "1") {
            html = "<h2>Text Based Puzzles</h2>"
              +"<article>"
              + "These are rather simple to create but can in fact be the hardest to solve. This type of puzzle architect consists"
              + "of a large body of text, with a text based answer. Ensure that you know exactly what your answer is and that it "
              + "is typed in correctly, also be aware that it is possible for numerous answers to be correct and would therfore suggest"
              + "you consider all the types of answers that could potentially come in and maybe consider adding in some more correct answers."
              + "</article>"
              + "<article>"
              + "  Most commonly, these types of puzzles would comprise of:"
              + "    <ul>"
              + "      <li>"
              + "        Mathematic puzzles, or equational puzzles based on probability."
              + "  </li>"
              + " <li>"
              + "    riddles or lateral thinking puzzles based around the usage of words "
              + "</li>"
              + "<li>"
              + "    Logic puzzles, trying to deduce the logic behind a particular puzzle"
              + "</li>"
              + "<li>"
              + "    Image based puzzles, where as the answer is within an image"
              + "</li>"
              + "</ul>"
              + "The difficulty of these puzzles can vary dramatically, from a quick an poetic riddle to the most frustrating lateral thinking puzzle"
              + " that stumps everyone."
          + "</article>"
        }
        else if (data == "2") {
            html = " <h2>Cross Word</h2>"
                  + "<article>"
                     + " Quite a niche puzzle, since this only goes one way, although a simple word search could also be constructed using this structure."
                      + "If you are able to create any variations of the classic word search/crossword genre I'm sure it would be a must for people to think through"
                      + "        and solve."
                      + "    </article>"
                      + "    <article>"
                      + "        Although the main usage of this is for Crosswords and word searches, if you find that there is a hidden potential i would ask you contact me"
                      + "        to let me know the extra tools you would need on this to create your puzzle of choice."
                      + "    </article>";
        }
        else {
            html = "<h2>Unkown</h2>"
                 + " <article>"
                 + "It appears you have written in a potentially new and thriving genre of puzzle to hit the human race since sliced bread. Unfortunately,"
                 + " I do not know or have the tools that may be specialised to this particular type of puzzle and would ask that you let me know if"
                 + " there are any particulars needed to form your ideas into virtual pixels."
                 + " </article>";
        }
        $('#Category-Description').html(html);
    })
}



