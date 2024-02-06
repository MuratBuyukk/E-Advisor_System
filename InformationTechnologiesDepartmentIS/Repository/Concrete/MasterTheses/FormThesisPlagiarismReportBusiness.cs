using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses
{
    public class FormThesisPlagiarismReportBusiness : IDatabaseBusiness<FormThesisPlagiarismReport>
    {
        MasterThesBusiness masterThesBusiness = new MasterThesBusiness();
        public void Add(FormThesisPlagiarismReport entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisPlagiarismReports.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisPlagiarismReport entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisPlagiarismReports.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisPlagiarismReports.Find(id);
                db.FormThesisPlagiarismReports.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisPlagiarismReport Get(Expression<Func<FormThesisPlagiarismReport, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisPlagiarismReports.Find(expression);
            }
        }

        public List<FormThesisPlagiarismReport> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisPlagiarismReports
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.MasterThes.Academician)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisPlagiarismReport> GetAll(Expression<Func<FormThesisPlagiarismReport, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisPlagiarismReports.Where(expression).ToList();
            }
        }

        public FormThesisPlagiarismReport GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisPlagiarismReports.Find(id);
            }
        }
        public FormThesisPlagiarismReport GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisPlagiarismReport entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisPlagiarismReports.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisPlagiarismReportViewModel ThesisPlagiarismReportViewModel(Guid studentId)
        {
            var thesis = masterThesBusiness.ThesisViewModel(studentId);
            var relatedForm = GetAll(f => f.ThesisId == thesis.ThesisId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormThesisPlagiarismReport();
                relatedForm.FormStatusId = 0;
            }
            var form = new ThesisPlagiarismReportViewModel
            {
                MasterThesis = thesis,
                Form = relatedForm
            };
            return form;
        }

        public void sendThesisPlagiarismReportForm(ThesisPlagiarismReportViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisPlagiarismReport
                {
                    FormDate = DateTime.Now,
                    ThesisId = viewModel.ThesisId,
                    IsPlagiarised = false,
                    FormStatusId = 1
                };
                db.FormThesisPlagiarismReports.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisPlagiarismReports.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}