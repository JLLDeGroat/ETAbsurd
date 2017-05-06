using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class ThreadStartModel
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Body { get; set; }

        
        public string Tags { get; set; }
        public string Subject { get; set; }
        public string Image { get; set; }

        
    }
}