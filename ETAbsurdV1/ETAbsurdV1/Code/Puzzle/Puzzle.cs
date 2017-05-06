using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETAbsurdV1.Models;

namespace ETAbsurdV1.Code.Puzzle
{
    public class Puzzle
    {
        public static List<string> BasePuzzleCategories()
        {
            List<string> Categories = new List<string>{ "Riddle","Question","Lateral Thinking","Logic","Mathematic","CrossWord", };
            return Categories;
        }

        public static List<string> AdvancePuzzleCategories(List<string> BaseCategories, List<string> MadeCategories)
        {
            List<string> AllCategories = BaseCategories.Union(MadeCategories).ToList();           
            return AllCategories;
        }
    }


    public class PuzzleCrossWord
    {
        public Int32 Position { get; set; }
        public string Character { get; set; }
        public Nullable<Int32> Question { get; set; }
        public static List<string> SplitList(string StringToSplit, char[] delimiterChars)
        {
            List<string> Strings = new List<string>();
            string[] ThisSplitString = StringToSplit.Split(delimiterChars);
            foreach (var i in ThisSplitString)
            {
                Strings.Add(i);
            }
            return Strings;
        }

        public static List<Int32> WordLength(List<String> AnswerList)
        {
            List<Int32> WordLength = new List<Int32>();
            foreach(var i in AnswerList)
            {
                WordLength.Add(i.Length);
            }
            return WordLength;
        }

        public static List<PuzzleCrossWord> PositionCharList(List<string> Dir, List<string> Pos, List<string> Ans, int X, int Total)
        {       
            Int32 Count = Dir.Count();
            List<PuzzleCrossWord> BoxLetterList = new List<PuzzleCrossWord>();
            for(var i = 0; i < Count - 1; i++)
            {
                bool NumberAdded = false;
                Int32 ThisPosition = Convert.ToInt32(Pos[i]);
                if(Dir[i] == "Across")
                {
                    char[] Characters = Ans[i].ToCharArray();
                    foreach (var l in Characters)
                    {
                        PuzzleCrossWord B = new PuzzleCrossWord();                        
                        B.Position = ThisPosition;
                        B.Character = l.ToString().ToUpper();
                        ThisPosition += 1;
                        if (NumberAdded == false)
                        {
                            NumberAdded = true;
                            B.Question = i;
                        }
                        bool AlreadyAddedInfo = BoxLetterList.Any(x => x.Position == B.Position);                        
                        if (AlreadyAddedInfo == false)
                        {
                            BoxLetterList.Add(B);
                        }                       
                    }
                }
                else
                {
                    char[] Characters = Ans[i].ToCharArray();
                    foreach (var l in Characters)
                    {
                        PuzzleCrossWord B = new PuzzleCrossWord();
                        B.Position = ThisPosition;
                        B.Character = l.ToString().ToUpper();
                        ThisPosition += X;
                        if (NumberAdded == false)
                        {
                            NumberAdded = true;
                            B.Question = i;
                        }
                        bool AlreadyAddedInfo = BoxLetterList.Any(x => x.Position == B.Position);
                        if (AlreadyAddedInfo == false)
                        {
                            BoxLetterList.Add(B);
                        }                        
                    }
                }  
            }
            return BoxLetterList;
        }

        public static List<string> AcrossList(List<string> Clues, List<int> Length, List<string> Directions)
        {
            List<string> AList = new List<string>();
            for (var i = 0; i< Directions.Count() - 1; i++)
            {
                if(Directions[i] == "Across")
                {
                    string QuestionsString = Clues[i] + ". (" + Length[i].ToString() +")";
                    AList.Add(QuestionsString);
                }
            }
            return AList;
        }

        public static List<string> DownList(List<string> Clues, List<int> Length, List<string> Directions)
        {
            List<string> DList = new List<string>();
            for (var i = 0; i < Directions.Count() - 1; i++)
            {
                if (Directions[i] == "Down")
                {
                    string QuestionsString = Clues[i] + ". (" + Length[i].ToString() + ")";
                    DList.Add(QuestionsString);
                }
            }
            return DList;
        }
    }
}