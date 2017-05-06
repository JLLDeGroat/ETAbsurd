using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;
using ETAbsurdV1.Code;
using System.Data.Entity;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace ETAbsurdV1.Controllers
{    
    public class ForumController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        // GET: Forum
        public ActionResult Index()
        {
            try
            {
                var Threads = db.ETA_Thread.ToList();
                Threads.Reverse();
                List<ForumClass> CategoryList = ForumClass.ForumCategories(Threads);



                ViewData["CategoryList"] = CategoryList;
                ViewData["Threads"] = Threads;

                return View();
              
            }
            catch (Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "ForumIndex", e = e.ToString().Substring(0, 50) });
            }
        }

        [Authorize]
        public ActionResult Start()
        {
            var Threads = db.ETA_Thread.ToList();
            Threads.Reverse();
            ViewData["Categories"] = ForumClass.ForumCategories(Threads);
            return View();
        }

        [HttpPost]
        public ActionResult Start(ThreadStartModel Thread)
        {
            try
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ThisUser.Threads = ThisUser.Threads + 1;
                db.Entry(ThisUser).State = EntityState.Modified;
                db.SaveChanges();
                if (ModelState.IsValid)
                {
                    ETA_Thread Th = new ETA_Thread();
                    Th.Name = Thread.Name;
                    Th.Subject = Thread.Subject;
                    Th.Body = Thread.Body;
                    Th.Category = Thread.Category;
                    Th.Image = Thread.Image;
                    Th.Date = DateTime.Now;
                    Th.Tags = Thread.Tags;
                    //Author
                    Th.Author = ThisUser.Username;
                    if (ThisUser.Avatar != null)
                    {
                        Th.AuthorAvatar = ThisUser.Avatar;
                    }
                    else
                    {
                        Th.AuthorAvatar = null;
                    }
                    //Stats
                    Th.UpVotes = 0;
                    Th.DownVotes = 0;
                    Th.Views = 0;
                    Th.Votes = 0;
                    Th.EditAmounts = 0;    
                    db.ETA_Thread.Add(Th);
                    db.SaveChanges();
                    //Return to the actual Thread thats been posted

                    var ThisThread = db.ETA_Thread.Where(p => p.Author.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase));
                    int ThisThreadId = 0;
                    foreach (var item in ThisThread)
                    {
                        if(item.Name == Th.Name & item.Body == Th.Body)
                        {
                            ThisThreadId = item.Id;
                        }
                    }
                    return RedirectToAction("Thread","Forum", new { Id = ThisThreadId });
                }

                var Threads = db.ETA_Thread.ToList();
                Threads.Reverse();
                ViewData["Categories"] = ForumClass.ForumCategories(Threads);
                return View();
            }
            catch(Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "StartThread", e = e.ToString().Substring(0, 50) });
            }
        }           

        public ActionResult Category(string ClickedCategory, ForumSearchRefinementModel Refinement)
        {
            //Search Started?
            bool Refining = false;
            //Initial List
            List<ETA_Thread> TheseThreads = new List<ETA_Thread>();
            //SEARCH FROM REFINE LIST
            if (!Refinement.Empty)
            {
                Refining = true;
                
                //Initialise refined list
                List<ETA_Thread> RefinedThreads = new List<ETA_Thread>();
                List<ETA_Thread> SubRefinedThreads = new List<ETA_Thread>();                
                if (ClickedCategory == null)
                {
                    //This will show all cats or just random threads and all that jazz
                    ViewData["ClickedCategory"] = "All";
                    TheseThreads = db.ETA_Thread.ToList();
                    TheseThreads.Reverse();
                }
                else
                {
                    //Show threads that are unique to this category
                    ViewData["ClickedCategory"] = ClickedCategory;
                    var Threads = db.ETA_Thread.Where(p => p.Category.Equals(ClickedCategory, StringComparison.CurrentCultureIgnoreCase));
                    TheseThreads = new List<ETA_Thread>(Threads);
                    TheseThreads.Reverse();                    
                }                
                foreach (var item in TheseThreads)
                {
                    ETA_User ThisUser = db.ETA_User.Where(p => p.Username.Equals(item.Author, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    item.AuthorAvatar = Code.Account.Images.ImageURL(ThisUser.Avatar, "Avatar", ThisUser.Username);
                    bool MeetsDateCriteria = false;
                    bool MeetsPhraseCriteria = false;
                    bool MeetsSubRefinedCriteria = true;
                    //ensure if all is null nothing changes and no searches on that type is made
                    if (Refinement.SearchPhrase == null)
                    {
                        MeetsSubRefinedCriteria = false;
                        Refinement.SearchPhrase = item.Name;
                    }
                    /*                      
                    if(Refinement.SearchTags == null)
                    {
                        Refinement.SearchTags = item.Tags;
                    }
                    */                    
                    //SEARCH PHRASE ON NAME
                    //regular expression on phrases to ensure it gets added, will ignore capital letters
                    if (Regex.IsMatch(item.Name, Refinement.SearchPhrase, RegexOptions.IgnoreCase))
                    {
                        MeetsPhraseCriteria = true;
                    }


                    //SEARCH PHRASE ON TAGS
                    //NOT IN USE YET
                    /*
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    string[] Tags = item.Tags.Split(delimiterChars);
                    string[] SearchTags = Refine.SearchTags.Split(delimiterChars);
                    foreach(var Tag in SearchTags)
                    {
                        foreach (string x in Tags)
                        {
                            if (Tag.Contains(x))
                            {
                                MeetsCriteria = true;
                            }
                        }
                    }
                    */

                    //Refine Date SEction              
                    //Difference in days          
                    var Difference = (DateTime.Now - item.Date);
                    //If Date is null then set to this item date plus one extra day for instant true and add
                    if (Refinement.SearchDate == null)
                    {
                        Refinement.SearchDate = Difference.TotalDays.ToString();
                    }
                    if (Difference.TotalDays < Convert.ToDouble(Refinement.SearchDate + 1))
                    {
                        MeetsDateCriteria = true;
                    }
                    else
                    {
                        MeetsDateCriteria = false;
                    }

                    //Add to list or lists
                    if (MeetsDateCriteria == true & MeetsPhraseCriteria == true)
                    {
                        RefinedThreads.Add(item);
                    }
                    else if (MeetsDateCriteria == true & MeetsPhraseCriteria == false & MeetsSubRefinedCriteria == true)
                    {
                        SubRefinedThreads.Add(item);
                    }
                }
                ViewData["ThreadList"] = TheseThreads;
                ViewData["RefinedThreads"] = RefinedThreads;
                ViewData["SubRefinedThreads"] = SubRefinedThreads;

                //Nullify all refinement so that empty places are empty and not hidden refinments                
                Refinement.SearchDate = null;
                Refinement.SearchPhrase = null;
                Refinement.SearchSubject = null;
                Refinement.SearchTags = null;
            }  
            //REFINE NOT USED
            else
            {                
                if (ClickedCategory == null)
                {
                    //This will show all cats or just random threads and all that jazz
                    ViewData["ClickedCategory"] = "All";
                    TheseThreads = db.ETA_Thread.ToList();
                    TheseThreads.Reverse();                   
                }
                else
                {
                    ViewData["ClickedCategory"] = ClickedCategory;
                    var Threads = db.ETA_Thread.Where(p => p.Category.Equals(ClickedCategory, StringComparison.CurrentCultureIgnoreCase));
                    TheseThreads = new List<ETA_Thread>(Threads);
                    TheseThreads.Reverse();                    
                }               
                foreach (var item in TheseThreads)
                {
                    ETA_User ThisUser = db.ETA_User.Where(p => p.Username.Equals(item.Author, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    item.AuthorAvatar = Code.Account.Images.ImageURL(ThisUser.Avatar, "Avatar", ThisUser.Username);
                } 
            }
            ViewData["ThreadList"] = TheseThreads;
            ViewData["Refining"] = Refining;
            return View();
        } 

        public ActionResult QuickComment(string Comment, string ClickedCategory, int? Id)
        {
            return RedirectToAction("Category", "Forum", new { ClickedCategory = ClickedCategory });
        }
        
        public ActionResult CategorySuggest(string Suggested)
        {
            if(Suggested != null)
            {                
                ViewData["Suggested"] = Suggested;
            }
            else
            {
                ViewData["Suggested"] = "false";
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult CategorySuggest(SuggestCategoryModel Suggestion)
        {
            string directoryMap = "~\\Documents\\CategorySuggestions\\";
            string text = Suggestion.Category + ", ";   
            var path = Server.MapPath(directoryMap);
            if (Directory.Exists(path))
            {  }
            else
            {
                Directory.CreateDirectory(path);
            }
            string targetPath = Path.Combine(path, "Suggestions.txt");            
            System.IO.File.WriteAllText(targetPath, text);
            return RedirectToAction("CategorySuggest","Forum", new { Suggested = Suggestion.Category });
        }    

        //Thread
        public ActionResult Thread(int? Id, string Edit, int? EditId)
        {
            //Check if Edit is Clicked
            try
            {
                bool? IsEditing = IsEditValid(Edit, EditId);
                if(IsEditing == true)
                {
                    //Edit is whether it post/thread, And the id of that post/thread
                    return RedirectToAction("Edit", "Forum", new { EditType = Edit, Id = EditId });
                }
            }
            catch
            {

            }
            //Load Rest     
            try
            {
                if(Id == null)
                {
                    return RedirectToAction("Oops", "Error", new { Message = "Thread", e = "No Thread Selected" });
                }
                //Get Thread
                ETA_Thread Thread = db.ETA_Thread.Find(Id);                
                //Get User of Thread Author
                ETA_User ThreadUser = db.ETA_User.Where(p => p.Username.Equals(Thread.Author, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                //For the View In List
                List<ETA_Thread> ThisThread = new List<ETA_Thread>();
                ThisThread.Add(db.ETA_Thread.Find(Id));   
                //ThreadInfo               
                List<ThreadPosterInfo> PostInfo = new List<ThreadPosterInfo>();
                PostInfo.Add(Code.ForumClass.ThreadPostStats(Thread, ThreadUser));
                //Get Post Infos
                List<ETA_Post> Posts = new List<ETA_Post>(db.ETA_Post.Where(p => p.ThreadId.ToString().Equals(Id.ToString(), StringComparison.CurrentCultureIgnoreCase))); 
                foreach (var item in Posts)
                {   
                    ThreadPosterInfo ThisPostsInfo = new ThreadPosterInfo();
                    ETA_User PostUser = db.ETA_User.Where(p => p.Username.Equals(item.Author, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    ThisPostsInfo = Code.ForumClass.PostStats(item, PostUser);                   
                    //Only add to list if not added before// Probably most intensive function here
                    bool AlreadyAddedInfo = PostInfo.Any(x => x.UserName == ThisPostsInfo.UserName);
                    if (AlreadyAddedInfo == false)
                    {
                        PostInfo.Add(ThisPostsInfo);
                    }                                                     
                }

                //Get this user if logged
                ViewData["ThisUser"] = "NotLogged";
                if (User.Identity.IsAuthenticated)
                {
                    var Data = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    List<ETA_User> ThisUser = new List<ETA_User>();
                    ThisUser.Add(Data);
                    ViewData["ThisUser"] = ThisUser;
                }

                ViewData["ThreadId"] = Thread.Id;
                ViewData["ThreadName"] = Thread.Name;
                ViewData["ThreadCategory"] = Thread.Category;
                ViewData["Thread"] = ThisThread;
                ViewData["Posts"] = Posts;
                ViewData["PostInfo"] = PostInfo;
            }
            catch (Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "ThreadGather", e = e.ToString().Substring(0, 50) });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Thread(int? Id, ThreadPostModel Text)
        {
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            ETA_User ThisUserAccount = db.ETA_User.Find(ThisUser.Id);
            ThisUserAccount.Posts = ThisUserAccount.Posts + 1;
            

            ETA_Post NewPost = new ETA_Post();

            NewPost.Author = User.Identity.Name;
            NewPost.AuthorAvatar = ThisUser.Avatar;
            NewPost.Date = DateTime.Now;
            NewPost.Body = Text.Post;
            NewPost.Quote = Text.Quote;
            NewPost.ThreadId = Convert.ToInt32(Id);
            //Post Stats
            NewPost.Votes = 0;
            NewPost.UpVotes = 0;
            NewPost.DownVotes = 0;

            CheckIfUserIsMentioned(Text);

            db.ETA_Post.Add(NewPost);
            db.SaveChanges();
            return RedirectToAction("Thread", "Forum", new { Id = Id });
        }


        public ActionResult Edit(string EditType, int? Id)
        {
            List<ETA_Thread> ThisThread = new List<ETA_Thread>();
            List<ETA_Post> ThisPost = new List<ETA_Post>();
            try
            {              
                if(EditType == "Thread")
                {
                    var data = db.ETA_Thread.Where(p => p.Id.ToString().Equals(Id.ToString(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    ThisThread.Add(data);
                }
                else if(EditType == "Post")
                {
                    var data = db.ETA_Post.Where(p => p.Id.ToString().Equals(Id.ToString(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    ThisPost.Add(data);                    
                }
                //More Authorisation Checks
                if(ThisThread != null)
                {
                    foreach (var item in ThisThread)
                    {
                        if (item.Author != User.Identity.Name)
                        {
                            return RedirectToAction("Oops", "Error", new { Message = "ForumEdit", e = "Invalid" });
                        }
                    }
                }
                if(ThisPost != null)
                {
                    foreach (var item in ThisPost)
                    {
                        if (item.Author != User.Identity.Name)
                        {
                            return RedirectToAction("Oops", "Error", new { Message = "ForumEdit", e = "Invalid" });
                        }
                    }
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "ForumEdit", e = e.ToString().Substring(0,50) });
            }


            ViewData["Thread"] = ThisThread;
            ViewData["Post"] = ThisPost;
            return View();
        }

        [HttpPost]
        public ActionResult Edit (ForumEdit Edit, string EditType, int? Id)
        {
            try
            {
                if(EditType == "Thread")
                {
                    var ThisThread = db.ETA_Thread.Find(Id);
                    ThisThread.Body = Edit.Body;

                    ThisThread.Edited = DateTime.Now;
                    ThisThread.EditAmounts += 1;
                    ThisThread.Edit = Edit.Body;

                    db.Entry(ThisThread).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if(EditType == "Post")
                {
                    var ThisPost = db.ETA_Post.Find(Id);
                    ThisPost.Body = Edit.Body;

                    ThisPost.Edited = DateTime.Now;
                    ThisPost.EditAmounts += 1;
                    ThisPost.Edit = Edit.Body;

                    db.Entry(ThisPost).State = EntityState.Modified;
                    db.SaveChanges();

                    Id = ThisPost.ThreadId;
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "Edit", e = e.ToString().Substring(0, 50) });
            }
            return RedirectToAction("Thread", "Forum", new { Id = Id });
        }


        //Remote Validation on Category suggestions
        public JsonResult CheckCategorySuggestion(string Category)
        {
            //gather already known categories
            var Threads = db.ETA_Thread.ToList();
            Threads.Reverse();
       
            List<ForumClass> KnownCategory = ForumClass.ForumCategories(Threads);
            bool found = false;
            //loopto find a match
            foreach(var item in KnownCategory)
            {
                if(Category.Trim() == item.CategoryName.ToLower())
                {
                    found = true;
                }               
            }
            // if match true fail, if match false go ahead
            if (found == true)
            {
                return Json("This already exists within me", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        //Remote Validation on Post lenght
        public JsonResult PostLength(string Post)
        {
            if (Post.Length <= 0)
            {
                return Json("Sorry, It can not be empty", JsonRequestBehavior.AllowGet);
            }
            else if (Post.Length <= 1)
            {
                return Json("Sorry, It can not be one character", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        //Send out notifications if one is mentioned in post eg @User
        public void CheckIfUserIsMentioned(ThreadPostModel Text)
        {
            char[] delimiterChars = { ' ' };
            string[] Words = Text.Post.Split(delimiterChars);
            foreach(var word in Words)
            {
                bool ReferenceSymbol = word.Contains("@");
                if(ReferenceSymbol == true)
                {
                    string Referenced = word.Replace("@","");

                    var ThisUser = db.ETA_User.Where(p => p.Username.Equals(Referenced, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                    if(ThisUser != null)
                    {
                        //Good to send email
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("grehvous@googlemail.com");
                            mail.To.Add(ThisUser.Email);
                            mail.Subject = "Absurdity";
                            mail.Body = Code.Email.Email.EmailString(ThisUser.Username);
                            mail.IsBodyHtml = true;
                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("grehvous@googlemail.com", "Bojangles21");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            }
                        }
                    }
                }
            }
        }
        
        //Voting
        public JsonResult Vote(int? Id, string Vote, string Type)
        {
            //String to be sent to algorithm for changing reputation
            string WhoToChangeRep = "";
            string ThisResult = "true";
            char[] delimiterChars = { ',' };
            try
            {
                if (Type == "post")
                {
                    var ThisPost = db.ETA_Post.Find(Id);
                    WhoToChangeRep = ThisPost.Author;
                    if (User.Identity.Name == ThisPost.Author)
                    {
                        ThisResult = "SelfVote";
                    }
                    if (User.Identity.IsAuthenticated & User.Identity.Name != ThisPost.Author)
                    {                        
                        if (ThisPost.Voters != null)
                        {
                            string[] VotesAlready = ThisPost.Voters.Split(delimiterChars);
                            foreach (var ThisVote in VotesAlready)
                            {
                                if (ThisVote == User.Identity.Name)
                                {
                                    ThisResult = "false";
                                }
                            }
                        }
                        if (ThisResult == "true")
                        {
                            ThisPost.Voters += User.Identity.Name + ",";
                            if (Vote == "up")
                            {
                                ThisPost.UpVotes += 1;
                            }
                            else if (Vote == "down")
                            {
                                ThisPost.DownVotes += 1;
                            }
                            db.Entry(ThisPost).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                else if (Type == "thread")
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var ThisThread = db.ETA_Thread.Find(Id);
                        WhoToChangeRep = ThisThread.Author;
                        if (User.Identity.Name == ThisThread.Author)
                        {
                            ThisResult = "SelfVote";
                        }
                        if (ThisThread.Voters != null)
                        {
                            string[] VotesAlready = ThisThread.Voters.Split(delimiterChars);
                            foreach (var ThisVote in VotesAlready)
                            {
                                if (ThisVote == User.Identity.Name)
                                {
                                    ThisResult = "false";
                                }
                            }
                        }
                        if (ThisResult == "true")
                        {
                            ThisThread.Voters += User.Identity.Name + ",";
                            if (Vote == "up")
                            {
                                ThisThread.UpVotes += 1;
                            }
                            else if (Vote == "down")
                            {
                                ThisThread.DownVotes += 1;
                            }
                            db.Entry(ThisThread).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    ThisResult = "false";
                }
            }
            catch
            {
                ThisResult = "false";
            }
            if (!User.Identity.IsAuthenticated)
            {
                return Json("NotLogged");
            }
            //If true it has gone through and vote worked
            if (ThisResult == "true")
            {
                //Add to user and Re evaluate reputation
                var ThisRepChange = db.ETA_User.Where(p => p.Username.Equals(WhoToChangeRep, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                decimal CurrentRep = Convert.ToDecimal(ThisRepChange.Reputation);
                int? CurrentUpVotes = ThisRepChange.UpVotes;
                int? CurrentDownVotes = ThisRepChange.DownVotes;
                int? CurrentTotalVotes = CurrentUpVotes + CurrentDownVotes;

                decimal NewRep = Code.Algorithm.Reputation.NewReputation(CurrentRep, CurrentUpVotes, CurrentDownVotes, CurrentTotalVotes, Vote);

                if(Vote == "up")
                {
                    ThisRepChange.UpVotes += 1;
                }
                if(Vote == "down")
                {
                    ThisRepChange.DownVotes += 1;
                }
                ThisRepChange.Reputation = NewRep;
                db.Entry(ThisRepChange).State = EntityState.Modified;


                //Add has voted to The voter User
                var HasVoted = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if(Vote == "up")
                {
                    HasVoted.GivenUpVotes += 1;
                }
                if(Vote == "down")
                {
                    HasVoted.GivenDownVotes += 1;
                }

                HasVoted.GivenVoteDates = DateTime.Now.ToString() + ",";

                db.Entry(HasVoted).State = EntityState.Modified;

                db.SaveChanges();

            }
            return Json(ThisResult); 
        }
                
        //Editing and ensuring no one can edit others posts
        public bool IsEditValid(string Edit, int? EditId)
        {
            string AuthoriseThisUser = "";
            if (Edit == "Thread")
            {
                var data = db.ETA_Thread.Where(p => p.Id.ToString().Equals(EditId.ToString(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                AuthoriseThisUser = data.Author;
            }
            else if (Edit == "post")
            {
                var data = db.ETA_Post.Where(p => p.Id.ToString().Equals(EditId.ToString(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                AuthoriseThisUser = data.Author;
            }
            //Double Authorise if user can edit this.
            if (User.Identity.IsAuthenticated & User.Identity.Name == AuthoriseThisUser)
            {
                //valid Edit
                return true;
            }
            else
            {
                //non valid edit
                return false;
            }    
        }

        //Reports a Post in the Forum
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
                string targetPath = Path.Combine(path, "Reports.txt");
                System.IO.File.WriteAllText(targetPath, text);
                Reported = "This User has been Reported";
            }
            catch
            {
                Reported = "Failed to report this";
            }            

            return Json(Reported);
        }
    }
}