using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageMagick;
using System.IO;

namespace ETAbsurdV1.Code.App
{
    public class Gif
    {

        public string Name { get; set; }
        public int Speed { get; set; }
        public static string CreateGif(List<String> Images, string DirectoryPath, string UserName)
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {                
                // Add first image and set the animation delay to 100ms
                collection.Add(Images.First());
                collection[0].AnimationDelay = 20;
                // Add second image, set the animation delay to 100ms and flip the image
                int thisword = 0;
                foreach (var word in Images)
                {
                    if (thisword != 0)
                    {
                        collection.Add(word);
                        collection[thisword].AnimationDelay = 20;                       
                    }
                    thisword += 1;
                }
                // Optionally reduce colors
                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                collection.Quantize(settings);
                // Optionally optimize the images (images should have the same size).
                collection.Optimize();
                // Save gif
                string NewGif = "~/Uploads/GifUploads/" + UserName + "/" + CreateGifName(UserName) + ".gif";
                collection.Write(HttpContext.Current.Server.MapPath(NewGif));  
                return NewGif;
            }
        }

        public static string ModifyGif(List<String> Images, string DirectoryPath, string UserName, string MS, string SAVE)
        {
            int MilliSeconds = Convert.ToInt32(MS);
            using (MagickImageCollection collection = new MagickImageCollection())
            {
                // Add first image and set the animation delay to 100ms
                collection.Add(Images.First());
                collection[0].AnimationDelay = MilliSeconds;
                // Add second image, set the animation delay to 100ms and flip the image
                int thisword = 0;
                foreach (var word in Images)
                {
                    if (thisword != 0)
                    {
                        collection.Add(word);
                        collection[thisword].AnimationDelay = MilliSeconds;
                    }
                    thisword += 1;
                }
                // Optionally reduce colors
                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                collection.Quantize(settings);
                // Optionally optimize the images (images should have the same size).
                collection.Optimize();
                // Save gif
                string NewGif = "";
                if (SAVE != "Save")
                {
                    NewGif = "~/Uploads/GifUploads/" + UserName + "/" + CreateGifName(UserName) + ".gif";
                }
                else
                {
                    NewGif = "~/Uploads/GifUploads/" + UserName + "/Saved/" + CreateGifName(UserName) + ".gif";
                }
                collection.Write(HttpContext.Current.Server.MapPath(NewGif));
                return NewGif;
            }
        }

        public static string SaveGifToFile(List<String> Images, string DirectoryPath, string UserName, string MS)
        {
            int MilliSeconds = Convert.ToInt32(MS);
            using (MagickImageCollection collection = new MagickImageCollection())
            {
                // Add first image and set the animation delay to 100ms
                collection.Add(Images.First());
                collection[0].AnimationDelay = MilliSeconds;
                // Add second image, set the animation delay to 100ms and flip the image
                int thisword = 0;
                foreach (var word in Images)
                {
                    if (thisword != 0)
                    {
                        collection.Add(word);
                        collection[thisword].AnimationDelay = MilliSeconds;
                    }
                    thisword += 1;
                }
                // Optionally reduce colors
                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                collection.Quantize(settings);
                // Optionally optimize the images (images should have the same size).
                collection.Optimize();
                // Save gif
                string NewGif = "~/Uploads/GifUploads/" + UserName + "/Saved/" + CreateGifName(UserName) + ".gif";
                collection.Write(HttpContext.Current.Server.MapPath(NewGif));
                return NewGif;
            }
        }
        public static string CreateGifName(string UserName)
        {
            string Name = DateTime.Now.ToString("MMddyyyy_hhmmsstt") + UserName;
            return Name;
        }
    }
}