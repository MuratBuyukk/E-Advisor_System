using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;


namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterProjects
{
    public class FormPlagiarismReportBusiness : IDatabaseBusiness<FormPlagiarismReport>
    {
        GraduationProjectBusiness graduationProjectBusiness = new GraduationProjectBusiness();
        public void Add(FormPlagiarismReport entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormPlagiarismReports.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormPlagiarismReport entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormPlagiarismReports.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormPlagiarismReports.Find(id);
                db.FormPlagiarismReports.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormPlagiarismReport Get(Expression<Func<FormPlagiarismReport, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormPlagiarismReports.Find(expression);
            }
        }

        public List<FormPlagiarismReport> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormPlagiarismReports
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormPlagiarismReport> GetAll(Expression<Func<FormPlagiarismReport, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormPlagiarismReports
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.FormStatus)
                    .Where(expression).ToList();
            }
        }

        public FormPlagiarismReport GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormPlagiarismReports.Find(id);
            }
        }
        public FormPlagiarismReport GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormPlagiarismReport entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormPlagiarismReports.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ProjectPlagiarismReportViewModel ProjectPlagiarismReportViewModel(Guid studentId)
        {
            var project = graduationProjectBusiness.ProjectViewModel(studentId);
            var relatedForm = GetAll(f => f.ProjectId == project.ProjectId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormPlagiarismReport();
                relatedForm.FormStatusId = 0;
            }
            var form = new ProjectPlagiarismReportViewModel
            {
                GraduationProject = project,
                Form = relatedForm
            };
            return form;
        }

        public void sendProjectPlagiarismReportForm(ProjectPlagiarismReportViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormPlagiarismReport
                {
                    FormDate = DateTime.Now,
                    ProjectId = viewModel.ProjectId,
                    IsPlagiarised = false,
                    FormStatusId = 1
                };
                db.FormPlagiarismReports.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormPlagiarismReports.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}