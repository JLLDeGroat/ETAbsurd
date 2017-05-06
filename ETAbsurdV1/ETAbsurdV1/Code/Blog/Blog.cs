using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Code.Blog
{
    public class Blog
    {
        //Determines and reformats if image is a copypaste URL or an uploaded file       
        public static string KarmaPercent(int KarmaUp, int Total)
        {
            string Percentage = "";
            if (Total > 0)
            {
                Percentage = ((KarmaUp * 100) / Total).ToString() + "%";
            }
            else
            {
                Percentage = "50%";
            }
            return Percentage;
        }

        public static string CutBlogLength(string Body)
        {
            if (Body.Length > 100)
            {
                Body = Body.Substring(0, 100);
            }
            return Body;
        }

    }
}