using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ETAbsurdV1.Models
{
    public class SuggestCategoryModel
    {
        [Required]
        [Remote("CheckCategorySuggestion", "Forum", ErrorMessage = "This already exists within me")]
        public string Category { get; set; }
    }
}