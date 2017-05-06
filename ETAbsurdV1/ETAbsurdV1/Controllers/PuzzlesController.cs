using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;
using ETAbsurdV1.Code;
using System.IO;

namespace ETAbsurdV1.Controllers
{
    public class PuzzlesController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        
        // GET: Puzzles
        public ActionResult Index()
        {        
            List<ETA_Puzzles> Puzzles = db.ETA_Puzzles.ToList();
            Puzzles.Reverse();
            List<ETA_Puzzles> SortedPuzzles = new List<ETA_Puzzles>();
            foreach(var item in Puzzles)
            {
                SortedPuzzles.Add(item);
                if(SortedPuzzles.Count() == 3)
                {
                    break;
                }
            }

            List<ETA_CrossWord> CrossWords = db.ETA_CrossWord.ToList();
            CrossWords.Reverse();
            List<ETA_CrossWord> SortedCrossWords = new List<ETA_CrossWord>();
            foreach(var item in CrossWords)
            {
                SortedCrossWords.Add(item);
                if(SortedCrossWords.Count() == 3)
                {
                    break;
                }
            }

            ViewData["Puzzles"] = SortedPuzzles;
            ViewData["CrossWords"] = SortedCrossWords;


            return View();
        }

        public ActionResult Puzzle(int? Id, string Category)
        {
            List<ETA_Puzzles> ThisPuzzle = new List<ETA_Puzzles>();
            ThisPuzzle.Add(db.ETA_Puzzles.Where(p => p.Id.ToString().Equals(Id.ToString(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            foreach(var item in ThisPuzzle)
            {
                item.Votes = item.UpVotes - item.DownVotes;
            }
            ViewData["Puzzle"] = ThisPuzzle;
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewData["User"] = User.Identity.Name;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(PuzzleStart ThisPuzzle)
        {
            if(ThisPuzzle.Category != null)
            {
                return RedirectToAction("Creation", "Puzzles", new { Type = ThisPuzzle.ChosenCategory });
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Creation(string Type)
        {
            string PuzzleStyle = "";
            if (Type != null)
            {                
                Type = Type.ToLower();
                if (Type == "riddle" | Type == "question" | Type == "lateral thinking" | Type == "logic" | Type == "mathematic")
                {
                    PuzzleStyle = "Normal";
                }
                else if (Type == "crossword")
                {
                    return RedirectToAction("CrossWordCreate", "Puzzles");
                }
                else
                {
                    PuzzleStyle = "None";
                }

                if (PuzzleStyle == "None")
                {
                    return RedirectToAction("Create", "Puzzles");
                }
            }
            else
            {
                return RedirectToAction("Create", "Puzzles");
            }
            ViewData["Style"] = PuzzleStyle;
            ViewData["Puzzle"] = Type;
            return View();
        }

        [HttpPost]
        public ActionResult Creation (Puzzle ThisPuzzle, PuzzleStart ThisPuzzleCategory)
        {
            try
            {
                ETA_Puzzles NewPuzzle = new ETA_Puzzles();
                NewPuzzle.Name = ThisPuzzle.Name;
                NewPuzzle.Body = ThisPuzzle.Body;
                NewPuzzle.Answer = ThisPuzzle.Answer;

                NewPuzzle.Category = ThisPuzzleCategory.ChosenCategory;

                NewPuzzle.Author = User.Identity.Name;
                NewPuzzle.UpVotes = 1;
                NewPuzzle.DownVotes = 1;
                NewPuzzle.Votes = 2;
                NewPuzzle.Date = DateTime.Now;

                db.ETA_Puzzles.Add(NewPuzzle);
                db.SaveChanges();

                var PuzzleList = db.ETA_Puzzles.Where(p => p.Author.Equals(User.Identity.Name));
                foreach(var item in PuzzleList)
                {
                    if(item.Name == NewPuzzle.Name & item.Date == NewPuzzle.Date)
                    {
                        return RedirectToAction("Puzzle", "Puzzles", new { Id = item.Id, Category = item.Category });
                    }
                }
                
            }
            catch (Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "PuzzleCreation", e = e.ToString().Substring(0, 50) });
            }
            return View();
        }

        [Authorize]
        public ActionResult Edit(int? Id, string Category)
        {
            ViewData["Style"] = "Normal";
            if (Id == null)
            {
                return RedirectToAction("Homepage", "Home");
            }
            var Puzzle = db.ETA_Puzzles.Find(Id);
            if (User.Identity.Name != Puzzle.Author)
            {
                return RedirectToAction("HomePage", "Home");
            }
            if (Category == "CrossWord")
            {
                ViewData["Style"] = "Crossword";
            }
            return View(Puzzle);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int? Id, Puzzle ThisPuzzle)
        {
            var Puzzle = db.ETA_Puzzles.Find(Id);
            Puzzle.Name = ThisPuzzle.Name;
            Puzzle.Body = ThisPuzzle.Body;
            Puzzle.Answer = ThisPuzzle.Answer;

            db.Entry(Puzzle).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Puzzle", "Puzzles", new { Id = Puzzle.Id, Category = Puzzle.Category });
        }
        [Authorize]
        public ActionResult CrossWordCreate()
        {
            return View();
        }

        public ActionResult CrossWord(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Oops", "Error");
            }
            char[] delimiterChars = { '|' };
            ETA_CrossWord CW = db.ETA_CrossWord.Find(Id);
            ViewData["Across"] = Code.Puzzle.PuzzleCrossWord.AcrossList(
                Code.Puzzle.PuzzleCrossWord.SplitList(CW.Clues, delimiterChars),
                Code.Puzzle.PuzzleCrossWord.WordLength(Code.Puzzle.PuzzleCrossWord.SplitList(CW.Answers, delimiterChars)),
                Code.Puzzle.PuzzleCrossWord.SplitList(CW.Directions, delimiterChars)
                );
            ViewData["Down"] = Code.Puzzle.PuzzleCrossWord.DownList(
                Code.Puzzle.PuzzleCrossWord.SplitList(CW.Clues, delimiterChars),
                Code.Puzzle.PuzzleCrossWord.WordLength(Code.Puzzle.PuzzleCrossWord.SplitList(CW.Answers, delimiterChars)),
                Code.Puzzle.PuzzleCrossWord.SplitList(CW.Directions, delimiterChars)
                );

            ViewData["TotalSquares"] = CW.GridTotal;
            ViewData["SquaresX"] = CW.GridX;
            ViewData["SquaresY"] = CW.GridY;
            ViewData["Name"] = CW.Name;
            ViewData["PosWord"] = Code.Puzzle.PuzzleCrossWord.PositionCharList(
                Code.Puzzle.PuzzleCrossWord.SplitList(CW.Directions, delimiterChars),
                Code.Puzzle.PuzzleCrossWord.SplitList(CW.Positions, delimiterChars),
                Code.Puzzle.PuzzleCrossWord.SplitList(CW.Answers, delimiterChars),
                CW.GridX, CW.GridTotal
                );

            return View();
        }

      
        //JSON RESULTS
        public JsonResult CreateCategories()
        {
            List<ETA_Puzzles> PuzzleList = db.ETA_Puzzles.ToList();
            //All Categories listed
            List<string> CategoryString = Code.Puzzle.Puzzle.BasePuzzleCategories();
            if (PuzzleList.Count() != 0)
            {
                List<string> MadeCategories = PuzzleList.Select(o => o.Category).ToList();
                CategoryString = Code.Puzzle.Puzzle.AdvancePuzzleCategories(CategoryString, MadeCategories);
            }
           return Json(CategoryString);
        }

        //Partial Views
        public JsonResult Description(string Key)
        {
            Key = Key.ToLower();
            if (Key == "riddle" | Key == "question" | Key == "lateral thinking" | Key == "logic" | Key == "mathematic" )
            {
                return Json("1");
            }
            
            else if(Key =="crossword")
            {
                return Json("2");
            }
            else
            {
                return Json("3");
            }
        }
        
        public JsonResult Vote(int? Id, string Vote)
        {

            string Result = "true";
            //If Not Logged Fail straight Away
            if (!User.Identity.IsAuthenticated)
            {
                return Json("N/A");
            }
            //If Its Self Vote
            var ThisPuzzle =  db.ETA_Puzzles.Find(Id);
            if(ThisPuzzle.Author == User.Identity.Name)
            {
                return Json("SelfVote");
            }

            //If Already Votes
            char[] delimiterChars = { ',' };
            if (ThisPuzzle.Voters != null)
            {
                string[] VotesAlready = ThisPuzzle.Voters.Split(delimiterChars);
                foreach (var word in VotesAlready)
                {
                    if(word == User.Identity.Name)
                    {
                        Result = "false";
                        break;
                    }
                }
            }

            //If Vote succeeds
            if(Result == "true")
            {
                if(Vote == "up")
                {
                    ThisPuzzle.UpVotes += 1;
                    ThisPuzzle.Votes += 1;
                }
                else if(Vote == "down")
                {
                    ThisPuzzle.DownVotes += 1;
                    ThisPuzzle.Votes += 1;
                }

                ThisPuzzle.Voters += User.Identity.Name + ",";

                db.Entry(ThisPuzzle).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json("Woop");
        }

        public JsonResult GetComments(int? Id)
        {
            List<ETA_Comment> AllCommentsWithId = new List<ETA_Comment>(db.ETA_Comment.Where(p => p.ParentId == Id));
            List<ETA_Comment> Comments = new List<ETA_Comment>(AllCommentsWithId.Where(p => p.Puzzle == true));
            Comments.Reverse();

            return Json(Comments);
        }

        public JsonResult PuzzleComment(string Comment, int Id)
        {
            string JsonResult = "Comment Made";
            var ThisPuzzle = db.ETA_Puzzles.Find(Id);

            if (User.Identity.IsAuthenticated)
            {
                if (Comment.Length > 6)
                {
                    ThisPuzzle.Comments += 1;

                    ETA_Comment NewComment = new ETA_Comment();
                    NewComment.Author = User.Identity.Name;
                    NewComment.Date = DateTime.Now;
                    NewComment.Body = Comment;

                    NewComment.Blog = false;
                    NewComment.Post = false;
                    NewComment.Thread = false;
                    NewComment.Game = false;
                    NewComment.Puzzle = true;

                    NewComment.ParentId = Id;
                    NewComment.Upvotes = 0;
                    NewComment.DownVotes = 0;
                    NewComment.Votes = 0;

                    db.ETA_Comment.Add(NewComment);
                    db.SaveChanges();

                    return Json(NewComment);
                }
                else
                {
                    JsonResult = "Comment Must be longer than 6 characters";
                }
            }
            else
            {
                JsonResult = "Please log in to Comment";
            }
            return Json(JsonResult);
        }

        public JsonResult ReportPost(string Reason, string Justification,
                                       string Accused, string Id)
        {
            string Reported = "";
            try
            {
                string directoryMap = "~\\Documents\\Reports\\";
                string text = Accused + Environment.NewLine +
                    Reason + Environment.NewLine +
                    Justification + Environment.NewLine
                    + Id + Environment.NewLine
                    + Environment.NewLine;
                var path = Server.MapPath(directoryMap);
                if (Directory.Exists(path))
                { }
                else
                {
                    Directory.CreateDirectory(path);
                }
                string targetPath = Path.Combine(path, "Puzzle.txt");
                System.IO.File.WriteAllText(targetPath, text);
                Reported = "This User has been Reported";
            }
            catch
            {
                Reported = "Failed to report this";
            }

            return Json(Reported);
        }

        public JsonResult AnswerPuzzle(string Answer, string Id)
        {
            var ThisPuzzle = db.ETA_Puzzles.Find(Convert.ToInt32(Id));
            Answer = Answer.ToLower();
            string ThisPuzzlesAnswer = ThisPuzzle.Answer.ToLower();

            if(Answer == ThisPuzzlesAnswer)
            {
                char[] delimiterChars = { ',' };
                bool FirstTime = true;
                if(ThisPuzzle.Completed != null)
                {
                    string[] CompletedUsers = ThisPuzzle.Completed.Split(delimiterChars);
                    foreach(var word in CompletedUsers)
                    {
                        if(word == User.Identity.Name)
                        {
                            FirstTime = false;
                        }
                    }
                    if (FirstTime == true)
                    {
                        ThisPuzzle.Completed += User.Identity.Name + ",";
                    }
                }
                else
                {
                    ThisPuzzle.Completed += User.Identity.Name + ",";
                }   
                db.Entry(ThisPuzzle).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json("Success");
            }
            else
            {
                return Json("Not Correct");
            }            
        }

        public JsonResult CrossWordSubmitted(CrossWordInput CrossWord)
        {
            try
            {
                ETA_CrossWord CW = new ETA_CrossWord();

                CW.Name = CrossWord.Name;
                CW.GridX = CrossWord.X;
                CW.GridY = CrossWord.Y;
                CW.GridTotal = CrossWord.TotalSquares;
                CW.Questions = CrossWord.Q.Count();
                CW.Author = User.Identity.Name;
                CW.Date = DateTime.Now;
                foreach (var item in CrossWord.Q)
                {
                    CW.Clues += item.Number + "." + " " + item.Clue + "|";
                    CW.Answers += item.Word + "|";
                    CW.Directions += item.Direction + "|";
                    CW.Positions += item.Position + "|";
                }

                db.ETA_CrossWord.Add(CW);
                db.SaveChanges();

                var ThisCrossWord = db.ETA_CrossWord.Where(p => p.Answers.Equals(CW.Answers, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                return Json(ThisCrossWord.Id);
            }
            catch
            {
                return Json("false");
            }
        }

        public JsonResult CrossWordAnswers(string Positions, string Characters, string Id)
        {
            char[] delimiterChars = { '|' };

            ETA_CrossWord ThisCW = db.ETA_CrossWord.Find(Convert.ToInt32(Id));
            List<Code.Puzzle.PuzzleCrossWord> ThisCrossWordChars = Code.Puzzle.PuzzleCrossWord.PositionCharList(
                Code.Puzzle.PuzzleCrossWord.SplitList(ThisCW.Directions, delimiterChars),
                Code.Puzzle.PuzzleCrossWord.SplitList(ThisCW.Positions, delimiterChars),
                Code.Puzzle.PuzzleCrossWord.SplitList(ThisCW.Answers, delimiterChars),
                ThisCW.GridX, ThisCW.GridTotal
                );

            List<Code.Puzzle.PuzzleCrossWord> InputChars = new List<Code.Puzzle.PuzzleCrossWord>();
            char[] delimiterChars2 = { ',' };
            List<string> InputPositions = Code.Puzzle.PuzzleCrossWord.SplitList(Positions, delimiterChars2);
            List<string> InputCharacters = Code.Puzzle.PuzzleCrossWord.SplitList(Characters, delimiterChars2);

            string Correct = "";
            for(var i = 0; i < InputPositions.Count() - 1; i++)
            {
                foreach(var item in ThisCrossWordChars)
                {
                    if(Convert.ToInt32(InputPositions[i]) == item.Position & InputCharacters[i] == item.Character)
                    {
                        Correct += item.Position.ToString() + ",";
                    }
                }
            }

            return Json(Correct);
        }
    }
}