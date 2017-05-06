using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETAbsurdV1.Models;

namespace ETAbsurdV1.Code.Faction
{
    public class Faction
    {
        private static AbsurdEntities db = new AbsurdEntities();


        public string Description { get; set; }

        public static string[] FactionCategories()
        {
            string[] CategoryArray = { "Meta", "Other", };

            return CategoryArray;
        }

        public static string FactionDescription(string FactionName)
        {
            var Desc = "";

            if(FactionName == "Theology")
            {
                Desc = "Theology in terms is to have faith or faith based structure where as a god is present in the universe who guides and controls to an extent the world,"
                    + " there are many theologies out there from large mainstream ones such as Christianity and islam however, it can expand to more unconventional faiths such as jedism or even Atheism."
                    + " A faith structure generally takes power and puts it into a usually unseen entity which ends and begins life. <br/>"
                    + " If you believe in a God or a power higher than yourself which dictates life in any such way then Theology will more than likely fit you as a faction.";
            }
            else if(FactionName == "Narcassism")
            {
                Desc = "Basic thoughts of a narcacist is that they are the most important, and that there ideals, moral compass and concepts are right and absolute in comparison to those"
                    + " around them. <br/>"
                    + " If you find yourself scoffing at peoples ideas, beliefs and attitudes and finding them to be below then you might find yourself well suited to the Narcasist faction.";
            }
            else if(FactionName == "Agnostic")
            {
                Desc = "Agnostics are on the fence, without conviction to an ideal. It could be that agnostics are truly open minded or that they are closed minded to everything and have yet"
                    + " to decide or convict to a belief system, or simply just do not need one. <br/>"
                    + " if you feel that all belief systems/structures just have not got it right or do not need one then the agnostic faction could be for you.";
            }
            else if(FactionName == "Nihilism")
            {
                Desc = "Nihilism is the held belief that everything is meaningless, that nothing exists to do a specific task and that deconstructing all previously percieved meaning could lead"
                    + " to finding the right path. <br/>"
                    + " If you feel that all you do has no meaning or purpose or that all held meaning at present should be deconstructed, then its probable that nihilism is the faction for you.";
            }
            else if(FactionName == "PostModernism")
            {
                Desc = "Post Modernism is quite popular in mainstream media, major triple AAA films such as Deadpool and popular netflix shows like bojack horseman depict post modernism by approaching"
                    + " what should be a sincere activity with both sincerity and insincerity, an anti hero in its extremes. <br/>"
                    + " If you feel that you act and feel both insincere and sincere when approached by what should be emotional intense activities then post modernism could be your faction";
            }
            else if(FactionName == "Solipsism")
            {
                Desc = "Not as well known as the other phylosophical ideas, solipsism is almost a play on the hologram theory of earth, whereby everything is created by yourself projected externally"
                    + " to create the world you live in, other entities are mere shadows of yourself manifested. <br/>"
                    + " If you are the only one who truly exists then this is the faction for you."; 
            }

            return Desc;
        }

        
        //Control Panel Population Class
        public static List<FactionUser> FactionUsers(string FactionName)
        {
            //Only retrieves 5 Users at present
            //Change Count to acheive different results
            List<FactionUser> FactionUsers = new List<FactionUser>();
            int Count = 0;
            var ThisFactionUsers = db.ETA_User.Where(p => p.Faction.Equals(FactionName, StringComparison.CurrentCultureIgnoreCase));
            foreach (var item in ThisFactionUsers)
            {
                if(Count < 5) {
                    FactionUser NewUser = new FactionUser();
                    NewUser.Username = item.Username;
                    NewUser.Avatar = item.Avatar;
                    
                    FactionUsers.Add(NewUser);
                    Count += 1;
                }
            }
            return FactionUsers;
        }

        public static List<ETA_FactionThread> FactionThreads(string FactionName)
        {
            List<ETA_FactionThread> TheseThreads = new List<ETA_FactionThread>();

            int Count = 0;
            var ThisFactionThreads = db.ETA_FactionThread.Where(p => p.Faction.Equals(FactionName, StringComparison.CurrentCultureIgnoreCase));
            foreach (var item in ThisFactionThreads)
            {
                if(Count < 5)
                {
                    TheseThreads.Add(item);
                    Count += 1;
                }
            }

            return TheseThreads;
        }

        public static List<ETA_FactionPost> FactionPosts(string FactionName)
        {
            List<ETA_FactionPost> FactionPosts = new List<ETA_FactionPost>();

            int Count = 0;
            var ThisFactionPosts = db.ETA_FactionPost.Where(p => p.Faction.Equals(FactionName, StringComparison.CurrentCultureIgnoreCase));
            foreach (var item in ThisFactionPosts)
            {
                if (Count < 5)
                {
                    FactionPosts.Add(item);
                    Count += 1;
                }
            }

            return FactionPosts;
        }
    }
}