using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisViewModel
    {
        public int ThesisId {  get; set; }
        public Student Student { get; set; }
        public Academician Advisor { get; set; }
        public Academician CoAdvisor { get; set; }

        public string Topic {  get; set; }
        public string Title { get; set; }
        public DateTime? DeliveryDate{ get; set; }
        public bool IsEnglish { get; set; }

        public string ProgramName { get; set; }
        public string ProgramHead { get; set; }
        public bool IsPlagiarised { get; set; } 
    }
}