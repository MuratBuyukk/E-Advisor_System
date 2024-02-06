using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InformationTechnologiesDepartmentIS.Models;
using System.Data.Entity;
using InformationTechnologiesAppointmentIS.Repository.Concrete;
using System.Globalization;
using InformationTechnologiesDepartmentIS.Repository.Concrete.MasterProjects;
using InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses;

namespace InformationTechnologiesDepartmentIS.Controllers
{
    [Authorize(Roles = "Academician, Admin")]
    public class AdminController : Controller
    {
        ProjectBusiness projectBusiness = new ProjectBusiness();
        AdvisorBusiness projectAdvisorBusiness = new AdvisorBusiness();
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
        ITDepartmentDbEntities db = new ITDepartmentDbEntities();
        FormProjectConsultantProposalBusiness formProjectConsultantProposal = new FormProjectConsultantProposalBusiness();
        FormProjectConsultantChangeProposalBusiness formProjectConsultantChangeProposal = new FormProjectConsultantChangeProposalBusiness();
        FormProjectSubjectChangeProposalBusiness formProjectSubjectChangeProposal = new FormProjectSubjectChangeProposalBusiness();
        FormProjectTitleProposalBusiness formProjectTitleProposalBusiness = new FormProjectTitleProposalBusiness();
        FormProjectDeliveryCertificateBusiness formProjectDeliveryCertificateBusiness = new FormProjectDeliveryCertificateBusiness();
        FormPlagiarismReportBusiness formPlagiarismReportBusiness = new FormPlagiarismReportBusiness();
        FormThesisConsultantProposalBusiness formThesisConsultantProposal = new FormThesisConsultantProposalBusiness();
        FormThesisConcultantChangeProposalBusiness formThesisConcultantChangeProposal = new FormThesisConcultantChangeProposalBusiness();
        FormThesisTitleProposalBusiness formThesisTitleProposal = new FormThesisTitleProposalBusiness();
        FormThesisSubjectChangeProposalBusiness FormThesisSubjectChangeProposal = new FormThesisSubjectChangeProposalBusiness();
        FormThesisConcultantAndSubjectChangeProposalBusiness formThesisConcultantAndSubjectChangeProposal = new FormThesisConcultantAndSubjectChangeProposalBusiness();
        FormThesisExtensionProposalBusiness formThesisExtensionProposal = new FormThesisExtensionProposalBusiness();
        FormThesisAssessmentProposalBusiness formThesisAssessmentProposal = new FormThesisAssessmentProposalBusiness();
        FormThesisPlagiarismReportBusiness formThesisPlagiarismReport = new FormThesisPlagiarismReportBusiness();
        FormThesisDeliveryCertificateBusiness formThesisDeliveryCertificate = new FormThesisDeliveryCertificateBusiness();
        public const string MAIL_SUBJECT = "Graduation Project | Feedback";
        public const string MAIL_BODY = "Some feedback regarding the final project has been published. To check: https://localhost:44316/Student/GraduationThesis";

        // GET: Admin
        public ActionResult Index()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);
            Guid userId = (Guid)currentUser.ProviderUserKey;
            Academician academician = academicianBusiness.GetByGuid(userId);
            return View(academician);
        }

        public ActionResult GraduationThesis()
        {
            var allProjects = projectBusiness.GetAll().Where(p => p.Advisor.UserId == getAcademicianId()).ToList();
            var pendingProjects = projectBusiness.GetAll(p => p.ProjectStatu.ProjectStatusName == "Pending").Where(p => p.Advisor.UserId == getAcademicianId()).ToList();
            var acceptedProjects = projectBusiness.GetAll(p => p.ProjectStatu.ProjectStatusName == "Accepted").Where(p => p.Advisor.UserId == getAcademicianId()).ToList();
            var rejectedProjects = projectBusiness.GetAll(p => p.ProjectStatu.ProjectStatusName == "Rejected").Where(p => p.Advisor.UserId == getAcademicianId()).ToList();


            var projects = new ProjectCountsViewModel
            {
                PendingCount = pendingProjects.Count,
                AcceptedCount = acceptedProjects.Count,
                RejectedCount = rejectedProjects.Count,
                Projects = allProjects
            };

            return View(projects);
        }

        public ActionResult GraduationThesisDetail(int id)
        {
            var model = projectBusiness.GetById(id);
            int numberOfQuotas = projectAdvisorBusiness
                .GetQuotasByAdvisorId(getAcademicianId());
            ViewBag.Quota = numberOfQuotas;
            return View(model);
        }
        public ActionResult PendingProjects()
        {
            var model = projectBusiness.GetAll(p => p.ProjectStatu.ProjectStatusName == "Pending").Where(p => p.Advisor.UserId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult AcceptedProjects()
        {
            var model = projectBusiness.GetAll(p => p.ProjectStatu.ProjectStatusName == "Accepted").Where(p => p.Advisor.UserId == getAcademicianId()).ToList();
            return View(model);
        }
        public ActionResult RejectedProjects()
        {
            var model = projectBusiness.GetAll(p => p.ProjectStatu.ProjectStatusName == "Rejected").Where(p => p.Advisor.UserId == getAcademicianId()).ToList();
            return View(model);
        }

        [HttpPost, ActionName("AcceptProject")]
        public ActionResult AcceptProject(int projectId, string toEmail)
        {
            projectBusiness.UpdateProjectStatus(projectId, 2);
            Guid academicianId = projectBusiness.GetById(projectId).Advisor.Academician.UserId;
            projectAdvisorBusiness.UpdateStudentQuota(academicianId);
            toEmail = projectBusiness.getStudentEmailByProjectId(projectId);
            //projectBusiness.SendEmail(toEmail, MAIL_SUBJECT, MAIL_BODY);
            return RedirectToAction("GraduationThesis");
        }

        [HttpPost, ActionName("RejectProject")]
        public ActionResult RejectProject(int projectId, string toEmail, string feedbackMessage)
        {
            projectBusiness.UpdateProjectStatus(projectId, 3);
            projectBusiness.SendFeedback(projectId, feedbackMessage);
            toEmail = projectBusiness.getStudentEmailByProjectId(projectId);
            //projectBusiness.SendEmail(toEmail, MAIL_SUBJECT, MAIL_BODY);
            return RedirectToAction("GraduationThesis");
        }

        public ActionResult Appointment(bool isCurrent = true)
        {
            appointmentBusiness.ChangeAppointmentStatusToOutDated();
            var appointmentViewModel = appointmentBusiness.GetAppointmentViewModelForAdmin(isCurrent);
            ViewBag.IsCurrent = isCurrent;
            return View(appointmentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppointment([Bind(Include = "AcademicianId, StudentId, AppointmentDate," +
    "AppointmentTopic, IsRenewable, AppointmentStatusId")] Appointment appointment, string AppointmentDate)
        {
            if (ModelState.IsValid)
            {
                appointment.AcademicianId = getAcademicianId();
                appointment.AppointmentStatusId = 1;
                if (DateTime.TryParseExact(AppointmentDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    appointment.AppointmentDate = parsedDate;
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                }

            }
            else
            {
                ModelState.AddModelError("inputError", "The required fields provided is missing!");
            }
            return RedirectToAction("Appointment");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelAppointment(int appointmentId)
        {
            appointmentBusiness.UpdateAppointmentStatus(appointmentId, 3);
            return RedirectToAction("Appointment");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeActiveAppointment(int appointmentId)
        {
            var appointment = appointmentBusiness.GetById(appointmentId);
            if (appointment.Student!=null)
            {
                appointmentBusiness.UpdateAppointmentStatus(appointmentId, 2);
            }
            else
            {
                appointmentBusiness.UpdateAppointmentStatus(appointmentId, 1);
            }
            return RedirectToAction("Appointment");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeRenewable(int AppointmentId)
        {
            Guid userId = getAcademicianId();
            appointmentBusiness.ChangeRenewability(AppointmentId, true, userId);
            return RedirectToAction("Appointment");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeNotRenewable(int AppointmentId)
        {
            Guid userId = getAcademicianId();
            appointmentBusiness.ChangeRenewability(AppointmentId, false, userId);
            return RedirectToAction("Appointment");
        }

        [HttpPost, ActionName("ShowOutDatedAppointment")]
        [ValidateAntiForgeryToken]
        public ActionResult ShowOutDatedAppointment()
        {
            return RedirectToAction("Appointment", new { isCurrent = false });
        }

        [HttpPost, ActionName("ShowCurrentAppointment")]
        [ValidateAntiForgeryToken]
        public ActionResult ShowCurrentAppointment()
        {
            return RedirectToAction("Appointment", new { isCurrent = true });
        }

        public Guid getAcademicianId()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name);
            var userId = (Guid)currentUser.ProviderUserKey;
            return userId;
        }

        public ActionResult MasterProjectForms()
        { 
            return View();
        }

        public ActionResult ProjectAdvisorProposalForm()
        {
            var model = formProjectConsultantProposal.GetAll().Where(p => p.AcademicianId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ProjectAdvisorProposalFormDetail(int id)
        {
            var model = formProjectConsultantProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAdvisorProposalStatus(int formId, int statusId)
        {
            formProjectConsultantProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ProjectAdvisorProposalForm");
        }

        public ActionResult ProjectAdvisorChangeProposalForm()
        {
            var model = formProjectConsultantChangeProposal.GetAll().Where(p => 
            p.NewAcademicianId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ProjectAdvisorChangeProposalFormDetail(int id)
        {
            var model = formProjectConsultantChangeProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAdvisorChangeProposalStatus(int formId, int statusId)
        {
            formProjectConsultantChangeProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ProjectAdvisorChangeProposalForm");
        }
        public ActionResult ProjectTopicChangeProposalForm()
        {
            var model = formProjectSubjectChangeProposal.GetAll().Where(p => p.GraduationProject.AcademicianId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ProjectTopicChangeProposalFormDetail(int id)
        {
            var model = formProjectSubjectChangeProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTopicChangeProposalStatus(int formId, int statusId)
        {
            formProjectSubjectChangeProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ProjectTopicChangeProposalForm");
        }

        public ActionResult ProjectTitleProposalForm()
        {
            var model = formProjectTitleProposalBusiness.GetAll().Where(p => p.GraduationProject.AcademicianId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ProjectTitleProposalFormDetail(int id)
        {
            var model = formProjectTitleProposalBusiness.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProjectTitleProposalStatus(int formId, int statusId)
        {
            formProjectTitleProposalBusiness.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ProjectTitleProposalForm");
        }


        public ActionResult ProjectDeliveryCertificateForm()
        {
            var model = formProjectDeliveryCertificateBusiness.GetAll().Where(p => p.GraduationProject.AcademicianId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ProjectDeliveryCertificateFormDetail(int id)
        {
            var model = formProjectDeliveryCertificateBusiness.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProjectDeliveryCertificateStatus(int formId, int statusId)
        {
            formProjectDeliveryCertificateBusiness.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ProjectDeliveryCertificateForm");
        }

        public ActionResult ProjectPlagiarismReportForm()
        {
            var model = formPlagiarismReportBusiness.GetAll().Where(p => p.GraduationProject.AcademicianId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ProjectPlagiarismReportFormDetail(int id)
        {
            var model = formPlagiarismReportBusiness.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePlagiarismReportProposalStatus(int formId, int statusId)
        {
            formPlagiarismReportBusiness.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ProjectPlagiarismReportForm");
        }

        public ActionResult MasterThesisForms()
        {
            return View();
        }

        public ActionResult ThesisAdvisorProposalForm()
        {
            var model = formThesisConsultantProposal.GetAll().Where(p => p.AdvisorId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ThesisAdvisorProposalFormDetail(int id)
        {
            var model = formThesisConsultantProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisAdvisorProposalStatus(int formId, int statusId)
        {
            formThesisConsultantProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisAdvisorProposalForm");
        }

        public ActionResult ThesisAdvisorChangeProposalForm()
        {
            var model = formThesisConcultantChangeProposal.GetAll().Where(p => p.NewAdvisorId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ThesisAdvisorChangeProposalFormDetail(int id)
        {
            var model = formThesisConcultantChangeProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisAdvisorChangeProposalStatus(int formId, int statusId)
        {
            formThesisConcultantChangeProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisAdvisorChangeProposalForm");
        }

        public ActionResult ThesisTitleProposalForm()
        {
            var model = formThesisTitleProposal.GetAll().Where(p => p.MasterThes.AdvisorId == getAcademicianId()).ToList();
            return View(model);
        }
        public ActionResult ThesisTitleProposalFormDetail(int id)
        {
            var model = formThesisTitleProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisTitleProposalStatus(int formId, int statusId)
        {
            formThesisTitleProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisTitleProposalForm");
        }
        public ActionResult ThesisSubjectChangeProposalForm()
        {
            var model = FormThesisSubjectChangeProposal.GetAll().Where(p => p.MasterThes.AdvisorId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ThesisSubjectChangeProposalFormDetail(int id)
        {
            var model = FormThesisSubjectChangeProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisSubjectChangeProposalStatus(int formId, int statusId)
        {
            FormThesisSubjectChangeProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisSubjectChangeProposalForm");
        }

        public ActionResult ThesisAdvisorAndSubjectChangeProposalForm()
        {
            var model = formThesisConcultantAndSubjectChangeProposal.GetAll().Where(p => p.NewAdvisorId == getAcademicianId()).ToList();
            return View(model);
        }
        public ActionResult ThesisAdvisorAndSubjectChangeProposalFormDetail(int id)
        {
            var model = formThesisConcultantAndSubjectChangeProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisAdvisorAndSubjectChangeProposalStatus(int formId, int statusId)
        {
            formThesisConcultantAndSubjectChangeProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisAdvisorAndSubjectChangeProposalForm");
        }

        public ActionResult ThesisExtensionProposalForm()
        {
            var model = formThesisExtensionProposal.GetAll().Where(p => p.MasterThes.AdvisorId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ThesisExtensionProposalFormDetail(int id)
        {
            var model = formThesisExtensionProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisExtensionProposalStatus(int formId, int statusId)
        {
            formThesisExtensionProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisExtensionProposalForm");
        }
        public ActionResult ThesisAssessmentProposalForm()
        {
            var model = formThesisAssessmentProposal.GetAll().Where(p => p.MasterThes.AdvisorId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ThesisAssessmentProposalFormDetail(int id)
        {
            var model = formThesisAssessmentProposal.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisAssessmentProposalStatus(int formId, int statusId)
        {
            formThesisAssessmentProposal.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisAssessmentProposalForm");
        }

        public ActionResult ThesisPlagiarismReportForm()
        {
            var model = formThesisPlagiarismReport.GetAll().Where(p => p.MasterThes.AdvisorId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ThesisPlagiarismReportFormDetail(int id)
        {
            var model = formThesisPlagiarismReport.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisPlagiarismReportStatus(int formId, int statusId)
        {
            formThesisPlagiarismReport.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisPlagiarismReportForm");
        }
        public ActionResult ThesisDeliveryCertificateForm()
        {
            var model = formThesisDeliveryCertificate.GetAll().Where(p => p.MasterThes.AdvisorId == getAcademicianId()).ToList();
            return View(model);
        }

        public ActionResult ThesisDeliveryCertificateFormDetail(int id)
        {
            var model = formThesisDeliveryCertificate.GetAll().Where(p => p.FormId == id).LastOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateThesisDeliveryCertificateStatus(int formId, int statusId)
        {
            formThesisDeliveryCertificate.UpdateFormStatus(formId, statusId);
            return RedirectToAction("ThesisDeliveryCertificateForm");
        }

    }
}
