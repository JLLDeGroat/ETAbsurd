using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ETAbsurdV1.Models
{
    public class ContactUs
    {

       
        public string UserName { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "No less than 10 Characters, talk to me... I'm lonely.")]
        public string Body { get; set; }


        public string Image { get; set; }

        
        public string Reason { get; set; }


       
    }
}