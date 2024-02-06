using InformationTechnologiesDepartmentIS.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InformationTechnologiesDepartmentIS.Models;
using System.Web.Security;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesAppointmentIS.Repository.Concrete;
using InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses;
using InformationTechnologiesDepartmentIS.Repository.Concrete.MasterProjects;
using System.Diagnostics;
using System.Globalization;
using System.Web.Services.Description;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;

namespace InformationTechnologiesDepartmentIS.Controllers
{

    [Authorize(Roles = "Student, Admin")]
    public class StudentController : Controller
    {
        ITDepartmentDbEntities db = new ITDepartmentDbEntities();
        StudentBusiness studentBusiness = new StudentBusiness();
        AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
        ProjectBusiness projectBusiness = new ProjectBusiness();

        FormProjectConsultantProposalBusiness consultantProposalBusiness = new FormProjectConsultantProposalBusiness();
        FormProjectDeliveryCertificateBusiness projectDeliveryCertificateBusiness = new FormProjectDeliveryCertificateBusiness();
        FormProjectConsultantChangeProposalBusiness consultantChangeProposalBusiness = new FormProjectConsultantChangeProposalBusiness();
        FormProjectSubjectChangeProposalBusiness subjectChangeProposalbusiness = new FormProjectSubjectChangeProposalBusiness();
        FormProjectSubjectChangeProposalBusiness subjectChangeProposalBusiness = new FormProjectSubjectChangeProposalBusiness();
        FormProjectDeliveryCertificateBusiness deliveryCertificateBusiness = new FormProjectDeliveryCertificateBusiness();
        FormProjectTitleProposalBusiness titleProposalBusiness = new FormProjectTitleProposalBusiness();
        FormPlagiarismReportBusiness plagiarismReportBusiness = new FormPlagiarismReportBusiness();
        GraduationProjectBusiness graduationProjectBusiness = new GraduationProjectBusiness();
        
        FormThesisConsultantProposalBusiness thesisConsultantProposalBusiness = new FormThesisConsultantProposalBusiness();
        FormThesisConcultantChangeProposalBusiness thesisConcultantChangeBusiness = new FormThesisConcultantChangeProposalBusiness();
        FormThesisTitleProposalBusiness thesisTitleProposalBusiness = new FormThesisTitleProposalBusiness();
        FormThesisSubjectChangeProposalBusiness ThesisSubjectChangeProposalBusiness = new FormThesisSubjectChangeProposalBusiness();
        FormThesisConcultantAndSubjectChangeProposalBusiness thesisConcultantAndSubjectChangeProposalBusiness = new FormThesisConcultantAndSubjectChangeProposalBusiness();
        FormThesisExtensionProposalBusiness thesisExtensionProposalBusiness = new FormThesisExtensionProposalBusiness();
        FormThesisAssessmentProposalBusiness thesisAssessmentProposalBusiness = new FormThesisAssessmentProposalBusiness();
        FormThesisPlagiarismReportBusiness thesisPlagiarismReportBusiness = new FormThesisPlagiarismReportBusiness();
        FormThesisDeliveryCertificateBusiness thesisDeliveryCertificateBusiness = new FormThesisDeliveryCertificateBusiness();
        MasterThesBusiness masterThesBusiness = new MasterThesBusiness();

        public const string MAIL_SUBJECT = "Graduation Project Request";
        public const string MAIL_BODY = "A student requested consultancy from you for graduation project. To see: https://localhost:44316/Admin/PendingProjects";
        // GET: Student
        public ActionResult Index()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);
            Guid userId = (Guid)currentUser.ProviderUserKey;
            Student student = studentBusiness.GetByGuid(userId);
            return View(student);
        }

        public ActionResult GraduationThesis()
        {
            int? statusId = studentBusiness.GetStudentProjectStatusById(getStudentId());
            int value = 0;
            var model = projectBusiness.GetProjectAdvisorViewModel(getStudentId());
            if (statusId == 0)
            {
                value = 0;
            }
            else if (statusId == 1)
            {
                value = 1;
            }
            else if (statusId == 2)
            {
                value = 2;
            }
            else if (statusId == 3)
            {
                value = 3;
            }

            model.Response = value;
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GraduationThesis([Bind(Include = "ProjectTitle,ProjectMainDomain ," +
            "ProjectSubDomains, Problem, Solution, ThesisStatement, SubmissionDate, StudentId, AdvisorUserId, ProjectStatusId")] ProjectAdvisorViewModel project)
        {
            if (ModelState.IsValid)
            {
                project.StudentUserId = getStudentId();
                projectBusiness.sendProject(project);
                string toEmail = projectBusiness.getAcademicianEmailByProjectId(project.ProjectId);
                //projectBusiness.SendEmail(toEmail, MAIL_SUBJECT, MAIL_BODY);
                return RedirectToAction("GraduationThesis");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing!");
            }


            ProjectAdvisorViewModel model = projectBusiness.GetProjectAdvisorViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("DeleteExistingProject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteExistingProject()
        {
            Guid studentId = getStudentId();
            var project = db.Projects.FirstOrDefault(p => p.StudentUserId == studentId);
            if (project != null)
            {
                projectBusiness.Delete(project.ProjectId);
                return RedirectToAction("GraduationThesis");
            }

            return View();
        }

        public Guid getStudentId()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);
            var userId = (Guid)currentUser.ProviderUserKey;
            return userId;
        }

        public ActionResult Appointment()
        {
            var appointmentViewModel = appointmentBusiness.GetAppointmentViewModel();
            return View(appointmentViewModel);
        }

        [HttpPost, ActionName("RequestAppointment")]
        public ActionResult RequestAppointment(int appointmentId, string appointmentTopic)
        {
                Guid studentId = getStudentId();
                string message = appointmentBusiness.RequestAppointment(appointmentId, studentId, appointmentTopic);
                if (message == "exist")
                {
                    ViewBag.PopupMessage = "Appointment already exists!";
                    var appointmentViewModel = appointmentBusiness.GetAppointmentViewModel();
                    return View("Appointment", appointmentViewModel);
                }
                 return RedirectToAction("Appointments", new { message = message });
        }

        [HttpPost, ActionName("CancelAppointment")]
        public ActionResult CancelAppointment(int appointmentId)
        {
                appointmentBusiness.CancelAppointment(appointmentId);
                return RedirectToAction("Appointments");
        }


        [HttpPost, ActionName("Appointment")]
        public ActionResult Appointment([Bind(Include = "AcademicianId, AppointmentDate")] 
        AppointmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = appointmentBusiness.GetAppointmentViewModelByFilter(viewModel.AcademicianId, viewModel.AppointmentDate);
                if (model.Appointments == null || model.Appointments.ToArray().Length == 0)
                {
                    ViewBag.Data = "No appointments available.";
                }
                return View(model);
            }
            var model2 = appointmentBusiness.GetAppointmentViewModel();
            return View(model2);
        }

        public ActionResult Appointments(string message)
        {
            var model = appointmentBusiness.GetAppointmentViewModelForStudent();
            ViewBag.Message = message;
            return View(model);
        }

        public ActionResult ProjectAdvisorProposalForm()
        {
            var model = consultantProposalBusiness.AdvisorFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ProjectAdvisorProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAdvisorProposalForm([Bind(Include = "AcademicianId, FormDate, StudentId, Topic, ProgramId")]
        ProjectConsultantProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                consultantProposalBusiness.sendConsultantProposalForm(viewModel);
                return RedirectToAction("GraduationProjectForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
                    }
            var model = consultantProposalBusiness.AdvisorFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ProjectAdvisorChangeProposalForm()
        {
            var model = consultantChangeProposalBusiness.AdvisorChangeFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ProjectAdvisorChangeProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAdvisorChangeProposalForm([Bind(Include = "AcademicianId, Topic, ProjectId, OldAcademicianId, OldTopic")]
        ProjectConsultantChangeProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                consultantChangeProposalBusiness.sendConsultantChangeProposalForm(viewModel);
                return RedirectToAction("GraduationProjectForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }

            var model = consultantChangeProposalBusiness.AdvisorChangeFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ProjectSubjectChangeProposalForm()
        {
            var model = subjectChangeProposalBusiness.SubjectChangeFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ProjectSubjectChangeProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectSubjectChangeProposalForm([Bind(Include = "Topic, ProjectId, OldTopic")]
        ProjectSubjectChangeProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                subjectChangeProposalbusiness.sendSubjectChangeProposalForm(viewModel);
                return RedirectToAction("GraduationProjectForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
          }

            var model = subjectChangeProposalBusiness.SubjectChangeFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ProjectTitleProposalForm()
        {
            var model = titleProposalBusiness.ProjectTitleProposalFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ProjectTitleProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectTitleProposalForm([Bind(Include = "Title, ProjectId")]
        ProjectTitleProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                titleProposalBusiness.sendProjectTitleProposalForm(viewModel);
                return RedirectToAction("GraduationProjectForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = titleProposalBusiness.ProjectTitleProposalFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ProjectDeliveryCertificateForm()
        {
            var model = deliveryCertificateBusiness.ProjectDeliveryCertificateFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ProjectDeliveryCertificateForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectDeliveryCertificateForm([Bind(Include = "ProjectId, DeliveryDate, IsEnglish")]
        ProjectDeliveryDateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                projectDeliveryCertificateBusiness.sendProjectDeliveryCertificateForm(viewModel);
                return RedirectToAction("GraduationProjectForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = deliveryCertificateBusiness.ProjectDeliveryCertificateFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ProjectPlagiarismReportForm()
        {
            var model = plagiarismReportBusiness.ProjectPlagiarismReportViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ProjectPlagiarismReportForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectPlagiarismReportForm([Bind(Include = "ProjectId")]
        ProjectPlagiarismReportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                plagiarismReportBusiness.sendProjectPlagiarismReportForm(viewModel);
                return RedirectToAction("GraduationProjectForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = plagiarismReportBusiness.ProjectPlagiarismReportViewModel(getStudentId());
            return View(model);
        }

        public ActionResult GraduationProjectForms()
        {
            bool projectExist = false;
            List<GraduationProject> projects = graduationProjectBusiness.GetAll();
            foreach(var p in projects)
            {
                if (p.StudentId == getStudentId())
                {
                    projectExist = true;
                    break;
                }
            }
            ViewBag.projectExist = projectExist;
            var model = ProjectFormsViewModel(getStudentId());
            return View(model);
        }

        public ActionResult MasterThesisForms()
        {
            bool thesisExist = false;
            List<MasterThes> theses = masterThesBusiness.GetAll();
            foreach (var t in theses)
            {
                if (t.StudentId == getStudentId())
                {
                    thesisExist = true;
                    break;
                }
            }
            ViewBag.thesisExist = thesisExist;
            var model = ThesisFormsViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisAdvisorProposalForm()
        {
            var model = thesisConsultantProposalBusiness.AdvisorFormViewModel(getStudentId());
            return View(model);
        }
        [HttpPost, ActionName("ThesisAdvisorProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisAdvisorProposalForm([Bind(Include = "AdvisorId, CoAdvisorId, StudentId, Topic, ProgramId")] ThesisConsultantProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Eğer AdvisorId ve CoAdvisorId değerleri eşitse hata mesajı ekle
                if (viewModel.AdvisorId == viewModel.CoAdvisorId)
                {
                    ModelState.AddModelError("inputError", "Advisor and Co-advisor cannot be the same.");
                    var errorModel = thesisConsultantProposalBusiness.AdvisorFormViewModel(getStudentId());
                    errorModel.errorMessage = "Advisor and Co-advisor cannot be the same.";
                    return View(errorModel);
                }

                thesisConsultantProposalBusiness.sendConsultantProposalForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided are missing.");
            }

            var model = thesisConsultantProposalBusiness.AdvisorFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisAdvisorChangeProposalForm()
        {
            var model = thesisConcultantChangeBusiness.AdvisorChangeFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisAdvisorChangeProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisAdvisorChangeProposalForm([Bind(Include = "AcademicianId, Topic, ThesisId, OldAdvisorId, OldTopic")]
        ThesisConsultantChangeProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                thesisConcultantChangeBusiness.sendConsultantChangeProposalForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }

            var model = thesisConcultantChangeBusiness.AdvisorChangeFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisTitleProposalForm()
        {
            var model = thesisTitleProposalBusiness.ThesisTitleProposalFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisTitleProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisTitleProposalForm([Bind(Include = "Title, ThesisId")]
        ThesisTitleProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                thesisTitleProposalBusiness.sendThesisTitleProposalForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = thesisTitleProposalBusiness.ThesisTitleProposalFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisSubjectChangeProposalForm()
        {
            var model = ThesisSubjectChangeProposalBusiness.SubjectChangeFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisSubjectChangeProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisSubjectChangeProposalForm([Bind(Include = "Topic, ThesisId, OldTopic")]
        ThesisSubjectChangeProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ThesisSubjectChangeProposalBusiness.sendSubjectChangeProposalForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }

            var model = ThesisSubjectChangeProposalBusiness.SubjectChangeFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisAdvisorAndSubjectChangeProposalForm()
        {
            var model = thesisConcultantAndSubjectChangeProposalBusiness.AdvisorSubjectChangeFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisAdvisorAndSubjectChangeProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisAdvisorAndSubjectChangeProposalForm([Bind(Include = "AcademicianId, Topic, ThesisId, OldAdvisorId, OldTopic")]
        ThesisConsultantAndSubjectChangeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                thesisConcultantAndSubjectChangeProposalBusiness.sendConsultantSubjectChangeProposalForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }

            var model = thesisConcultantAndSubjectChangeProposalBusiness.AdvisorSubjectChangeFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisExtensionProposalForm()
        {
            var model = thesisExtensionProposalBusiness.ThesisExtensionProposalFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisExtensionProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisExtensionProposalForm([Bind(Include = "ThesisId")]
        ThesisExtensionProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                thesisExtensionProposalBusiness.sendThesisExtensionProposalForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = thesisExtensionProposalBusiness.ThesisExtensionProposalFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisAssessmentProposalForm()
        {
            var model = thesisAssessmentProposalBusiness.ThesisAssessmentProposalFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisAssessmentProposalForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisAssessmentProposalForm([Bind(Include = "ThesisId")]
        ThesisAssessmentProposalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                thesisAssessmentProposalBusiness.sendThesisAssessmentProposalForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = thesisAssessmentProposalBusiness.ThesisAssessmentProposalFormViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisPlagiarismReportForm()
        {
            var model = thesisPlagiarismReportBusiness.ThesisPlagiarismReportViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisPlagiarismReportForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisPlagiarismReportForm([Bind(Include = "ThesisId")]
        ThesisPlagiarismReportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                thesisPlagiarismReportBusiness.sendThesisPlagiarismReportForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = thesisPlagiarismReportBusiness.ThesisPlagiarismReportViewModel(getStudentId());
            return View(model);
        }

        public ActionResult ThesisDeliveryCertificateForm()
        {
            var model = thesisDeliveryCertificateBusiness.ThesisDeliveryCertificateFormViewModel(getStudentId());
            return View(model);
        }

        [HttpPost, ActionName("ThesisDeliveryCertificateForm")]
        [ValidateAntiForgeryToken]
        public ActionResult ThesisDeliveryCertificateForm([Bind(Include = "ThesisId, DeliveryDate, IsEnglish")]
        ThesisDeliveryDateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                thesisDeliveryCertificateBusiness.sendThesisDeliveryCertificateForm(viewModel);
                return RedirectToAction("MasterThesisForms");
            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing");
            }
            var model = thesisDeliveryCertificateBusiness.ThesisDeliveryCertificateFormViewModel(getStudentId());
            return View(model);
        }

        public ProjectFormsViewModel ProjectFormsViewModel(Guid studentId)
        {
            var project = graduationProjectBusiness.GetAll(p => p.StudentId == studentId).FirstOrDefault();
            if (project == null)
            {
                var viewModel = new ProjectFormsViewModel();
                viewModel.FormProjectConsultantProposal = consultantProposalBusiness.GetAll(f => f.StudentId == studentId).LastOrDefault();
                return viewModel;
            }
            else
            {
                int projectId = project.ProjectId;
                var graduationProject = graduationProjectBusiness.ProjectViewModel(studentId);
                string advisorFullName = graduationProject.Academician.AcademicianFirstName + " " + graduationProject.Academician.AcademicianLastName;
                var formProjectConsultantProposal = consultantProposalBusiness.GetAll(f => f.StudentId == studentId).LastOrDefault();
                var formProjectConsultantChangeProposal = consultantChangeProposalBusiness.GetAll(f => f.ProjectId == projectId).LastOrDefault();
                var formProjectSubjectChange = subjectChangeProposalBusiness.GetAll(f => f.ProjectId == projectId).LastOrDefault();
                var formProjectTitle = titleProposalBusiness.GetAll(f => f.ProjectId == projectId).LastOrDefault();
                var formDelivery = projectDeliveryCertificateBusiness.GetAll(f => f.ProjectId == projectId).LastOrDefault();
                var formPlagiarism = plagiarismReportBusiness.GetAll(f => f.ProjectId == projectId).LastOrDefault();
                var viewModel = new ProjectFormsViewModel
                {
                    FormProjectConsultantProposal = formProjectConsultantProposal,
                    FormProjectConsultantChangeProposal = formProjectConsultantChangeProposal,
                    FormProjectSubjectChangeProposal = formProjectSubjectChange,
                    FormProjectTitleProposal = formProjectTitle,
                    FormProjectDeliveryCertificate = formDelivery,
                    FormPlagiarismReport = formPlagiarism,
                    AdvisorFullName = advisorFullName,
                };
                return viewModel;
            }
        }

        public ThesisFormsViewModel ThesisFormsViewModel(Guid studentId)
        {
            var thesis = masterThesBusiness.GetAll(t => t.StudentId == studentId).FirstOrDefault();
            if (thesis == null)
            {
                var viewModel = new ThesisFormsViewModel();
                viewModel.FormThesisConsultantProposal = thesisConsultantProposalBusiness.GetAll(f => f.StudentId == studentId).LastOrDefault();
                return viewModel;
            }
            else
            {
                int thesisId = thesis != null ? thesis.ThesisId : 0;
                var masterThesis = masterThesBusiness.ThesisViewModel(studentId);
                string advisorFullName = masterThesis.Advisor.AcademicianFirstName + " " + masterThesis.Advisor.AcademicianLastName;
                var formThesisConsultantProposal = thesisConsultantProposalBusiness.GetAll(f => f.StudentId == studentId).LastOrDefault();
                var formThesisConsultantChangeProposal = thesisConcultantChangeBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var formThesisSubjectChange = ThesisSubjectChangeProposalBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var formThesisTitle = thesisTitleProposalBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var formThesisConsultantAndSubjectChange = thesisConcultantAndSubjectChangeProposalBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var formThesisExtensionProposal = thesisExtensionProposalBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var formThesisAssessmentProposal = thesisAssessmentProposalBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var formThesisDelivery = thesisDeliveryCertificateBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var formThesisPlagiarism = thesisPlagiarismReportBusiness.GetAll(f => f.ThesisId == thesisId).LastOrDefault();
                var viewModel = new ThesisFormsViewModel
                {
                    AdvisorFullName = advisorFullName,
                    FormThesisConsultantProposal = formThesisConsultantProposal,
                    FormThesisConsultantChangeProposal = formThesisConsultantChangeProposal,
                    FormThesisTitleProposal = formThesisTitle,
                    FormThesisSubjectChangeProposal = formThesisSubjectChange,
                    FormThesisConsultantAndSubjectChange = formThesisConsultantAndSubjectChange,
                    FormThesisExtensionProposal = formThesisExtensionProposal,
                    FormThesisAssessmentProposal = formThesisAssessmentProposal,
                    FormThesisDeliveryCertificate = formThesisDelivery,
                    FormThesisPlagiarismReport = formThesisPlagiarism
                };
                return viewModel;
            }
        }
    }
}
