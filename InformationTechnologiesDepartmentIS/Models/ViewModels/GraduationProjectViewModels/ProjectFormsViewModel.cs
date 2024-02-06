using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels
{
    public class ProjectFormsViewModel
    {        
        public string AdvisorFullName { get; set; }
        public FormProjectConsultantProposal FormProjectConsultantProposal { get; set; }
        public FormProjectConsultantChangeProposal FormProjectConsultantChangeProposal { get; set; }
        public FormProjectSubjectChangeProposal FormProjectSubjectChangeProposal { get; set; }
        public FormProjectTitleProposal FormProjectTitleProposal { get; set; }
        public FormProjectDeliveryCertificate FormProjectDeliveryCertificate { get; set; }
        public FormPlagiarismReport FormPlagiarismReport { get; set; }
    }
}