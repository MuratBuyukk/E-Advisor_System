using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels
{
    public class ThesisDeliveryDateViewModel
    {
        public ThesisViewModel MasterThesis { get; set; }
        public int ThesisId { get; set; }
        [Required(ErrorMessage = "Delivery Date is required!")]
        public DateTime DeliveryDate { get; set; }
        [Required(ErrorMessage = "Project Language is required!")]
        public bool IsEnglish { get; set; }
        public FormThesisDeliveryCertificate Form { get; set; }

    }
}