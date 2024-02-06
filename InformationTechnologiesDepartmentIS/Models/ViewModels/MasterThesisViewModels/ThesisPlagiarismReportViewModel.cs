using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisPlagiarismReportViewModel
    {
        public ThesisViewModel MasterThesis { get; set; }
        public FormThesisPlagiarismReport Form { get; set; }
        public int ThesisId { get; set; }
        public bool IsPlagiarism { get; set; }
    }
}