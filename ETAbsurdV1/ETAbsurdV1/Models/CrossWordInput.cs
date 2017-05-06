using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class CrossWordInput
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int TotalSquares { get; set; }
        public string SquareLength { get; set; }
        public int instructions { get; set; }
        public string Name { get; set; }

        public List<CrossWordInputQuestions> Q { get; set; }      


    }

    public class CrossWordInputQuestions
    {
        public string Word { get; set; }
        public string Clue { get; set; }
        public string Direction { get; set; }
        public int Number { get; set; }
        public int Position { get; set; }
        public int WordLength { get; set; }
    }
}