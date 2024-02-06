using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels
{
    public class ProjectAdvisorViewModel
    {
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Project title is required!")]
        public string ProjectTitle { get; set; }
        [Required(ErrorMessage = "Project main domain is required!")]
        public string ProjectMainDomain { get; set; }
        [Required(ErrorMessage = "Project subdomains is required!")]
        public string ProjectSubDomains { get; set; }
        [Required(ErrorMessage = "Problem is required!.")]
        public string Problem { get; set; }
        [Required(ErrorMessage = "Solution is required!")]
        public string Solution { get; set; }
        [Required(ErrorMessage = "Thesis statement is required!")]
        public string ThesisStatement { get; set; }
        public Nullable<System.Guid> StudentUserId { get; set; }
        [Required(ErrorMessage = "Please select an advisor!")]
        public Nullable<System.Guid> AdvisorUserId { get; set; }
        public Nullable<int> ProjectStatusId { get; set; }
        public string FeedbackMessage { get; set; }
        public Nullable<System.DateTime> SubmissionDate { get; set; }
        public List<Academician> Academicians { get; set; }
        public List<Advisor> ProjectAdvisors { get; set; }
        public int Response { get; set; } = 0;

    }
}