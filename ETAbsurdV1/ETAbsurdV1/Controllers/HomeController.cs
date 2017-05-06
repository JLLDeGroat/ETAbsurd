using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Code;
using ETAbsurdV1.Models;

namespace ETAbsurdV1.Controllers
{
    public class HomeController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tech()
        {
            return View();
        }

        public JsonResult Techsharp(int id)
        {
            id += 1;

            ViewData["int"] = id;
            if (id >= 11)
            {
                return Json("failed");
            }            
            return Json(id);
        }

       

        public ActionResult HomePage()
        {
            ViewData["Quote"] = Homepage.HomepageQuote();

            //BLOG SECTION
            List<ETA_Blog> Blog = new List<ETA_Blog>();
            //Revearse for most recent
            var TheseBlogs = db.ETA_Blog.ToList();
            TheseBlogs.Reverse();
            foreach (var item in TheseBlogs)
            {
                //get Image Location
                item.Image1 = Code.Account.Images.ImageURL(item.Image1, "Blog", item.Author);
                //Determine Karma
                item.KarmaPercent = Code.Blog.Blog.KarmaPercent(item.KarmaUp, item.KarmaTotal);
                //Shorten The body if its to long
                item.Body = Code.Blog.Blog.CutBlogLength(item.Body);
                //Images                
                item.Image1 = Code.Account.Images.ImageURL(item.Image1, "Blog", item.Author);
                Blog.Add(item);
                if (Blog.Count == 5)
                {
                    break;
                }
            }
            ViewData["Blogs"] = Blog;


            //THREAD SECTION
            var TheseThreads = db.ETA_Thread.ToList();
            TheseThreads.Reverse();
            List<ETA_Thread> Threads = new List<ETA_Thread>();
            foreach (var item in TheseThreads)
            {
                item.Votes = item.UpVotes - item.DownVotes;
                item.AuthorAvatar = Code.Account.Images.ImageURL(item.AuthorAvatar, "Avatar", item.Author);
                Threads.Add(item);
                if (Threads.Count() == 5)
                {
                    break;
                }
            }
            ViewData["Threads"] = Threads;

            //PUZZLE SECTION
            var ThesePuzzles = db.ETA_Puzzles.ToList();
            ThesePuzzles.Reverse();
            List<ETA_Puzzles> Puzzles = new List<ETA_Puzzles>();
            foreach (var item in ThesePuzzles)
            {
                Puzzles.Add(item);
                if (Puzzles.Count() == 5)
                {
                    break;
                }
            }
            ViewData["Puzzles"] = Puzzles;

            //CrossWord Sections
            var TheseCrossWords = db.ETA_CrossWord.ToList();
            TheseCrossWords.Reverse();
            List<ETA_CrossWord> CrossWord = new List<ETA_CrossWord>();
            foreach (var item in TheseCrossWords)
            {
                CrossWord.Add(item);
                if (CrossWord.Count() == 3)
                {
                    break;
                }
            }
            ViewData["CrossWords"] = CrossWord;

            return View();
        }




    }

}