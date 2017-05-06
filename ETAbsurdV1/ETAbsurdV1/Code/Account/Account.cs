using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ETAbsurdV1.Models;
using ETAbsurdV1.Code;

namespace ETAbsurdV1.Code.Account
{
    public class Account
    {
        public static string AvatarUrl(string Avatar, string UserName)
        {
            if (Avatar.Length > 12 && Avatar.Substring(0, 12) == "|{Uploaded}|")
            {
                Avatar = Avatar.Replace("|{Uploaded}|", "");
                Avatar = "\\Uploads\\AvatarUpload\\" + UserName + "\\" + Avatar;
            }
            return Avatar;
        }
        public static List<ETA_Thread> ThreadList(List<ETA_Thread> Threads, string UserName)
        {
            List<ETA_Thread> AllThreads = new List<ETA_Thread>(Threads.Where(p => p.Author.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)));
            List<ETA_Thread> ShownThreads = new List<ETA_Thread>();
            if (AllThreads != null)
            {
                int Limit = 0;
                foreach (var item in AllThreads)
                {
                    if (item.Name.Length > 10)
                    {
                        item.Name = item.Name.Substring(0, 10);
                    }
                    ShownThreads.Add(item);
                    Limit = Limit + 1;
                    if (Limit == 5)
                    {
                        break;
                    }
                }
            }
            return ShownThreads;
        }

        public static List<ETA_Post> PostList(List<ETA_Post> Posts, string UserName)
        {
            List<ETA_Post> AllPosts = new List<ETA_Post>(Posts.Where(p => p.Author.Equals(UserName,StringComparison.CurrentCultureIgnoreCase)));
            List<ETA_Post> ShownPosts = new List<ETA_Post>();
            if (AllPosts != null)
            {
                int Limit = 0;
                foreach (var item in AllPosts)
                {
                    if (item.Body.Length > 30)
                    {
                        item.Body = item.Body.Substring(0, 30);
                    }
                    ShownPosts.Add(item);
                    Limit =+ 1;
                    if (Limit == 5)
                    {
                        break;
                    }
                }
            }
            return ShownPosts;
        }

        public static List<ETA_Comment> CommentList(List<ETA_Comment> Comments, string UserName)
        {
            List<ETA_Comment> AllComments = new List<ETA_Comment>(Comments.Where(p => p.Author.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)));
            List<ETA_Comment> ShownComments = new List<ETA_Comment>();
            if (AllComments != null)
            {
                AllComments.Reverse();
                int Limit = 0;
                foreach (var item in AllComments)
                {
                    ShownComments.Add(item);
                    Limit =+ 1;
                    if (Limit == 5)
                    {
                        break;
                    }
                }
            }
            return ShownComments;
        }

        public static List<ETA_Blog> BlogList(List<ETA_Blog> AllBLogs, string UserName)
        {
            List<ETA_Blog> UserBlogs = new List<ETA_Blog>(AllBLogs.Where(p => p.Author.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)));
            UserBlogs.Reverse();
            foreach (var B in UserBlogs)
            {              
                B.Image1 = Images.ImageURL(B.Image1, "Blog", B.Author);
                B.Body = Blog.Blog.CutBlogLength(B.Body);
            }
            if (UserBlogs.Count > 3)
            {
                return UserBlogs.GetRange(0, 3);
            }
            else
            {
                return UserBlogs;
            }
        }

        public static List<ETA_Blog> FollowedBlogList(List<ETA_Blog> AllBlogs, string Following)
        {
            List<ETA_Blog> FollowedBlogs = new List<ETA_Blog>();
            if (Following != null)
            {
                char[] delimiterChars = { ',' };
                string[] Followers = Following.Split(delimiterChars);                
                foreach (var F in Followers)
                {
                    List<ETA_Blog> ThisBlog = new List<ETA_Blog>(AllBlogs.Where(p => p.Author.Equals(F, StringComparison.CurrentCultureIgnoreCase)));
                    ThisBlog.Reverse();
                    if (ThisBlog.Count() != 0)
                        foreach (var B in ThisBlog)
                        {
                            B.Image1 = Images.ImageURL(B.Image1, "Blog", B.Author);
                            //Get Latest from this User and break
                            B.Body = Blog.Blog.CutBlogLength(B.Body);                           
                            FollowedBlogs.Add(B);
                            break;
                        }
                    if (FollowedBlogs.Count() == 3)
                    {
                        break;
                    }
                }               
            }
            return FollowedBlogs;
        }

        public static List<String> GifList(string UserName)
        {
            string filepath = HttpContext.Current.Server.MapPath("~/Uploads/GifUploads/" + UserName + "/Saved/");
            DirectoryInfo d = new DirectoryInfo(filepath);

            List<String> GifList = new List<String>();
            if (d.Exists)
            {
                foreach (var file in d.GetFiles("*.gif"))
                {
                    GifList.Add("/Uploads/GifUploads/" + UserName + "/Saved/" + file.Name);
                    if (GifList.Count() == 3)
                    {
                        break;
                    }
                }
            }            
            return GifList;
        }        

    }

    public class Images
    {
        public static string ImageURL(string Image, string ImageType, string ImageAuthor)
        {
            if (Image != null)
            {
                string Directory = "";
                string SubDirectory = "";
                if (ImageType == "Blog")
                {
                    Directory = "BlogUpload";
                }
                if (ImageType == "Avatar")
                {
                    Directory = "AvatarUpload";
                }
                if (ImageType == "Gif")
                {
                    Directory = "GifUploads";
                    SubDirectory = "Saved\\";
                }
                if (Image.Length > 12 && Image.Substring(0, 12) == "|{Uploaded}|")
                {
                    Image = Image.Replace("|{Uploaded}|", "");
                    Image = "\\Uploads\\" + Directory + "\\" + ImageAuthor + "\\" + SubDirectory + Image;
                }
            }
            return Image;
        }      

    }
}