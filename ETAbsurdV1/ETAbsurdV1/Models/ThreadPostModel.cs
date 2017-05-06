using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETAbsurdV1.Models
{
    public class ThreadPostModel
    {
        [Required(ErrorMessage = "The Post content can not be blank")]
        [Remote("PostLength", "Forum", ErrorMessage = "Must not be empty")]
        public string Post { get; set; }


        public string Quote { get; set; }

    }
}