using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationTechnologiesDepartmentIS.Models
{
        public class LoginModel
        {
            [Required(ErrorMessage = "Email is required!")]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Required(ErrorMessage = "Password is required!")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
