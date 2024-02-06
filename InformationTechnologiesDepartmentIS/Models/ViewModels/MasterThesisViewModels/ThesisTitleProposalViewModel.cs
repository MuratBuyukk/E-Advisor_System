using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisTitleProposalViewModel
    {
        public ThesisViewModel MasterThesis { get; set; }
        public int ThesisId { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        public FormThesisTitleProposal Form { get; set; }
    }
}