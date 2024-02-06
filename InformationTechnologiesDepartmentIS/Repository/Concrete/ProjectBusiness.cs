using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using System.Web.UI;
using System.Net.Mail;
using System.Net;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class ProjectBusiness : IDatabaseBusiness<Project>
    {
        AdvisorBusiness advisorBusiness = new AdvisorBusiness();
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        public void Add(Project entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Projects.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Project entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Projects.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Projects.Find(id);
                db.Projects.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Project Get(Expression<Func<Project, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Projects.Find(expression);
            }
        }

        public List<Project> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Projects
                    .Include(p => p.Student)
                    .Include(p => p.ProjectStatu)
                    .Include(p => p.Advisor)
                    .Include(p => p.Advisor.Academician)
                    .ToList();
            }
        }

        public List<Project> GetAll(Expression<Func<Project, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Projects
                    .Include(p => p.Student)
                    .Include(p => p.ProjectStatu)
                    .Include(p => p.Advisor)
                    .Include(p => p.Advisor.Academician).
                    Where(expression).ToList();
            }
        }

        public Project GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                Project projects = db.Projects
                   .Include(p => p.Student)
                   .Include(p => p.ProjectStatu)
                   .Include(p => p.Advisor)
                   .Include(p => p.Advisor.Academician)
                   .FirstOrDefault(p => p.ProjectId == id);

                return projects;
            }
        }

        public Project GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Project entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Projects.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void UpdateProjectStatus(int projectId, int statusId)
        {
            using (var context = new ITDepartmentDbEntities())
            {
                var project = context.Projects.SingleOrDefault(p => p.ProjectId == projectId);
                if (project != null)
                {
                    project.ProjectStatusId = statusId;
                    context.SaveChanges();
                }
            }
        }

        public void SendFeedback(int projectId, string feedbackMessage)
        {
            using (var context = new ITDepartmentDbEntities())
            {
                var project = context.Projects.SingleOrDefault(p => p.ProjectId == projectId);
                if (project != null)
                {
                    project.FeedbackMessage = feedbackMessage;
                    context.SaveChanges();
                }
            }
        }

        public ProjectAdvisorViewModel GetProjectAdvisorViewModel(Guid studentId)
        {
            var db = new ITDepartmentDbEntities();
            var projectAdvisors = db.Advisors
                .Include(pa => pa.Academician)
                .Where(pa => pa.StudentQuota > 0)
                .ToList();

            var project = db.Projects.Where(p => p.StudentUserId == studentId ).SingleOrDefault()
;

            var projectAdvisorViewModel = new ProjectAdvisorViewModel
            {
                ProjectAdvisors = projectAdvisors,
                Academicians = projectAdvisors.Select(pa => pa.Academician).ToList(),                
                FeedbackMessage = project?.FeedbackMessage,
            };

            return projectAdvisorViewModel;
        }

        public ProjectAdvisorViewModel sendProject(ProjectAdvisorViewModel projectAdvisorViewModel)
        {

            using (var db = new ITDepartmentDbEntities())
            {
                var project = new Project
                {
                    ProjectId = projectAdvisorViewModel.ProjectId,
                    ProjectTitle = projectAdvisorViewModel.ProjectTitle,
                    ProjectMainDomain = projectAdvisorViewModel.ProjectMainDomain,
                    ProjectSubDomains = projectAdvisorViewModel.ProjectSubDomains,
                    Problem = projectAdvisorViewModel.Problem,
                    ThesisStatement = projectAdvisorViewModel.ThesisStatement,
                    Solution = projectAdvisorViewModel.Solution,
                    StudentUserId   = projectAdvisorViewModel.StudentUserId,
                    AdvisorUserId = projectAdvisorViewModel.AdvisorUserId,
                    ProjectStatusId = projectAdvisorViewModel.ProjectStatusId,
                    SubmissionDate = projectAdvisorViewModel.SubmissionDate,
                };
                db.Projects.Add(project);
                db.SaveChanges();
            }
            return null;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            
            // Configure the SMTP client with Outlook SMTP server and credentials
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("19MISY1027@isik.edu.tr", "beytullah2001"),
                EnableSsl = true
            };

            // Create the email message
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("19MISY1027@isik.edu.tr"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            try
            {
                // Send the email
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., log the error or display a message to the user
                // You should implement proper error handling for production use
            }
            finally
            {
                // Dispose of the SMTP client and mail message
                smtpClient.Dispose();
                mailMessage.Dispose();
            }
        }

        public string getStudentEmailByProjectId(int projectId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var studentEmail = db.Projects
                    .Where(p => p.ProjectId == projectId)
                    .Include(p => p.Student)
                    .Select(p => p.Student.StudentEmail)
                    .FirstOrDefault();

                return studentEmail;
            }
        }

        public string getAcademicianEmailByProjectId(int projectId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var academicianEmail = db.Projects
                    .Where(p => p.ProjectId == projectId)
                    .Include(p => p.Advisor)
                    .Select(p => p.Advisor.Academician.AcademicianEmail)
                    .FirstOrDefault();

                return academicianEmail;
            }
        }

    }

}
