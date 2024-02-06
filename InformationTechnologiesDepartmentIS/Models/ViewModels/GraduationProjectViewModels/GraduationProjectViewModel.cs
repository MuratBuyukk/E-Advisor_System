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
        public string ProgramHead {  get; set; }
        public string ProgramName { get; set; }
        public int ProgramId { get; set; }
    }
}