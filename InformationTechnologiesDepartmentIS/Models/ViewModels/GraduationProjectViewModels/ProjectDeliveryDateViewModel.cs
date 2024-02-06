using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels
{
    public class ProjectDeliveryDateViewModel
    {
        public ProjectViewModel GraduationProject { get; set; }
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Delivery Date is required!")]
        public DateTime DeliveryDate { get; set; }
        [Required(ErrorMessage = "Project Language is required!")]
        public bool IsEnglish { get; set; }
        public FormProjectDeliveryCertificate Form { get; set; }


    }
}