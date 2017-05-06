using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETAbsurdV1.Models;


namespace ETAbsurdV1.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        private AbsurdEntities db = new AbsurdEntities();
        // GET: App
       
        public ActionResult CreateGif()
        {
            return View();
        }
        public JsonResult UploadGifImage()
        {
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string directoryMap = "~\\Uploads\\GifUploads\\" + User.Identity.Name;
            string targetPath = "";
            List<String> ImagesList = new List<String>();
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
                            targetPath = Path.Combine(path, fileName);
                            file.SaveAs(targetPath);
                            //Add Image to List
                            ImagesList.Add(targetPath);
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
            //New GIF
            string GifLocation = Code.App.Gif.CreateGif(ImagesList, directoryMap, User.Identity.Name);
            return Json(GifLocation);
        }
        public JsonResult ModifyGifImage()
        {
            //requested ms change
            string MS = Request["MS"];
            //
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string directoryMap = "~\\Uploads\\GifUploads\\" + User.Identity.Name;
            string targetPath = "";
            List<String> ImagesList = new List<String>();
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
                            targetPath = Path.Combine(path, fileName);
                            file.SaveAs(targetPath);
                            //Add Image to List
                            ImagesList.Add(targetPath);
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
            //New GIF
            string GifLocation = Code.App.Gif.ModifyGif(ImagesList, directoryMap, User.Identity.Name, MS, null);
            return Json(GifLocation);
        }

        public JsonResult SaveGif()
        {
            string MS = Request["MS"];            
            //
            var ThisUser = db.ETA_User.Where(p => p.Username.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string directoryMap = "~\\Uploads\\GifUploads\\" + User.Identity.Name;
            string targetPath = "";
            List<String> ImagesList = new List<String>();
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
                            targetPath = Path.Combine(path, fileName);
                            file.SaveAs(targetPath);
                            //Add Image to List
                            ImagesList.Add(targetPath);
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
            //New GIF
            string SavePath = Server.MapPath("~\\Uploads\\GifUploads\\" + User.Identity.Name + "\\Saved");
            if (Directory.Exists(SavePath))
            {
            }
            else
            {
                Directory.CreateDirectory(SavePath);
            }

            try
            {
                //string GifLocation = Code.App.Gif.SaveGifToFile(ImagesList, directoryMap, User.Identity.Name, MS);
                string GifLocation = Code.App.Gif.ModifyGif(ImagesList, directoryMap, User.Identity.Name, MS, "Save");
                return Json("true");
            }
            catch {
                return Json("false");
            }

            
        }
    }
}
