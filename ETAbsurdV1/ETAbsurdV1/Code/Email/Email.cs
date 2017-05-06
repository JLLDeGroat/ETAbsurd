using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETAbsurdV1.Models;

namespace ETAbsurdV1.Code.Email
{
    public class Email
    {
        
        public static string EmailString(string UserName)
        {         
            string EmailBody = "<h1>Good Morro " + UserName + "</h1>"
                + "<article>" 
                + "You have been reference in a post about something or other, if you would like to follow the link that follows it will take you"
                + " to the relevant page where you were referenced."
                + "</article>"
                + "<h2>I'm afraid that until the site is good and proper, you wont be able to follow the link that follows until I have a set URL.</h2>"
                + "<h1>Also should make this e-mail look a bit better.........</h1>";
            return EmailBody;
        }


        public static string ContactUsString(string Query)
        {
            string EmailBody = "<h1>you have been sent the following Message: " + Query +  "</h1>";
            return EmailBody;
        }
       
    }
}