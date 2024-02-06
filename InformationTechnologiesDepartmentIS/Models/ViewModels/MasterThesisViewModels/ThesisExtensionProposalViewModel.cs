using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisExtensionProposalViewModel
    {
        public ThesisViewModel MasterThesis { get; set; }
        public FormThesisExtensionProposal Form { get; set; }
        public DateTime ExtendedyDate { get; set; }
        public int ThesisId { get; set; }
    }
}