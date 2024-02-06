using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels
{
    public class ProjectConsultantChangeProposalViewModel
    {
        public ProjectViewModel GraduationProject { get; set; }
        public List<Academician> Academicians { get; set; }
        [Required(ErrorMessage = "Advisor is required!")]
        public Guid AcademicianId { get; set; }
        [Required(ErrorMessage = "Project topic is required!")]
        public string Topic { get; set; }
        public string OldTopic { get; set; }
        public int ProjectId { get; set; }
        public string Advisor { get; set; }
        public Guid OldAcademicianId { get; set; }
        public FormProjectConsultantChangeProposal Form { get; set; }
    }
}