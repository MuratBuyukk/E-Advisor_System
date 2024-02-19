using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Repository.Concrete;
using InformationTechnologiesFacultyIS.Repository.Concrete;
using InformationTechnologiesProgramIS.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace InformationTechnologiesDepartmentIS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        StudentBusiness studentBusiness = new StudentBusiness();
        AdvisorBusiness advisorBusiness = new AdvisorBusiness();
        CampusBusiness campusBusiness = new CampusBusiness();
        FacultyBusiness facultyBusiness = new FacultyBusiness();
        DepartmentBusiness departmentBusiness = new DepartmentBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();

        // GET: Administrator
        public ActionResult Index()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);
            Guid userId = (Guid)currentUser.ProviderUserKey;
            Academician academician = academicianBusiness.GetByGuid(userId);
            return View(academician);
        }

        public ActionResult AddUser()
        {
            return View();
        }

      

//if (viewModel.Role == "Academician" || viewModel.Role == "Admin")
//{
//    Academician academician = new Academician();
//    academician.UserId = userId;
//    academician.AcademicianFirstName = viewModel.FirstName;
//    academician.AcademicianLastName = viewModel.LastName;
//    academician.AcademicianEmail = viewModel.Email;
//    academician.RoomNo = "101";
//    academician.ProgramId = 1;
//    academicianBusiness.Add(academician);
//    Advisor advisor = new Advisor();
//    advisor.UserId = userId;
//    advisor.StudentQuota = 30;
//    advisorBusiness.Add(advisor);
//}

        public ActionResult Campus()
                {
                    return View(campusBusiness.GetAll());
                }

        [HttpPost, ActionName("AddCampus")]
        [ValidateAntiForgeryToken]
        public ActionResult AddCampus([Bind(Include = "CampusName")]
        Campus campus)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(campus.CampusName))
                {
                    ModelState.AddModelError("inputError", "The required fields provided is missing");
                    return RedirectToAction("Campus");
                }
                else
                {
                    campusBusiness.Add(campus);
                    return RedirectToAction("Campus");
                }
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            return RedirectToAction("Campus");
        }

        [HttpPost, ActionName("RemoveCampus")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCampus([Bind(Include = "CampusId,Password")]
        Campus campus, string password)
        {
            Debug.WriteLine("model state: " + ModelState.IsValid); // It returns false, the model is not valid but it works

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }   

            string username = User.Identity.Name;
            bool isValid = Membership.ValidateUser(username, password);
            Debug.WriteLine("passwordcrrect: " + isValid);
            if (isValid)
            {
                campusBusiness.Delete(campus);
            }
            else
            {
                ModelState.AddModelError("Password", "Password is wrong.");
            }
            return RedirectToAction("Campus");
        }

        [HttpPost, ActionName("EditCampus")]
        [ValidateAntiForgeryToken]
        public ActionResult EditCampus([Bind(Include = "CampusId,CampusName")]
        Campus campus)
        {
            Debug.WriteLine("campusID: " + campus.CampusId);
            Debug.WriteLine("campusName: " + campus.CampusName);
            Debug.WriteLine("Campus" + campus);
            if (string.IsNullOrEmpty(campus.CampusName))
            {
                ModelState.AddModelError("CampusName", "Campus Name is required.");
            }
            else
            {
                campusBusiness.Update(campus);
            }


            return RedirectToAction("Campus");
        }

        public ActionResult Faculty()
        {
            return View(facultyBusiness.GetFacultyViewModel());
        }

        [HttpPost, ActionName("AddFaculty")]
        [ValidateAntiForgeryToken]
        public ActionResult AddFaculty([Bind(Include = "FacultyName,CampusId")]
        Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(faculty.FacultyName))
                {
                    ModelState.AddModelError("inputError", "The required fields provided is missing");
                    return RedirectToAction("Faculty");
                }
                else
                {
                    facultyBusiness.Add(faculty);
                    return RedirectToAction("Faculty");
                }
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            return RedirectToAction("Faculty");
        }

        [HttpPost, ActionName("RemoveFaculty")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFaculty([Bind(Include = "FacultyId,Password")]
        Faculty faculty, string password)
        {
            Debug.WriteLine("model state: " + ModelState.IsValid); // It returns false, the model is not valid but it works

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }

            string username = User.Identity.Name;
            bool isValid = Membership.ValidateUser(username, password);
            Debug.WriteLine("passwordcrrect: " + isValid);
            if (isValid)
            {
                facultyBusiness.Delete(faculty);
            }
            else
            {
                ModelState.AddModelError("Password", "Password is wrong.");
            }
            return RedirectToAction("Faculty");
        }

        [HttpPost, ActionName("EditFaculty")]
        [ValidateAntiForgeryToken]
        public ActionResult EditFaculty([Bind(Include = "FacultyId,FacultyName,CampusId")]
        Faculty faculty)
        {
            if (string.IsNullOrEmpty(faculty.FacultyName))
            {
                ModelState.AddModelError("FacultyName", "Faculty Name is required.");
            }
            else
            {
                facultyBusiness.Update(faculty);
            }

            return RedirectToAction("Faculty");
        }

        public ActionResult Department()
        {
            return View(departmentBusiness.GetDepartmentViewModel());
        }

        [HttpPost, ActionName("AddDepartment")]
        [ValidateAntiForgeryToken]
        public ActionResult AddDepartment([Bind(Include = "DepartmentName,FacultyId")]
        Department department)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(department.DepartmentName))
                {
                    ModelState.AddModelError("inputError", "The required fields provided is missing");
                    return RedirectToAction("Department");
                }
                else
                {
                    departmentBusiness.Add(department);
                    return RedirectToAction("Department");
                }
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            return RedirectToAction("Department");
        }

        [HttpPost, ActionName("RemoveDepartment")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDepartment([Bind(Include = "DepartmentId,Password")]
        Department department, string password)
        {
            Debug.WriteLine("model state: " + ModelState.IsValid); // It returns false, the model is not valid but it works

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }

            string username = User.Identity.Name;
            bool isValid = Membership.ValidateUser(username, password);
            Debug.WriteLine("passwordcrrect: " + isValid);
            if (isValid)
            {
                departmentBusiness.Delete(department);
            }
            else
            {
                ModelState.AddModelError("Password", "Password is wrong.");
            }
            return RedirectToAction("Department");
        }

        [HttpPost, ActionName("EditDepartment")]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartment([Bind(Include = "DepartmentId,DepartmentName,FacultyId")]
        Department department)
        {
            Debug.WriteLine("did: "+ department.DepartmentId);
            Debug.WriteLine("dname: " + department.DepartmentName);
            Debug.WriteLine("fid: " + department.FacultyId);
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                ModelState.AddModelError("DepartmentName", "Department Name is required.");
            }
            else
            {
                departmentBusiness.Update(department);
            }

            return RedirectToAction("Department");
        }

        public ActionResult Program()
        {
            return View(programBusiness.GetProgramViewModel());
        }

        [HttpPost, ActionName("AddProgram")]
        [ValidateAntiForgeryToken]
        public ActionResult AddProgram([Bind(Include = "ProgramName,DepartmentId,HeadId")]
        Program program)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(program.ProgramName))
                {
                    ModelState.AddModelError("inputError", "The required fields provided is missing");
                    return RedirectToAction("Program");
                }
                else
                {
                    programBusiness.Add(program);
                    return RedirectToAction("Program");
                }
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            return RedirectToAction("Program");
        }

        [HttpPost, ActionName("RemoveProgram")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveProgram([Bind(Include = "ProgramId,Password")]
        Program program, string password)
        {
            Debug.WriteLine("model state: " + ModelState.IsValid); // It returns false, the model is not valid but it works

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }

            string username = User.Identity.Name;
            bool isValid = Membership.ValidateUser(username, password);
            Debug.WriteLine("passwordcrrect: " + isValid);
            if (isValid)
            {
                programBusiness.Delete(program);
            }
            else
            {
                ModelState.AddModelError("Password", "Password is wrong.");
            }
            return RedirectToAction("Program");
        }

        [HttpPost, ActionName("EditDepartment")]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartment([Bind(Include = "ProgramId,ProgramName,DepartmentId,HeadId")]
        Program program)
        {

            if (string.IsNullOrEmpty(program.ProgramName))
            {
                ModelState.AddModelError("ProgramName", "Department Name is required.");
            }
            else
            {
                programBusiness.Update(program);
            }

            return RedirectToAction("Program");
        }

        public ActionResult Student()
        {
            var model = studentBusiness.AddUserViewModel();
            return View(model);
        }

        [HttpPost, ActionName("AddStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent([Bind(Include = "FirstName, LastName, Email, Password, Role")] AddUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                Membership.CreateUser(viewModel.Email, viewModel.Password, viewModel.Email, "question", "answer", true, out status);
                Roles.AddUserToRole(viewModel.Email, "Student");
                MembershipUser addedUser = Membership.GetUser(viewModel.Email);

                if (addedUser != null)
                {
                    Guid userId = (Guid)addedUser.ProviderUserKey;
                    Student student = new Student();
                    string studentNumber = "";
                    student.UserId = userId;
                    student.StudentFirstName = viewModel.FirstName;
                    student.StudentLastName = viewModel.LastName;
                    student.StudentEmail = viewModel.Email;
                    int indexOfAtSymbol = viewModel.Email.IndexOf("@");
                    if (indexOfAtSymbol != -1)
                    {
                        studentNumber = viewModel.Email.Substring(0, indexOfAtSymbol);
                    }
                    student.StudentNumber = studentNumber;
                    student.ProgramId = 1;
                    student.StandingId = 1;
                    studentBusiness.Add(student);
                }

            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing!");
            }
            return RedirectToAction("Student");

        }

        [HttpPost, ActionName("EditStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent([Bind(Include = "UserId, StudentFirstName, StudentLastName, StudentNumber, StudentEmail, StandingId, ProgramId")]
        Student student)
        {
            if (string.IsNullOrEmpty(student.UserId.ToString()))
            {
                ModelState.AddModelError("Student", "Student is required.");
            }
            else
            {
                studentBusiness.Update(student);
            }

            return RedirectToAction("Student");
        }

        [HttpPost, ActionName("RemoveStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveStudent([Bind(Include = "StudentId, Password")]
        Guid studentId, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }

            string username = User.Identity.Name;
            bool isValid = Membership.ValidateUser(username, password);
            if (isValid)
            {
                studentBusiness.Delete(studentId);
            }
            else
            {
                ModelState.AddModelError("Password", "Password is wrong.");
            }
            return RedirectToAction("Student");
        }

        public ActionResult Academician()
        {
            var model = academicianBusiness.AddAcademiciansViewModel();
            return View(model);
        }

        [HttpPost, ActionName("AddAcademician")]
        [ValidateAntiForgeryToken]
        public ActionResult AddAcademician([Bind(Include = "FirstName, LastName, Email, Password, Office")] AddAcademicianViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                Membership.CreateUser(viewModel.Email, viewModel.Password, viewModel.Email, "question", "answer", true, out status);
                Roles.AddUserToRole(viewModel.Email, "Academician");
                MembershipUser addedUser = Membership.GetUser(viewModel.Email);

                if (addedUser != null)
                {
                    Guid userId = (Guid)addedUser.ProviderUserKey;
                    Academician academician = new Academician();
                    academician.UserId = userId;
                    academician.AcademicianFirstName = viewModel.FirstName;
                    academician.AcademicianLastName = viewModel.LastName;
                    academician.AcademicianEmail = viewModel.Email;
                    academician.RoomNo = viewModel.Office;
                    academician.ProgramId = 1;
                    academicianBusiness.Add(academician);
                }

            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing!");
            }
            return RedirectToAction("Academician");

        }

        [HttpPost, ActionName("EditAcademician")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAcademician([Bind(Include = "UserId, AcademicianFirstName, AcademicianLastName, AcademicianEmail, RoomNo, ProgramId")]
        Academician academician)
        {
            if (string.IsNullOrEmpty(academician.UserId.ToString()))
            {
                ModelState.AddModelError("Academician", "Academician is required.");
            }
            else
            {
                academicianBusiness.Update(academician);
            }

            return RedirectToAction("Academician");
        }

        [HttpPost, ActionName("RemoveAcademician")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAcademician([Bind(Include = "AcademicianId, Password")]
        Guid academicianId, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }
            MembershipUser currentUser = Membership.GetUser();
            string username = User.Identity.Name;
            bool isValid = Membership.ValidateUser(username, password);
            if (isValid)
            {
                academicianBusiness.Delete(academicianId);
            }
            else
            {
                ModelState.AddModelError("Password", "Password is wrong.");
            }
            return RedirectToAction("Academician");
        }
    }
}