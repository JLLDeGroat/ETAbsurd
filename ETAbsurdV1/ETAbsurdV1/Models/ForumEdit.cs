using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class ForumEdit
    {
        [Required]
        public string Body { get; set; }
    }
}