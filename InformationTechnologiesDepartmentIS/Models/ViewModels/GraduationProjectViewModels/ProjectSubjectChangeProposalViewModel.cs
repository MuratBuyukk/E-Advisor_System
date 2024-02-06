using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels
{
    public class ProjectSubjectChangeProposalViewModel
    {
        public ProjectViewModel GraduationProject { get; set; }
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Topic is required!")]
        public string Topic { get; set; }
        public string OldTopic { get; set; }
        public FormProjectSubjectChangeProposal Form { get; set; }

    }
}