using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels
{
    public class ManageFacultyViewModel
    {
        public List<Faculty> Faculties { get; set; }
        public List<Campus> Campuses { get; set; }
    }
}