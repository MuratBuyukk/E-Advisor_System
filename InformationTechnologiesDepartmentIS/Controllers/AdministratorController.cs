using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
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

        public ActionResult University()
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
                    return RedirectToAction("University");
                }
                else
                {
                    campusBusiness.Add(campus);
                    return RedirectToAction("University");
                }
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            return View(campusBusiness.GetAll());
        }

        [HttpPost, ActionName("AddUser")]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "FirstName,LastName,Email,Password,Role")] AddUserViewModel viewModel)
        {
            MembershipCreateStatus status;
            Membership.CreateUser(viewModel.Email, viewModel.Password, viewModel.Email, "question", "answer", true, out status);
            Roles.AddUserToRole(viewModel.Email, viewModel.Role);

            MembershipUser addedUser = Membership.GetUser(viewModel.Email);
          
            if (addedUser != null)
            {
                Debug.WriteLine(addedUser);
                Guid userId = (Guid)addedUser.ProviderUserKey;
                if (viewModel.Role == "Academician" || viewModel.Role == "Admin")
                {
                    Academician academician = new Academician();
                    academician.UserId = userId;
                    academician.AcademicianFirstName = viewModel.FirstName;
                    academician.AcademicianLastName = viewModel.LastName;
                    academician.AcademicianEmail = viewModel.Email;
                    academician.RoomNo = "101";
                    academician.ProgramId = 1;
                    academicianBusiness.Add(academician);
                    Advisor advisor = new Advisor();
                    advisor.UserId = userId;
                    advisor.StudentQuota = 30;
                    advisorBusiness.Add(advisor);
                }
                else if (viewModel.Role == "Student")
                {
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
                    studentBusiness.Add(student);
                }
            }
            else
            {
                Debug.WriteLine("added User is empty");
            }

            return RedirectToAction("AddUser");
        }
    }
}