using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;
using System.IO;

namespace ETAbsurdV1.Controllers
{
    public class BlogController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        // GET: Blog
        public ActionResult Index()
        {
            //mappedItems * 100.0 / totalItems
            List<ETA_Blog> Blog = new List<ETA_Blog>();
            foreach (var item in db.ETA_Blog.ToList())
            {
                //Determine Karma
                item.KarmaPercent = Code.Blog.Blog.KarmaPercent(item.KarmaUp, item.KarmaTotal);
                //Shorten The body if its to long
                item.Body = Code.Blog.Blog.CutBlogLength(item.Body);
                item.Image1 = Code.Account.Images.ImageURL(item.Image1, "Blog", item.Author);      
                Blog.Add(item);
            }
            //Is Current User a Blogger
            ViewData["IsBlogger"] = "false";
            if(User.Identity.IsAuthenticated)
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if(ThisUser.Blogger == true)
                {
                    ViewData["IsBlogger"] = "true";
                }
            }

            ViewData["Blogs"] = Blog;
            return View();
        }

        public ActionResult FollowedIndex(string FollowedUsers)
        {
            if(FollowedUsers == null)
            {
                return RedirectToAction("Index","Blog");
            }
            List<ETA_Blog> Blog = new List<ETA_Blog>();

            char[] delimiterChars = { ',' };
            string[] UsersFollowed = FollowedUsers.Split(delimiterChars);
            foreach(var word in UsersFollowed)
            {
                var ThisFollowedUser = db.ETA_Blog.Where(p => p.Author.Equals(word, StringComparison.CurrentCultureIgnoreCase));
                foreach(var FollowedBloggersBlog in ThisFollowedUser)
                {
                    Blog.Add(FollowedBloggersBlog);
                }
            }           
            foreach (var item in Blog.ToList())
            {
                //Determine Karma
                item.KarmaPercent = Code.Blog.Blog.KarmaPercent(item.KarmaUp, item.KarmaTotal);

                //Shorten The body if its to long
                item.Body = Code.Blog.Blog.CutBlogLength(item.Body);               
            }
            //Is Current User a Blogger
            ViewData["IsBlogger"] = "false";
            if (User.Identity.IsAuthenticated)
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (ThisUser.Blogger == true)
                {
                    ViewData["IsBlogger"] = "true";
                }
            }
            ViewData["Blogs"] = Blog;
            return View();
        }

        public ActionResult UserIndex(string UserName)
        {
            //mappedItems * 100.0 / totalItems
            List<ETA_Blog> Blog = new List<ETA_Blog>(db.ETA_Blog.Where(p => p.Author.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)));
            foreach (var item in Blog)
            {
                //Determine Karma
                item.KarmaPercent = Code.Blog.Blog.KarmaPercent(item.KarmaUp, item.KarmaTotal);
                //Shorten The body if its to long
                item.Body = Code.Blog.Blog.CutBlogLength(item.Body);
            }
            //Is Current User a Blogger
            ViewData["IsBlogger"] = "false";
            if (User.Identity.IsAuthenticated)
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (ThisUser.Blogger == true)
                {
                    ViewData["IsBlogger"] = "true";
                }
            }
            ViewData["Blogs"] = Blog;
            return View();
        }

        public ActionResult Bloggers(string Search)
        {
            List<ETA_User> Bloggers = new List<ETA_User>(db.ETA_User.Where(p => p.Blogger.Equals(true)));
            foreach (var item in Bloggers)
            {
                item.Avatar = Code.Account.Images.ImageURL(item.Avatar, "Avatar", item.Username);
            }
            ViewData["Bloggers"] = Bloggers;

            foreach (var item in Bloggers)
            {
                item.KarmaTotal = Code.Algorithm.Reputation.NewKarma(item.KarmaUp, item.KarmaDown);
                //shorten Down Reputation as it currently has 10 decimals                
                item.Reputation = Code.Algorithm.Reputation.CutReputation(item.Reputation);             
                //Find average Blog uploads
                //only if blog is not 0
                if(item.Blogs != 0)
                { 
                    double AverageBlogFrequency = (DateTime.Now - Convert.ToDateTime(item.RegisterDate)).TotalDays;
                    item.BlogFrequency = Convert.ToInt32(AverageBlogFrequency) / item.Blogs;
                }

            }
            ViewData["Followers"] = null;
            
            //check if followed if logged in
            if (User.Identity.IsAuthenticated)
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (ThisUser.Following != null)
                {
                    char[] delimiterChars = { ',' };
                    string[] Followers = ThisUser.Following.Split(delimiterChars);
                    ViewData["Followers"] = Followers;
                }
            }           
            return View();
        }

        [Authorize]
        public ActionResult NewBlog(bool? FirstBlog)
        {
            ViewData["FirstBlog"] = "false";
            if(FirstBlog == true)
            {
                ViewData["FirstBlog"] = "true";
            }

            return View();
        }


        [HttpPost]
        public ActionResult NewBlog(BlogStart ThisBlog)
        {
            ETA_Blog Blog = new ETA_Blog();
            if (ModelState.IsValid)
            {
                try
                {                    
                    Blog.Author = User.Identity.Name;
                    Blog.Date = DateTime.Now;
                    Blog.Title = ThisBlog.Name;
                    Blog.Body = ThisBlog.Body;
                    if(ThisBlog.Image1 != null)
                    {
                        Blog.Image1 = ThisBlog.Image1;
                        Blog.TitleImage = ThisBlog.Image1;
                    }
                    if(ThisBlog.Image2 != null)
                    {
                        Blog.Image2 = ThisBlog.Image2;
                    }
                    if(ThisBlog.Image3 != null)
                    {
                        Blog.Image3 = ThisBlog.Image3;
                    }
                    Blog.KarmaUp = 1;
                    Blog.KarmaDown = 1;
                    Blog.KarmaTotal = 2;
                    Blog.Comments = 0;

                    Blog.Subject = null;
                    Blog.Views = 0;

                    db.ETA_Blog.Add(Blog);
                    db.SaveChanges();                   
                    
                }
                catch (Exception e)
                {
                    return RedirectToAction("Oops", "Error", new { Message = "BlogCreation", e = e.ToString().Substring(0, 50) });
                }
            }
            else
            {
                ModelState.AddModelError("", "Blog is not valid");
            }

            var CreatedBlog = db.ETA_Blog.Where(p => p.Body.Equals(Blog.Body, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            //Change User to Blogger
            ETA_User ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            ThisUser.Blogger = true;
            ThisUser.Blogs += 1;
            ThisUser.KarmaTotal += 2;
            ThisUser.KarmaDown += 1;
            ThisUser.KarmaUp += 1;
            db.Entry(ThisUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            //Find Blog and go to it
            if(CreatedBlog.Date == Blog.Date)
            {
                return RedirectToAction("Blog", "Blog", new { Id = CreatedBlog.Id });
            }

            return RedirectToAction("Index", "Blog");
            
        }


        public ActionResult Blog(int? Id)
        {
            if(Id == null)
            {
                return RedirectToAction("Index", "Blog");
            }
            else
            {
                ETA_Blog data = db.ETA_Blog.Find(Id);
                data.Views += 1;
                db.SaveChanges();
                List<ETA_Blog> ThisBlog = new List<ETA_Blog>();

                data.TitleImage = Code.Account.Images.ImageURL(data.Image1, "Blog", data.Author);
                data.Image1 = Code.Account.Images.ImageURL(data.Image1, "Blog", data.Author);
                data.Image2 = Code.Account.Images.ImageURL(data.Image2, "Blog", data.Author);
                data.Image3 = Code.Account.Images.ImageURL(data.Image3, "Blog", data.Author);

                if (System.Text.RegularExpressions.Regex.IsMatch(data.Body, "~#Img1#~", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    data.Body = data.Body.Replace("~#Img1#~", "<<<" + data.Image1 + ">>>");
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(data.Body, "~#Img2#~", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    data.Body = data.Body.Replace("~#Img2#~", "<<<" + data.Image2 + ">>>");
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(data.Body, "~#Img3#~", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    data.Body = data.Body.Replace("~#Img3#~", "<<<" + data.Image3 + ">>>");
                }
                
                ThisBlog.Add(data);

                var OtherData = db.ETA_Blog.ToList();
                OtherData.Reverse();
                List<ETA_Blog> OtherBlogs = new List<ETA_Blog>();
                int Limit = 0; // Limit is 5
                foreach(var item in OtherData)
                {
                    if(data.Id != item.Id)
                    {
                        if (item.Image1 != null)
                        {
                            item.Image1 = Code.Account.Images.ImageURL(item.Image1, "Blog", item.Author);
                        }
                        OtherBlogs.Add(item);                        
                    }
                    Limit += 1;
                    if(Limit == 5)
                    {
                        break;
                    }
                }

                //Blog Karma Rating
                int? PosKarma = data.KarmaUp;
                int? NegKarma = data.KarmaDown;
                int? TotKarma = PosKarma + NegKarma;

                decimal PerPosKarma = (Convert.ToDecimal(PosKarma) / Convert.ToDecimal(TotKarma)) * 100;
                decimal PerNegKarma = (Convert.ToDecimal(NegKarma) / Convert.ToDecimal(TotKarma)) * 100;

                ViewData["NegKarma"] = PerNegKarma;
                ViewData["PosKarma"] = PerPosKarma;
                ViewData["ThisBlog"] = ThisBlog;
                ViewData["OtherBlogs"] = OtherBlogs;
            }            
            return View();
        }

        public ActionResult EditBlog(string Type, int? EditId)
        {
            try
            {
                var ThisBlog = db.ETA_Blog.Find(EditId);
                //Check if correct user is logged
                if(ThisBlog.Author != User.Identity.Name)
                {
                    return RedirectToAction("Blog", "Blog", new { Id = ThisBlog.Id });
                }

                BlogEdit Edit = new BlogEdit();
                Edit.Name = ThisBlog.Title;
                Edit.Body = ThisBlog.Body;
                Edit.Image1 = ThisBlog.Image1;
                Edit.Image2 = ThisBlog.Image2;
                Edit.Image3 = ThisBlog.Image3;
                Edit.Id = ThisBlog.Id;

                ViewBag.BlogBody = Edit.Body;
                return View(Edit);
            }
            catch(Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "BlogCreation", e = e.ToString().Substring(0, 50) });
            }
        }

        [HttpPost]
        public ActionResult EditBlog(BlogEdit ThisEdit, int? EditId)
        {
            try
            {               
                var ThisBlog = db.ETA_Blog.Find(EditId);
                
                ThisBlog.Image1 = ThisEdit.Image1;
                ThisBlog.Image2 = ThisEdit.Image2;
                ThisBlog.Image3 = ThisEdit.Image3;
                ThisBlog.Body = ThisEdit.Body;
                ThisBlog.Title = ThisEdit.Name;

                ThisBlog.EditDate = DateTime.Today;
                ThisBlog.Edits += 1;

                db.Entry(ThisBlog).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Blog", "Blog", new { Id = EditId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "BlogCreation", e = e.ToString().Substring(0, 50) });
            }
        }

        public JsonResult BlogComment(string Comment, int Id)
        {
            string JsonResult = "Comment Made";
            var ThisBlog = db.ETA_Blog.Find(Id);

            if (User.Identity.IsAuthenticated)
            {
                if (Comment.Length > 6)
                {
                    ThisBlog.Comments += 1;

                    ETA_Comment NewComment = new ETA_Comment();
                    NewComment.Author = User.Identity.Name;
                    NewComment.Date = DateTime.Now;
                    NewComment.Body = Comment;

                    NewComment.Blog = true;
                    NewComment.Post = false;
                    NewComment.Thread = false;
                    NewComment.Game = false;
                    NewComment.Puzzle = false;

                    NewComment.ParentId = Id;
                    NewComment.Upvotes = 0;
                    NewComment.DownVotes = 0;
                    NewComment.Votes = 0;

                    db.ETA_Comment.Add(NewComment);
                    db.SaveChanges();

                    return Json(NewComment);
                }
                else {
                    JsonResult = "Comment Must be longer than 6 characters";
                }
            }
            else {
                JsonResult = "Please log in to Comment";
            }
            return Json(JsonResult);
        }

        public JsonResult GetComments(string Id)
        {       
            var Comments = db.ETA_Comment.Where(p => p.ParentId.ToString().Equals(Id, StringComparison.CurrentCultureIgnoreCase));
            List<ETA_Comment> CommentList = new List<ETA_Comment>(Comments);
            List<ETA_Comment> ThisCommentList = new List<ETA_Comment>();
            if(CommentList.Count() != 0)
            {
               foreach(var item in CommentList)
                {
                    if(item.Blog == true)
                    {
                        ThisCommentList.Add(item);
                    }
                }
            }
            else
            {
                return Json("No Comments Available");
            }
            if(ThisCommentList.Count() == 0)
            {
                return Json("No Comments Available");
            }
            
            return Json(ThisCommentList);
        }
              

        public JsonResult BlogImage(string Code, int? Id)
        {
            string newCode = "";

            return Json(newCode);
        }

        public JsonResult KarmaVote(string Id, string Type)
        {
            var ThisBlog = db.ETA_Blog.Find(Convert.ToInt32(Id));

            string JsonReturn = "Karma Judged";
            char[] delimiterChars = { ',' };
            try
            {                
                if (ThisBlog.KarmaVoters != null)
                {
                    string[] Voters = ThisBlog.KarmaVoters.Split(delimiterChars);
                    //Check if Blog has been voted on already
                    foreach (var word in Voters)
                    {
                        if (word == User.Identity.Name)
                        {
                            JsonReturn = "You have already judged this article.";
                        }
                    }
                }
                //Check if Author is voter
                if(ThisBlog.Author == User.Identity.Name)
                {
                    JsonReturn = "You can not Judge your own article.";
                }
            }
            catch {
                JsonReturn = "Could not judge, please refresh and try again.";
            }
            //If its a new Vote, Alter Database for it
            if(JsonReturn.ToString() == "Karma Judged")
            {
                ThisBlog.KarmaVoters += User.Identity.Name + ",";
                if(Type == "Renegade")
                {
                    ThisBlog.KarmaDown += 1;
                }
                else if(Type == "Paragon")
                {
                    ThisBlog.KarmaUp += 1;
                }
                ThisBlog.KarmaTotal += 1;

                db.Entry(ThisBlog).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            if(JsonReturn == "Karma Judged")
            {
                var BlogUser = db.ETA_User.Where(p => p.Username.Equals(ThisBlog.Author, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                BlogUser.KarmaTotal += 1;
                if(Type == "Renegade")
                {
                    BlogUser.KarmaDown += 1;
                }
                else if(Type == "Paragon")
                {
                    BlogUser.KarmaUp += 1;
                }

                db.Entry(BlogUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(JsonReturn);
        }

        public void UploadImage()
        {
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string directoryMap = "~\\Uploads\\BlogUpload\\" + User.Identity.Name;
            {
                try
                {
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            if (file.ContentLength != 0)
                            {
                                string fileName = file.FileName;
                                if (fileName == "")
                                {
                                    fileName = "Photo1.png";
                                }
                                var path = Server.MapPath(directoryMap);
                                if (Directory.Exists(path))
                                {
                                }
                                else
                                {
                                    Directory.CreateDirectory(path);
                                }
                                string targetPath = Path.Combine(path, fileName);
                                file.SaveAs(targetPath);
                            }
                        }
                    }
                    else
                    {
                    }
                }
                catch
                {

                }
            }
        }


        public JsonResult FollowBlogger(string UserName)
        {
            string JsonReturn = "Followed";
            if (!User.Identity.IsAuthenticated)
            {
                return Json("Redirect");
            }

            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var ToFollow = db.ETA_User.Where(p => p.Username.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            //Maybe add the amount of followers to ETA?

            //CHeck if not already following and site messup
            char[] delimiterChars = { ',' };
            
            if (ThisUser.Following != null)
            {
                string[] AlreadyFollowing = ThisUser.Following.Split(delimiterChars);
                foreach (var Word in AlreadyFollowing)
                {
                    if(Word == UserName)
                    {
                        JsonReturn = "AlreadyFollowed";
                    }
                }
            }
            if (JsonReturn == "Followed")
            {
                ThisUser.Following += UserName + ",";
                db.Entry(ThisUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(JsonReturn);
        }

        public JsonResult UnfollowBlogger(string UserName)
        {
            string JsonReturn = "Unfollowed";

            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            char[] delimiterChars = { ',' };
            string[] AlreadyFollowing = ThisUser.Following.Split(delimiterChars);

            foreach (var word in AlreadyFollowing)
            {
                if(word == UserName)
                {
                    try
                    {
                        UserName = UserName + ",";
                        var Replacement = "";
                        ThisUser.Following = ThisUser.Following.Replace(UserName, Replacement);
                    }
                    catch {
                        JsonReturn = "Failed";
                    }
                }
            }
            db.Entry(ThisUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();



            return Json(JsonReturn);
        }
    }
}