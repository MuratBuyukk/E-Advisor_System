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
    
    public partial class Academician
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Academician()
        {
            this.Appointments = new HashSet<Appointment>();
            this.FormProjectConsultantProposals = new HashSet<FormProjectConsultantProposal>();
            this.FormProjectConsultantChangeProposals = new HashSet<FormProjectConsultantChangeProposal>();
            this.GraduationProjects = new HashSet<GraduationProject>();
            this.Programs = new HashSet<Program>();
            this.FormThesisConsultantProposals = new HashSet<FormThesisConsultantProposal>();
            this.FormThesisConsultantProposals1 = new HashSet<FormThesisConsultantProposal>();
            this.FormThesisConcultantChangeProposals = new HashSet<FormThesisConcultantChangeProposal>();
            this.FormThesisConsultantAndSubjectChangeProposals = new HashSet<FormThesisConsultantAndSubjectChangeProposal>();
            this.MasterTheses = new HashSet<MasterThes>();
            this.MasterTheses1 = new HashSet<MasterThes>();
            this.FormProjectConsultantChangeProposals1 = new HashSet<FormProjectConsultantChangeProposal>();
            this.FormThesisConcultantChangeProposals1 = new HashSet<FormThesisConcultantChangeProposal>();
            this.FormThesisConsultantAndSubjectChangeProposals1 = new HashSet<FormThesisConsultantAndSubjectChangeProposal>();
        }
    
        public System.Guid UserId { get; set; }
        public string AcademicianFirstName { get; set; }
        public string AcademicianLastName { get; set; }
        public string AcademicianEmail { get; set; }
        public string RoomNo { get; set; }
        public Nullable<int> ProgramId { get; set; }
    
        public virtual Program Program { get; set; }
        public virtual Advisor Advisor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormProjectConsultantProposal> FormProjectConsultantProposals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormProjectConsultantChangeProposal> FormProjectConsultantChangeProposals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GraduationProject> GraduationProjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Program> Programs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormThesisConsultantProposal> FormThesisConsultantProposals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormThesisConsultantProposal> FormThesisConsultantProposals1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormThesisConcultantChangeProposal> FormThesisConcultantChangeProposals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormThesisConsultantAndSubjectChangeProposal> FormThesisConsultantAndSubjectChangeProposals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MasterThes> MasterTheses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MasterThes> MasterTheses1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormProjectConsultantChangeProposal> FormProjectConsultantChangeProposals1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormThesisConcultantChangeProposal> FormThesisConcultantChangeProposals1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormThesisConsultantAndSubjectChangeProposal> FormThesisConsultantAndSubjectChangeProposals1 { get; set; }
    }
}
