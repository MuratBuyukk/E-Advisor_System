using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels
{
    public class ProjectTitleProposalViewModel
    {
        public ProjectViewModel GraduationProject { get; set; }
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        public FormProjectTitleProposal Form { get; set; }

    }
}