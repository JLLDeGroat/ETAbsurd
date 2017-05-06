using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class BlogStart
    {
        [Required(ErrorMessage = "The title can not be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The blog content can not be blank")]
        public string Body { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }
    }

    public class BlogEdit
    {
        [Required(ErrorMessage = "The title can not be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The blog content can not be blank")]
        public string Body { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public int Id { get; set; }
    }
}