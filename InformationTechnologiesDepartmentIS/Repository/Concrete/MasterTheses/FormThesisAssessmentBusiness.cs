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
    public class FormThesisAssessmentProposalBusiness : IDatabaseBusiness<FormThesisAssessmentProposal>
    {
        MasterThesBusiness masterThesBusiness = new MasterThesBusiness();
        public void Add(FormThesisAssessmentProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisAssessmentProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisAssessmentProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisAssessmentProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisAssessmentProposals.Find(id);
                db.FormThesisAssessmentProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisAssessmentProposal Get(Expression<Func<FormThesisAssessmentProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisAssessmentProposals.Find(expression);
            }
        }

        public List<FormThesisAssessmentProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisAssessmentProposals
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.MasterThes.Academician)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisAssessmentProposal> GetAll(Expression<Func<FormThesisAssessmentProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisAssessmentProposals.Where(expression).ToList();
            }
        }

        public FormThesisAssessmentProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisAssessmentProposals.Find(id);
            }
        }
        public FormThesisAssessmentProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisAssessmentProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisAssessmentProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisAssessmentProposalViewModel ThesisAssessmentProposalFormViewModel(Guid studentId)
        {
            var thesis = masterThesBusiness.ThesisViewModel(studentId);
            var ralatedForm = GetAll(f => f.ThesisId == thesis.ThesisId).LastOrDefault();
            if (ralatedForm == null)
            {
                ralatedForm = new FormThesisAssessmentProposal();
                ralatedForm.FormStatusId = 0;
            }
            var form = new ThesisAssessmentProposalViewModel
            {
                MasterThesis = thesis,
                Form = ralatedForm
            };
            return form;
        }

        public void sendThesisAssessmentProposalForm(ThesisAssessmentProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisAssessmentProposal
                {
                    FormDate = DateTime.Now,
                    ThesisId = viewModel.ThesisId,
                    FormStatusId = 1
                };
                db.FormThesisAssessmentProposals.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisAssessmentProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}