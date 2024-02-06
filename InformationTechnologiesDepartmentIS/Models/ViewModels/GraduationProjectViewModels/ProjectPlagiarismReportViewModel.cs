using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels
{
    public class ProjectPlagiarismReportViewModel
    {
        public ProjectViewModel GraduationProject { get; set; }
        public FormPlagiarismReport Form { get; set; }
        public int ProjectId { get; set; }
        public bool IsPlagiarised { get; set; }

    }
}