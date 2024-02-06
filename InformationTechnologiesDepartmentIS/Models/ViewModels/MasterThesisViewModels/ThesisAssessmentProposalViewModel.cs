using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisAssessmentProposalViewModel
    {
        public ThesisViewModel MasterThesis { get; set; }
        public FormThesisAssessmentProposal Form { get; set; }

        public int ThesisId { get; set; }
    }
}