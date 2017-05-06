using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class ForumSearchRefinementModel
    {
        public string SearchPhrase { get; set; }
        public string SearchTags { get; set; }
        public string SearchSubject { get; set; }
        public string SearchDate { get; set; }

        public bool Empty
        {
            get
            {
                return (string.IsNullOrWhiteSpace(SearchPhrase) &&
                        string.IsNullOrWhiteSpace(SearchTags) &&
                        string.IsNullOrWhiteSpace(SearchSubject) &&
                        string.IsNullOrWhiteSpace(SearchDate));
                }
        }
    }
}