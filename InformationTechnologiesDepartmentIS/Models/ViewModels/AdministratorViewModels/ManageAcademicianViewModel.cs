using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels
{
    public class ManageAcademicianViewModel
    {
        public List<Academician> Academicans { get; set; }

        public List<Program> Programs { get; set; }
    }
}