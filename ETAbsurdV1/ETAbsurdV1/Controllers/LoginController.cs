using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ETAbsurdV1.Models;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;

namespace ETAbsurdV1.Controllers
{
    public class LoginController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();

        public ActionResult Login(bool? PasswordChanged)
        {
            ViewData["Reset"] = "";
            if(PasswordChanged == true)
            {
                ViewData["Reset"] = "PASSWORD CHANGED";
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel user, int? SentFromThread, int? SentFromBlog, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.UserName, user.Password))
                {
                    try
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                        if(SentFromThread != null){
                            return RedirectToAction("Thread", "Forum", new { Id = SentFromThread });
                        }
                        else if(SentFromBlog != null)
                        {
                            return RedirectToAction("Blog", "Blog", new { Id = SentFromBlog });
                        }
                        if(ReturnUrl != null)
                        {
                            try
                            { 
                                return Redirect(ReturnUrl);
                            }
                            catch
                            {

                            }
                        }
                        return RedirectToAction("ControlPanel", "Account");
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Error", "Oops", new { Message = "Login", Exception = e.ToString().Substring(0, 50) });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("HomePage", "Home");
        }
        
        [HttpPost]
        public ActionResult Register(RegisterModel Model, string Username, string Email, string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ETA_User NewUser = new ETA_User();
                    //Personal details
                    NewUser.Username = Username;
                    NewUser.Email = Email;
                    NewUser.Avatar = null;
                    //Encode password
                    NewUser.Password = Helpers.SHA1.Encode(Password);
                    //Date of register
                    NewUser.RegisterDate = DateTime.Now;                    
                    //Stats
                    //Votes
                    NewUser.UpVotes = 0;
                    NewUser.DownVotes = 0;
                    NewUser.GivenDownVotes = 0;
                    NewUser.GivenUpVotes = 0;
                    //Posts & Threads
                    NewUser.Threads = 0;
                    NewUser.Posts = 0;
                    //Competitions
                    NewUser.CompetitionEntries = 0;
                    NewUser.CompetitionsWon = 0;
                    NewUser.CompetitionsSecond = 0;
                    NewUser.CompetitionsThird = 0;
                    NewUser.CompetitionsTopTen = 0;
                    //Reputation
                    NewUser.Reputation = Convert.ToDecimal(0.00);
                    //Total Reputation Vote count to generate an average
                    NewUser.ReputationAverage = 1;
                    //Starting Tag
                    NewUser.RepTag = UserTagGenerator.TagGenerator.OnRegister("Authentic");
                    //Blogs & Karma
                    NewUser.Blogger = false;
                    NewUser.Blogs = 0;
                    NewUser.KarmaTotal = 2;
                    NewUser.KarmaUp = 1;
                    NewUser.KarmaDown = 1;
                    //Faction
                    NewUser.Faction = null;
                    NewUser.FactionContribution = 0;
                    NewUser.FactionDownVotes = 0;
                    NewUser.FactionUpVotes = 0;
                    NewUser.FactionPosts = 0;

                    //Create & Save
                    db.ETA_User.Add(NewUser);
                    db.SaveChanges();
                    //Authenticate to log in
                    FormsAuthentication.SetAuthCookie(Username, true);
                    return RedirectToAction("HomePage", "Home");
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Oops", new { Message = "Register", Exception = e.ToString().Substring(0, 50) });
                }
            }
            return View();
        }

        
        //Checking for duplicate entries on registration               
        public JsonResult checkDuplicateEmail(string Email)
        {
            var data = db.ETA_User.Where(p => p.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (data != null)
            {
                return Json("Sorry, I already know this email", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        //Checking for Duplicates entries on registration       
        public JsonResult checkDuplicateUsername(string UserName)
        {
            var data = db.ETA_User.Where(p => p.Username.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (data != null)
            {
                return Json("Sorry, I already know this name", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }                     
        //Checking if the Username they are after is on the system on PASSWORDRETRIEVAL        
        public JsonResult CheckUserNameExists(string UserName)
        {
            var data = db.ETA_User.Where(p => p.Username.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (data == null)
            {
                return Json("Sorry, I do not remember this one", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        //They have forgotten their password and want to start retrieving it again
           
        public ActionResult PasswordRetrieval(string Email, string UserName, string GotCode, string Code)
        {
            //This will show standard non-used page

            PasswordRetrievalModel SectionOfUser = new PasswordRetrievalModel();
            SectionOfUser.Section = "PartOne";
            
            //They have entered the code and are going to reset their password page
            if(Code != null)
            {
                var ThisUser = db.ETA_User.Where(p => p.PasswordRetrievalCode.Equals(Code, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if(ThisUser != null)
                {
                    return RedirectToAction("ResetPassword", "Login", new { ValidReset = true, UserToReset = ThisUser.Username});
                }
            }
            //
            //Send the e-mail with the code as they have filled out either username or email
            string HasSent = "False";
            if (GotCode != "Yes" & UserName != null | Email != null)
            {                
                if (ModelState.IsValid)
                {
                    try
                    {
                        if(Email == null)
                        {
                            var UserEnteredUserName = db.ETA_User.Where(p => p.Username.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                            Email = UserEnteredUserName.Email;
                        }
                        //Create Random Code for re-entry on site
                        Random RandomNumber = new Random();
                        string CodeToSend = RandomNumber.Next(99999999, 999999999).ToString();

                        //Send code to the email provided, they have to log in to get it (Obviously)
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("grehvous@googlemail.com");
                        mail.To.Add(Email);
                        mail.Subject = "Your password reset notification";
                        mail.Body = "Nice, quick and simple e-mail to let you know you have asked for a password reset from me (www.ETAbsurd.com)" + Environment.NewLine
                                + "It is: " + CodeToSend;
                        SmtpClient smtp = new SmtpClient("smtp.1and1.com");
                        smtp.Credentials = new NetworkCredential("jay.degroat@lombardestates.com", "Bojangles21");
                        smtp.Send(mail);

                        //Add Retrieval code to the database for password recovery
                        var ThisUser = db.ETA_User.Where(p => p.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        ThisUser.PasswordRetrievalCode = CodeToSend;
                        db.Entry(ThisUser).State = EntityState.Modified;
                        db.SaveChanges();

                        //confirm that has been sent
                        HasSent = "True";
                        //Send to Part 2
                        SectionOfUser.Section = "PartTwo";
                    }
                    catch (Exception e)
                    {
                        //there was an error and gone to error page
                        return RedirectToAction("Oops", "Error", new { Message = "PasswordRetrieval", e = e.ToString().Substring(0, 50) });
                    }
                }
                else
                {
                    //Model didnt work
                    ModelState.AddModelError("", "I can not get it to work");
                }
            }
            else if(GotCode == "Yes")
            {
                //They clicked on got code already and will show the code section to enter that in
                SectionOfUser.Section = "PartTwo";
            }

            List<PasswordRetrievalModel> SectionUserIsOn = new List<PasswordRetrievalModel>();
            SectionUserIsOn.Add(SectionOfUser);
            ViewData["Section"] = SectionUserIsOn;
            ViewData["HasSent"] = HasSent; 
            return View();
        }

        public ActionResult ResetPassword(bool? ValidReset, string UserToReset)
        {
            if(ValidReset == true)
            {
                ViewData["ThisUser"] = UserToReset;
                return View();
            }
            return RedirectToAction("Homepage", "Home");
        }        
        [HttpPost]
        public ActionResult ResetPassword(string Password, string ConfirmPassword, bool? ValidReset,
                                            string UserToReset)
        {
            try
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(UserToReset, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ThisUser.Password = Helpers.SHA1.Encode(Password);
                ThisUser.PasswordRetrievalCode = null;
                db.Entry(ThisUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception e)
            {
                return RedirectToAction("Oops", "Error", new { Message = "PasswordReset", e = e.ToString().Substring(0, 50) });
            }
            return RedirectToAction("Login", "Login", new { PasswordChanged = true });
        }

        //See if Faction menu should appear on top
        public JsonResult NavigationFaction()
        {
            if (User.Identity.IsAuthenticated)
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if(ThisUser.Faction != null)
                {
                    return Json(ThisUser.Faction.ToString());
                }
            }
            return Json("Fail");
        }
    }
}