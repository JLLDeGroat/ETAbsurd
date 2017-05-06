using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ETAbsurdV1.Models;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace ETAbsurdV1.Controllers
{
    public class ZTestController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        // GET: ZTest
        public ActionResult Index(int? Ident, string TheTag)
        {
            try { 
                if(Ident != null)
                {
                    var ClickedUser = db.ETA_User.Find(Ident);
                    db.Entry(ClickedUser).State = EntityState.Deleted;
                    db.SaveChanges();
                }

                ViewData["XXX"] = db.ETA_User.ToList();
                if (TheTag != null)
                {
                    ViewData["TheTag"] = TheTag;
                }
                else
                {
                    ViewData["TheTag"] = "NOT WORKING";
                }


                //TEST 1/3/2017 -- to create date on the forum page because it can not be null
                // THIS should also be changed on the actual sql management system so that it doesnt get screwed every build..
                var ForumList = db.ETA_Thread.ToList();
                ViewData["ThreadList"] = ForumList;


                var PostList = db.ETA_Post.ToList();
                ViewData["Posts"] = PostList;

                return View();
            }
            catch
            {
                return RedirectToAction("Index", "ZTest");
            }
        }

        public ActionResult StringGen()
        {            
            string value2 = UserTagGenerator.TagGenerator.OnRegister("Authentic");   
            return RedirectToAction("Index", "Ztest", new { TheTag = value2 });
        } 

        public ActionResult DateMaker(int ID)
        {
            var data = db.ETA_Thread.Find(ID);

            data.Date = DateTime.Now;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "ZTest");
        }

        public ActionResult DeletePost(int Id)
        {
            var ClickedPost = db.ETA_Post.Find(Id);
            db.Entry(ClickedPost).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("index", "Ztest");
        }




        public ActionResult BlogLayouts()
        {
            return View();
        }



        public ActionResult Sortie()
        {
            Random Num = new Random();


            ViewData["HD1"] = "Press Ups: " + Num.Next(10,40);
            ViewData["HD2"] = "Sit Ups: " + Num.Next(40, 100);
            ViewData["HD3"] = "Star Jumps: " + Num.Next(5,15) + " mins";
            ViewData["B1"] = "Sit Ups: " + Num.Next(40,80);
            ViewData["B2"] = "Star Jumps: " + Num.Next(5,10) + " mins";
            ViewData["B3"] = "Squats: " + Num.Next(40,80);

            return View();
        }

        public ActionResult TestMail()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("grehvous@googlemail.com");
                mail.To.Add("grehvous@googlemail.com");
                mail.Subject = "Absurdity";
                mail.Body = "<h1>You have been mentioned :D epic</h1>";
                mail.IsBodyHtml = true;
                
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {                    
                    smtp.Credentials = new NetworkCredential("grehvous@googlemail.com", "Bojangles21");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return RedirectToAction("index", "ztest");
        }


        public ActionResult DeleteVotes(int id)
        {

            ETA_Post ThisPost = db.ETA_Post.Find(id);

            ThisPost.Voters = null;
            ThisPost.UpVotes = 0;

            db.Entry(ThisPost).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Index", "ZTest");
        }   
      


        public ActionResult TestRep()
        {
            return View();
        }

        public JsonResult TestRepCalc(string Rep, string Vote)
        {
            decimal result = Convert.ToDecimal(Rep);

            var ThisUser = db.ETA_User.Where(p => p.Username.Equals("Oblongamous", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if(Vote == "up")
            {
                ThisUser.UpVotes += 1;
            }
            if(Vote == "down")
            {
                ThisUser.DownVotes += 1;
            }

            decimal CurrentRep = Convert.ToDecimal(ThisUser.Reputation);
            int? CurrentUpVotes = ThisUser.UpVotes;
            int? CurrentDownVotes = ThisUser.DownVotes;
            int? CurrentTotalVotes = CurrentUpVotes + CurrentDownVotes;

            decimal NewRep = Code.Algorithm.Reputation.NewReputation(CurrentRep, CurrentUpVotes, CurrentDownVotes, CurrentTotalVotes, Vote);            
            ThisUser.Reputation = NewRep;

            db.Entry(ThisUser).State = EntityState.Modified;
            db.SaveChanges();

            return Json(ThisUser.Reputation);
        }
        


        public JsonResult ChangeAvatar()
        {
            List<ETA_User> UserList = db.ETA_User.ToList();

            foreach (var item in UserList)
            {
                if(item.Avatar == null)
                {
                    item.Avatar = "http://www.imgion.com/images/01/awesome-cartoonism-hotdog.jpg";
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }



            return Json("Done");
        }


        public ActionResult FactionGraphics()
        {
            return View();
        }

        public ActionResult JqueryObj()
        {
            return View();
        }

        public ActionResult FurBeana()
        {
            return View();
        }

        
        public ActionResult Storage()
        {
           

            return View();
        }

        public ActionResult StoreBlogs()
        {
            var List = db.ETA_Blog.ToList();

            foreach (var i in List)
            {
                ETA_BlogStorage b = new ETA_BlogStorage();
                b.Author =          i.Author;
                b.Body =            i.Body;
                b.Category =        i.Category;
                b.Comments =        i.Comments;
                b.Date =            i.Date;
                b.EditDate =        i.EditDate;
                b.Edits =           i.Edits;
                b.Image1 =          i.Image1;
                b.Image2 =          i.Image2;
                b.Image3 =          i.Image3;
                b.KarmaDown =       i.KarmaDown;
                b.KarmaPercent =    i.KarmaPercent;
                b.KarmaTotal =      i.KarmaTotal;
                b.KarmaUp =         i.KarmaUp;
                b.KarmaVoters =     i.KarmaVoters;
                b.KarmaVotes =      i.KarmaVotes;
                b.Subject =         i.Subject;
                b.Title =           i.Title;
                b.TitleImage =      i.TitleImage;
                b.Views =           i.Views;
                db.ETA_BlogStorage.Add(b);
                db.SaveChanges();

            }

            return RedirectToAction("Storage", "Ztest");
        }
        public ActionResult ReuploadBlog()
        {
            var List = db.ETA_BlogStorage.ToList();

            foreach (var i in List)
            {
                ETA_Blog b = new ETA_Blog();
                b.Author = i.Author;
                b.Body = i.Body;
                b.Category = i.Category;
                b.Comments = i.Comments;
                b.Date = i.Date;
                b.EditDate = i.EditDate;
                b.Edits = i.Edits;
                b.Image1 = i.Image1;
                b.Image2 = i.Image2;
                b.Image3 = i.Image3;
                b.KarmaDown = i.KarmaDown;
                b.KarmaPercent = i.KarmaPercent;
                b.KarmaTotal = i.KarmaTotal;
                b.KarmaUp = i.KarmaUp;
                b.KarmaVoters = i.KarmaVoters;
                b.KarmaVotes = i.KarmaVotes;
                b.Subject = i.Subject;
                b.Title = i.Title;
                b.TitleImage = i.TitleImage;
                b.Views = i.Views;
                db.ETA_Blog.Add(b);
                db.SaveChanges();

            }

            return RedirectToAction("Storage", "Ztest");
        }
    }
}