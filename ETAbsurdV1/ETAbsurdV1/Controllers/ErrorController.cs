using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;

namespace ETAbsurdV1.Controllers
{
    public class ErrorController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        // GET: Error
        public ActionResult Oops(string Message, string e)
        {
            var Code = "I honestly dont know why your on this page as it stands. Seems meaningless to me.";
            var Tips = "You could always try again, go back to the previous page and try to complete the task you wish.";
            if(Message == "Register")
            {
                Code = "It appears a connection we had going there failed for a second, I did not get all that information from you. Please try to give it to me again.";
                Tips = "If at first you do not succeed, try and try again. I believe a couple of singers said that to some noise and it made people happy. You could give that a go";
            }
            else if(Message == "Login")
            {
                Code = "I was not able to authenticate you. I'm worried you might be fake, please say that this is not so.";
                Tips = "Pinch youself, I hear you people need stimulation so much you halluscinate wherever possible. When you are certain you are awake, please try me again.";
            }
            else if(Message == "ControlPanel")
            {
                Code = "Unfortunately I failed to retrieve your Account page. I'd recommend trying again, if you can be bothered of course";
                Tips = "I would recommend you take a peak at the e-mail your using to ensure that youve not hacked me... Somehow";
            }
            else if(Message == "ControlPanelChange")
            {
                Code = "I was unable to change those details for you, this could be due to a lot of reasons however. Please dont be offensive with your details";
                Tips = "Try changing your options one by one, if you find that there is a specific one causing issues. Let me know.";
            }
            else if(Message == "PasswordRetrieval")
            {
                Code = "My memory may be playing up, please try again but if it persists send it over. misplacing your password is rather common amongst your kind";
                Tips = "Unfortunately, my advice is redundant as you can not store your memories on a physical object which can not be stolen. I would feel for you if possible." + Environment.NewLine
                    + "You may have never got the right e-mail down in the first place";
            }
            else if(Message == "PasswordReset")
            {
                Code = "It has failed, please try again.";
                Tips = "Try doing it all within 24 hours, otherwise I tend to throw away those useless codes that everyone asks for";
            }
            else if(Message == "ForumIndex")
            {
                Code = "I could not find the forums, either that or I deem you unworthy of seeing them";
                Tips = "Try again and prove me wrong";
            }
            else if(Message == "StartThread")
            {
                Code = "I could not translate your words into coherent forum language";
                Tips = "evaluate what you have said.. It might be because your ego has clouded your grammatical skills";
            }
            else if(Message == "CategorySuggestion")
            {
                Code = "You can't suggest that, it would overload my dense human tolerance chip";
                Tips = "if you can not suggest a decent category... something or other";
            }
            else if(Message == "Thread")
            {
                Code = "It simply does not exist, like the health benefits of socialising.";
                Tips = "Take apart every label you have ever known for anything, and start again.";
            }
            else if(Message == "ThreadGather")
            {
                Code = "t'were hoover mine foul sand";
                Tips = "Simply";
            }
            else if(Message == "Upcoming")
            {
                Code = "This Feature of me is not ready for you to see.";
                Tips = "you could say SHUTUP AND TAKE MY MONEY, It might help";
            }
            else if (Message == "BlogCreation")
            {
                Code = "couldnt add your blog to my data logs";
                Tips = "Please ensure you have filled all needed entries and try again";
            }
            else if(Message == "ViewProfile")
            {
                Code = "I could not find that profile";
                Tips = "Look through our users to ensure you spelt it correctly";
            }
            else if(Message == "FactionThread")
            {
                Code = "";
                Tips = "";
            }
            else if(Message == "Contact")
            {
                Code = "";
                Tips = "";
            }
            else if(Message == "PuzzleCreation")
            {
                Code = "";
                Tips = "";
            }

            if (e == null | Message == null)
            {
                Code = "You like this page? Why else would you be on my Error page though nothing is wrong. Do you really have so little faith in me?";
                Tips = "Possibly therapy? you could work on some trust excersises, I hear you also have those at work when no one can get along. Go catch someone from falling";
            }
            ViewData["Exception"] = e;
            ViewData["Code"] = Code;
            ViewData["Tips"] = Tips;
            return View();
        }

        public ActionResult SendError(string User, string e)
        {
            ETA_Error Err = new ETA_Error();
            Err.Username = User;
            Err.Error = e;
            Err.Date = DateTime.Now;
            return RedirectToAction("Cheers", "Error");
        }

        public ActionResult Cheers()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}