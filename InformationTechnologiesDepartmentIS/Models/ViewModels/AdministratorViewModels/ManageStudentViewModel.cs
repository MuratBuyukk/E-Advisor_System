using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels
{
    public class ManageStudentViewModel
    {
        public List<Student> Students { get; set; }

        public List<Program> Programs { get; set; }

    }
}