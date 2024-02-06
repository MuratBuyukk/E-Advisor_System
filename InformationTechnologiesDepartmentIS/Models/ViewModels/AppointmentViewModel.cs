using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public List<Appointment> Appointments { get; set; }
        public Appointment Appointment { get; set; }
        public List<Academician> Academicians { get; set; }
        public List<Student> Students { get; set; }
        public List<AppointmentStatus> AppointmentStatus { get; set; }
        [Required(ErrorMessage = "Please select an academician.")]
        public Guid AcademicianId { get; set; }
        public string AppointmentDate { get; set; }


    }
}