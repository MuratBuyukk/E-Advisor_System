using InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisFormsViewModel
    {
        public string AdvisorFullName { get; set; }
        public FormThesisConsultantProposal FormThesisConsultantProposal { get; set; }
        public FormThesisConcultantChangeProposal FormThesisConsultantChangeProposal { get; set; }
        public FormThesisTitleProposal FormThesisTitleProposal { get; set; }
        public FormThesisSubjectChangeProposal FormThesisSubjectChangeProposal { get; set; }
        public FormThesisConsultantAndSubjectChangeProposal FormThesisConsultantAndSubjectChange { get; set; }
        public FormThesisExtensionProposal FormThesisExtensionProposal { get; set; }
        public FormThesisAssessmentProposal FormThesisAssessmentProposal { get; set; }
        public FormThesisPlagiarismReport FormThesisPlagiarismReport { get; set; }
        public FormThesisDeliveryCertificate FormThesisDeliveryCertificate { get; set; }

    }
}