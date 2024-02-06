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
    
    public partial class Advisor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Advisor()
        {
            this.Projects = new HashSet<Project>();
        }
    
        public System.Guid UserId { get; set; }
        public Nullable<int> StudentQuota { get; set; }
        public Nullable<bool> IsUGAdvisor { get; set; }
        public Nullable<bool> IsGradAdvisor { get; set; }
    
        public virtual Academician Academician { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
