var crossWordData = {
    X: 0, Y: 0, TotalSquares: 0,
    SquareLength: 0, Instructions: 0,
    Q: [],
};
var questionArray = [];






var int = 1

function nextarray() {
    int += 1;
    var thisQuestion = question();
    thisQuestion.Number = int;

    crossWordData.Q.push(thisQuestion);

    for (var i = 0; i < crossWordData.Q.length; i++) {
        console.log(crossWordData.Q[i]);
    }
}