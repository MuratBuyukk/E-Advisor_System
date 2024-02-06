//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InformationTechnologiesDepartmentIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FormThesisConsultantProposal
    {
        public int FormId { get; set; }
        public Nullable<System.DateTime> FormDate { get; set; }
        public System.Guid StudentId { get; set; }
        public System.Guid AdvisorId { get; set; }
        public Nullable<System.Guid> CoAdvisorId { get; set; }
        public string Topic { get; set; }
        public Nullable<int> FormStatusId { get; set; }
        public int ProgramId { get; set; }
    
        public virtual Academician Academician { get; set; }
        public virtual Academician Academician1 { get; set; }
        public virtual FormStatus FormStatus { get; set; }
        public virtual Program Program { get; set; }
        public virtual Student Student { get; set; }
    }
}