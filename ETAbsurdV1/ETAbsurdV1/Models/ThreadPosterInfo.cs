using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class ThreadPosterInfo
    {
        public string UserName { get; set; }
        public string UserTag { get; set; }
        public Nullable<int> UserPosts { get; set; }
        public Nullable<int> UserThreads { get; set; }
        public string JoinDate { get; set; }
        public Nullable<int> Votes { get; set; }
        public string Avatar { get; set; }
    }
}