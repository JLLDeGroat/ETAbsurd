using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class Puzzle
    {
        //Base puzzles
        public string Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public string Answer { get; set; }

    }


    public class PuzzleStart
    {

        [Required]
        public string Category { get; set; }
        [Required]
        public string ChosenCategory { get; set; }
    }


    public class PuzzleSearch
    {

        public string Name { get; set; }
        
        public string PuzzleType { get; set; }

    }
}