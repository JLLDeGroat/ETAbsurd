using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace ETAbsurdV1.Models
{
    public class PasswordRetrievalModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Makes no sense")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        [Remote("CheckUserNameExists", "Login", ErrorMessage = "I do not remember this one")]
        public string UserName { get; set; }

        public string Code { get; set; }

        public string Section { get; set; }
    }
}