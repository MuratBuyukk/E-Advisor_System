using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisSubjectChangeProposalViewModel
    {
        public ThesisViewModel MasterThesis { get; set; }
        public int ThesisId { get; set; }
        [Required(ErrorMessage = "Topic is required!")]
        public string Topic { get; set; }
        public string OldTopic { get; set; }
        public FormThesisSubjectChangeProposal Form { get; set; }

    }
}