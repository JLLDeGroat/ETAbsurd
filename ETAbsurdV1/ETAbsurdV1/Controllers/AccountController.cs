using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ETAbsurdV1.Models;
using System.Data.Entity;
using System.IO;

namespace ETAbsurdV1.Controllers
{
   [Authorize]
    public class AccountController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
       
        public ActionResult ControlPanel()
        {           
            try
            {
                //Get Basic Information
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                List<ETA_User> ThisUserList = new List<ETA_User>();
                ThisUserList.Add(ThisUser);
                ViewData["LoggedUser"] = ThisUserList;
                //Get Stats
                int? TotalVotes = ThisUser.GivenUpVotes + ThisUser.GivenDownVotes;
                ViewData["TotalVotes"] = TotalVotes;
                //Check if AVATAR IS NULL
                if (ThisUser.Avatar != null)
                {
                    ThisUser.Avatar = Code.Account.Account.AvatarUrl(ThisUser.Avatar, User.Identity.Name);
                }
                //shorten Down Reputation as it currently has 10 decimals                
                ThisUser.Reputation = Code.Algorithm.Reputation.CutReputation(ThisUser.Reputation);       
                //Followed Section                
                ViewData["Followed"] = Code.Account.Account.FollowedBlogList(db.ETA_Blog.ToList(), ThisUser.Following);
                //This User BLogs
                ViewData["UserBlog"] = Code.Account.Account.BlogList(db.ETA_Blog.ToList() ,User.Identity.Name);
                //Karma
                ViewData["KarmaPercentage"] = Code.Algorithm.Reputation.NewKarma(ThisUser.KarmaUp, ThisUser.KarmaDown).ToString(); 
                //Get Activity
                //Post & Thread Tables to be created here...
                //Threads
                ViewData["Threads"] = Code.Account.Account.ThreadList(db.ETA_Thread.ToList(), User.Identity.Name);
                //Posts
                ViewData["Posts"] = Code.Account.Account.PostList(db.ETA_Post.ToList(), User.Identity.Name);
                //Comments
                ViewData["Comments"] = Code.Account.Account.CommentList(db.ETA_Comment.ToList(), User.Identity.Name);
                //Gifs
                ViewData["Gifs"] = Code.Account.Account.GifList(User.Identity.Name);
                return View();
            }
            catch(Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "ControlPanel", e = e.ToString().Substring(0, 50) });
            }              
        }
        
        [HttpPost]
        public ActionResult ControlPanel(string Signature, string Avatar, string TextColor, string Font)
        {
            try
            {
                var ThisUserName = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ETA_User ThisUser = db.ETA_User.Find(ThisUserName.Id);
                if (Signature != null) // Signature change
                {
                    ThisUser.Signature = Signature;
                }
                else if (Avatar != null) // Avatar change
                {
                    ThisUser.Avatar = Avatar;
                }
                else if(TextColor != null)
                {
                    ThisUser.TextColor = TextColor;
                }
                else if(Font != null)
                {
                    ThisUser.Font = Font;
                }
                db.Entry(ThisUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ControlPanel", "Account");
            }
            catch(Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "ControlPanelChange", e = e.ToString().Substring(0, 50) });           
            }
        }
        
        public ActionResult ViewProfile(string UserName)
        {            
            if(UserName == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    //Get Basic Information
                    var ThisUser = db.ETA_User.Where(p => p.Username.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    List<ETA_User> ThisUserList = new List<ETA_User>();
                    ThisUserList.Add(ThisUser);
                    ViewData["ViewUser"] = ThisUserList;
                    //Get Stats
                    int? TotalVotes = ThisUser.GivenUpVotes + ThisUser.GivenDownVotes;
                    ViewData["TotalVotes"] = TotalVotes;
                    //Check Image for Uploading or URL COPYPASTE
                    //Check if AVATAR IS NULL
                    if (ThisUser.Avatar != null)
                    {
                        ThisUser.Avatar = Code.Account.Account.AvatarUrl(ThisUser.Avatar, UserName);
                    }
                    //shorten Down Reputation as it currently has 10 decimals                
                    ThisUser.Reputation = Code.Algorithm.Reputation.CutReputation(ThisUser.Reputation);
                    //shorten Down Reputation as it currently has 10 decimals                
                    ThisUser.Reputation = Code.Algorithm.Reputation.CutReputation(ThisUser.Reputation);
                    //Followed Section                
                    ViewData["Followed"] = Code.Account.Account.FollowedBlogList(db.ETA_Blog.ToList(), ThisUser.Following);
                    //This User BLogs
                    ViewData["UserBlog"] = Code.Account.Account.BlogList(db.ETA_Blog.ToList(), UserName);
                    //Karma
                    ViewData["KarmaPercentage"] = Code.Algorithm.Reputation.NewKarma(ThisUser.KarmaUp, ThisUser.KarmaDown).ToString();
                    //Get Activity
                    //Post & Thread Tables to be created here...
                    //Threads
                    ViewData["Threads"] = Code.Account.Account.ThreadList(db.ETA_Thread.ToList(), UserName);
                    //Posts
                    ViewData["Posts"] = Code.Account.Account.PostList(db.ETA_Post.ToList(), UserName);
                    //Comments
                    ViewData["Comments"] = Code.Account.Account.CommentList(db.ETA_Comment.ToList(), UserName);
                    //Gifs
                    ViewData["Gifs"] = Code.Account.Account.GifList(UserName);
                    return View();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Oops", "Error", new { Message = "ViewProfile", e = e.ToString().Substring(0, 50) });
                }
            }           
        }

        public void UploadImage()
        {
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();   
            string directoryMap = "~\\Uploads\\AvatarUpload\\" + User.Identity.Name;
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

        public void UploadForumImage()
        {
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string directoryMap = "~\\Uploads\\ThreadUpload\\" + User.Identity.Name;
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
    }
}