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
    
    public partial class FormProjectDeliveryCertificate
    {
        public int FormId { get; set; }
        public Nullable<System.DateTime> FormDate { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public Nullable<bool> IsEnglish { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> FormStatusId { get; set; }
    
        public virtual FormStatus FormStatus { get; set; }
        public virtual GraduationProject GraduationProject { get; set; }
    }
}
