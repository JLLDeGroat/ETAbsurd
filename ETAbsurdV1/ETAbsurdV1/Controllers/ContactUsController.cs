using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;
using System.IO;
using System.Net.Mail;

namespace ETAbsurdV1.Controllers
{
    public class ContactUsController : Controller
    {

        private AbsurdEntities db = new AbsurdEntities();
        // GET: ContactUs

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactUs Query)
        {
            if (User.Identity.IsAuthenticated)
            {
                var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                Query.UserName = ThisUser.Email;
            }
            if (Query.UserName != null)
            {
                try
                {
                    //Write Query to Text File
                    string ThisQuery = Query.UserName + "|" + DateTime.Now.ToString() + Environment.NewLine
                                    + Query.Reason + Environment.NewLine
                                    + Query.Body + Environment.NewLine
                                    + Query.Image;
                    string directoryMap = "~\\Documents\\Reports\\";                    
                    var path = Server.MapPath(directoryMap);
                    if (Directory.Exists(path))
                    { }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }
                    string targetPath = Path.Combine(path, "Reports.txt");
                    System.IO.File.WriteAllText(targetPath, ThisQuery);

                    //Send the e-mail
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("grehvous@googlemail.com");
                        mail.To.Add("grehvous@googlemail.com");
                        mail.Subject = "Contact Us";
                        mail.Body = Code.Email.Email.EmailString(ThisQuery);
                        mail.IsBodyHtml = true;
                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new System.Net.NetworkCredential("grehvous@googlemail.com", "Bojangles21");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }

                    return RedirectToAction("ContactSent", "ContactUs");
                }
                catch(Exception e)
                {
                    return RedirectToAction("Oops", "Error", new { Message = "Contact", e = e.ToString().Substring(0, 50) });
                }
            }
            else
            {
                ModelState.AddModelError("", "There is something wrong.");
            }


            return View();
        }

        public ActionResult ContactSent()
        {
            return View();
        }
    }
}