using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserTagGenerator
{
    public class TagGenerator
    {
        public static string OnRegister(string Authentication)
        {
            if (Authentication == "Authentic")
            {
                Random RandomNumber = new Random();
                string[] PotentialTags = 
                    {
                        "bare", "barren", "clean", "empty", "fresh", "new" , "pale" , "plain",  "spotless", "uncompleted", "unfilled", "unmarked",
                        "untouched", "unused","vacant","vacuous","virgin","virginal","void","white", "different", "late", "modern",  "original",
                        "recent", "state-of-the-art", "strange",  "unfamiliar", "unique", "unusual", "au courant", "cutting-edge", "dewy",
                        "dissimilar", "distinct", "fashionable", "inexperienced", "just out", "latest", "modernistic", "modish", "neoteric", "newfangled",
                        "novel", "now", "spick-and-span", "topical", "ultramodern", "unaccustomed", "uncontaminated", "unknown", "unlike", "unseasoned",
                        "unskilled", "unspoiled", "untouched", "untrained", "untried", "untrodden", "unused", "up-to-date", "virgin", "youthful",
                        "experimental", "head", "hip", "innovative", "lead", "leading-edge", "liberal", "new", "new wave", "pioneering", "progressive",
                        "radical", "state-of-the-art", "vanguard"
                    };
                int RandomTagNumber = RandomNumber.Next(0, PotentialTags.Length);
                return PotentialTags[RandomTagNumber];
            }
            else
            {
                return "Broken";
            }
        }
    }
}