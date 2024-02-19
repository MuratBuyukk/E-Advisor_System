using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels
{
    public class AddAcademicianViewModel
    {
        public List<Academician> Academicians { get; set; }
        public List<Program> Programs { get; set; }
        [Required(ErrorMessage = "Please Enter an FirstName.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter an LastName.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Enter an Email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter an Password.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter an Office No.")]
        public string Office { get; set; }
    }
}