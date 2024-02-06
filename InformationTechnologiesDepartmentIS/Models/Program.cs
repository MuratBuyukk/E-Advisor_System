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
    
    public partial class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Program()
        {
            this.Academicians = new HashSet<Academician>();
            this.Students = new HashSet<Student>();
            this.FormProjectConsultantProposals = new HashSet<FormProjectConsultantProposal>();
            this.GraduationProjects = new HashSet<GraduationProject>();
            this.FormThesisConsultantProposals = new HashSet<FormThesisConsultantProposal>();
            this.MasterTheses = new HashSet<MasterThes>();
        }
    
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<System.Guid> HeadId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Academician> Academicians { get; set; }
        public virtual Department Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
        public virtual Academician Academician { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormProjectConsultantProposal> FormProjectConsultantProposals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GraduationProject> GraduationProjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormThesisConsultantProposal> FormThesisConsultantProposals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MasterThes> MasterTheses { get; set; }
    }
}