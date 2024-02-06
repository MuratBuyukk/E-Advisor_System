using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels
{
    public class ProjectViewModel
    {
        public Student Student { get; set; }
        public Academician Academician { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public bool IsEnglish { get; set; }
        public bool IsPlagiarised { get; set; }
        public int ProjectId { get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }
        public String ProgramName { get; set; }
        public String ProgramHead { get; set; }

    }
}