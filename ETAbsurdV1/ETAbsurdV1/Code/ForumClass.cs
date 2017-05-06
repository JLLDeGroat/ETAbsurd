using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETAbsurdV1.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace ETAbsurdV1.Code
{
    public class ForumClass
    {
        private AbsurdEntities db = new AbsurdEntities();

        public string CategoryName { get; set; }
        public int CategoryAmount { get; set; }
        public decimal CategoryTotal { get; set; }
        public static List<ForumClass> ForumCategories(List<ETA_Thread> Threads)
        {
            List<ForumClass> CategoryArray = new List<ForumClass>();
            int? TotalThreads = Threads.Count();
            foreach (var item in Threads)
            {
                ForumClass NewCategoryClass = new ForumClass();
                if (!CategoryArray.Any(f => f.CategoryName == item.Category))
                {
                    NewCategoryClass.CategoryName = item.Category;
                    NewCategoryClass.CategoryAmount = 1;
                    CategoryArray.Add(NewCategoryClass);
                }
                else
                {
                    var ThisCategory = CategoryArray.Where(p => p.CategoryName.Equals(item.Category, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    ThisCategory.CategoryAmount += 1;                    
                }                
            }            
            //Get Percentages of each category      
            foreach (var item in CategoryArray)
            {
                item.CategoryTotal = Convert.ToDecimal((item.CategoryAmount * 100)) / Convert.ToDecimal(TotalThreads);
            }

            return CategoryArray;
        }        


        public static ThreadPosterInfo ThreadPostStats (ETA_Thread Thread, ETA_User ThisUser)
        {
            ThreadPosterInfo ThisThreadsInfo = new ThreadPosterInfo();         
            ThisThreadsInfo.UserName = ThisUser.Username;
            ThisThreadsInfo.UserTag = ThisUser.RepTag;
            ThisThreadsInfo.UserPosts = ThisUser.Posts;
            ThisThreadsInfo.JoinDate = ThisUser.RegisterDate.ToString().Substring(0, 10);
            ThisThreadsInfo.Votes = Thread.UpVotes - Thread.DownVotes;
            ThisThreadsInfo.UserThreads = ThisUser.Threads;            
            ThisThreadsInfo.Avatar = Code.Account.Images.ImageURL(ThisUser.Avatar, "Avatar", Thread.Author);
            return ThisThreadsInfo;
        }

        public static ThreadPosterInfo PostStats(ETA_Post Poster, ETA_User ThisUser)
        {
            ThreadPosterInfo ThisPostsInfo = new ThreadPosterInfo();            
            ThisPostsInfo.UserName = ThisUser.Username;
            ThisPostsInfo.UserTag = ThisUser.RepTag;
            ThisPostsInfo.UserPosts = ThisUser.Posts;
            ThisPostsInfo.JoinDate = ThisUser.RegisterDate.ToString().Substring(0, 10);
            ThisPostsInfo.UserThreads = ThisUser.Threads;
            ThisPostsInfo.Votes = ThisUser.UpVotes - ThisUser.DownVotes;
            ThisPostsInfo.Avatar = Code.Account.Images.ImageURL(ThisUser.Avatar, "Avatar", Poster.Author);
            return ThisPostsInfo;
        }
    }
}