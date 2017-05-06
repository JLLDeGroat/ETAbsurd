using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ETAbsurdV1.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Not an Email")]
        [Remote("checkDuplicateEmail", "Login", ErrorMessage = "Email already exists")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "UserName")]
        [Remote("checkDuplicateUsername", "Login", ErrorMessage = "Username already exists")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Must be longer than 6 characters.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }
    }
}