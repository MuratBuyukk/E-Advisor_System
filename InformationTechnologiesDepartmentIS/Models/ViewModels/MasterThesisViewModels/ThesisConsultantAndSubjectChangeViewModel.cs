using InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisConsultantAndSubjectChangeViewModel
    {
        public ThesisViewModel MasterThesis { get; set; }
        public List<Academician> Academicians { get; set; }
        [Required(ErrorMessage = "Advisor is required!")]
        public Guid AcademicianId { get; set; }
        [Required(ErrorMessage = "Project topic is required!")]
        public string Topic { get; set; }
        public string OldTopic { get; set; }
        public int ThesisId { get; set; }
        public String Advisor { get; set; }
        public Guid OldAdvisorId { get; set; }
        public FormThesisConsultantAndSubjectChangeProposal Form{ get; set; }
    }
}