using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels
{
    public class ManageProgramViewModel
    {
        public List<Department> Departments { get; set; }
        public List<Program> Programs { get; set; }

        public List<Academician> Academicians { get; set; }
    }
}