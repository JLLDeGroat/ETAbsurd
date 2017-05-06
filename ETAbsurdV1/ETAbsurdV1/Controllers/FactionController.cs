using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;
using ETAbsurdV1.Code;
using System.Data.Entity;


namespace ETAbsurdV1.Controllers
{
    [Authorize]
    public class FactionController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        // GET: Faction
        
        public ActionResult Home()
        {
            //Get User Faction
            Boolean Registered = false;
            ViewData["Faction"] = "None";
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (ThisUser.Faction != null)
            {
                ViewData["Faction"] = ThisUser.Faction;
                Registered = true;
            }
           
            if(Registered == true)
            {
                return RedirectToAction("ControlPanel", "Faction");
            }
           
            return View();
        }

        public ActionResult ControlPanel()
        {
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if(ThisUser.Faction == null)
            {
                return RedirectToAction("Home", "Faction");
            }
            string Url = Request.Url.GetLeftPart(UriPartial.Authority);
            ViewData["Faction"] = ThisUser.Faction;
            ViewData["FactionImage"] = Url + "/Faction/Logos/" + ThisUser.Faction + ".jpg";

            return View();
        }

        public ActionResult StartThread()
        {
            ViewData["Categories"] = Code.Faction.Faction.FactionCategories();
            return View();
        }

        [HttpPost]
        public ActionResult StartThread(ETA_FactionThread Thread)
        {
            ViewData["Categories"] = Code.Faction.Faction.FactionCategories();
            try
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ThisUser.Threads = ThisUser.Threads + 1;
                db.Entry(ThisUser).State = EntityState.Modified;
                db.SaveChanges();
                if (ModelState.IsValid)
                {
                    ETA_FactionThread Th = new ETA_FactionThread();
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
                    //For Faction
                    Th.Faction = ThisUser.Faction;
                    db.ETA_FactionThread.Add(Th);
                    db.SaveChanges();
                    //Return to the actual Thread thats been posted

                    var ThisThread = db.ETA_FactionThread.Where(p => p.Date.ToString().Equals(Th.Date.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    int ThisThreadId = 0;
                    foreach (var item in ThisThread)
                    {
                        if (item.Name == Th.Name & item.Body == Th.Body)
                        {
                            ThisThreadId = item.Id;
                        }
                    }
                    return RedirectToAction("Thread", "Faction", new { Id = ThisThreadId });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "FactionThread", e = e.ToString().Substring(0, 50) });
            }
                
            return View();
        }

        public ActionResult Thread(int Id)
        {
            var ThisThread = db.ETA_FactionThread.Where(p => p.Id.Equals(Id)).FirstOrDefault();
            return View();
        }

        public JsonResult UserDistribution()
        {
            List<ETA_User> AllUsers = new List<ETA_User>(db.ETA_User.Where(p => p.Faction != null));

            decimal? Theology = 1;
            decimal? Agnostic = 1;
            decimal? Narcassism = 1;
            decimal? Nihilism = 1;
            decimal? PostModernism = 1;
            decimal? Solipsism = 1;
            decimal Total = 6 + AllUsers.Count();

            foreach (var item in AllUsers)
            {
                if(item.Faction == "Theology")
                {
                    Theology += 1;
                }
                else if (item.Faction == "Agnostic")
                {
                    Agnostic += 1;
                }
                else if (item.Faction == "Narcassism")
                {
                    Narcassism += 1;
                }
                else if (item.Faction == "Nihilism")
                {
                    Nihilism += 1;
                }
                else if (item.Faction == "PostModernism")
                {
                    PostModernism += 1;
                }
                else if (item.Faction == "Solipsism")
                {
                    Solipsism += 1;
                }
            }

            Double TheologyDecimal = Convert.ToDouble(Theology / Total) * 100;
            Decimal AgnosticDecimal = Convert.ToDecimal(Agnostic / Total) * 100;
            string NarcassismDecimal = (Narcassism / Total * 100).ToString();
            Decimal NihilismDecimal = Convert.ToDecimal(Nihilism / Total) * 100;
            Decimal PostModernismDecimal = Convert.ToDecimal(PostModernism / Total) * 100;
            Decimal SolipsismDecimal = Convert.ToDecimal(Solipsism / Total) * 100;

            string Distribution = TheologyDecimal.ToString() + "|" + AgnosticDecimal.ToString() + "|" 
                + NarcassismDecimal.ToString() + "|" + NihilismDecimal.ToString() + "|"                 
                + PostModernismDecimal.ToString() + "|" + SolipsismDecimal;

            return Json(Distribution);
        }

        public JsonResult FactionDescription(string FactionName)
        {
            string ThisFaction = Code.Faction.Faction.FactionDescription(FactionName);
            return Json(ThisFaction);
        }

        public JsonResult FactionDecisionConfirmation(string FactionName)
        {
            string JsonReturn = "Fail";
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (ThisUser.Faction == null)
            {
                ThisUser.Faction = FactionName;
                db.Entry(ThisUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                JsonReturn = FactionName;
            }
            else
            {
               
            }    
            return Json(JsonReturn);
        }

        public JsonResult ControlPanelPopulation(string FactionName, int? Id)
        {
            var JsonResult = "none";
            if(Id == 0)
            {              
               return Json(Code.Faction.Faction.FactionUsers(FactionName));
            }
            else if(Id == 1)
            {
                return Json(Code.Faction.Faction.FactionThreads(FactionName));
            }
            else if(Id == 2)
            {
                return Json(Code.Faction.Faction.FactionPosts(FactionName));
            }

            return Json(JsonResult);
        }
    }
}