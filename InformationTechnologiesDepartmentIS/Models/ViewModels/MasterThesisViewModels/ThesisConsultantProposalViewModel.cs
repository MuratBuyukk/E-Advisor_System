using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisConsultantProposalViewModel
    {
        public List<Academician> Academicians { get; set; }
        public Student Student { get; set; }
        public String ProgramName { get; set; }
        public String ProgramHead { get; set; }
        public Guid StudentId { get; set; }
        [Required(ErrorMessage = "Advisor is required!")]
        public Guid AdvisorId { get; set; }
        public Guid CoAdvisorId { get; set; }
        [Required(ErrorMessage = "Project topic is required!")]
        public string Topic { get; set; }
        public int ProgramId { get; set; }
        public FormThesisConsultantProposal FormThesisConsultantProposal { get; set; }
        public string errorMessage { get; set; }
        public String Advisor { get; set; }
        public String CoAdvisor { get; set; }

    }
}