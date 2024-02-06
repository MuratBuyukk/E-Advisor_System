using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses
{
    public class FormThesisSubjectChangeProposalBusiness : IDatabaseBusiness<FormThesisSubjectChangeProposal>
    {
        StudentBusiness studentBusiness = new StudentBusiness();
        MasterThesBusiness masterThesBusiness = new MasterThesBusiness();
        public void Add(FormThesisSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisSubjectChangeProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisSubjectChangeProposals.Find(id);
                db.FormThesisSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisSubjectChangeProposal Get(Expression<Func<FormThesisSubjectChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisSubjectChangeProposals.Find(expression);
            }
        }

        public List<FormThesisSubjectChangeProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisSubjectChangeProposals
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.MasterThes.Academician)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisSubjectChangeProposal> GetAll(Expression<Func<FormThesisSubjectChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisSubjectChangeProposals.Where(expression).ToList();
            }
        }

        public FormThesisSubjectChangeProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisSubjectChangeProposals.Find(id);
            }
        }
        public FormThesisSubjectChangeProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisSubjectChangeProposalViewModel SubjectChangeFormViewModel(Guid studentId)
        {
            var thesis = masterThesBusiness.ThesisViewModel(studentId);
            var relatedForm = GetAll(f => f.ThesisId == thesis.ThesisId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormThesisSubjectChangeProposal();
                relatedForm.FormStatusId = 0;
            }
            var form = new ThesisSubjectChangeProposalViewModel
            {
                MasterThesis = thesis,
                Form = relatedForm,
            };
            return form;
        }

        public void sendSubjectChangeProposalForm(ThesisSubjectChangeProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisSubjectChangeProposal
                {
                    FormDate = DateTime.Now,
                    ThesisId = viewModel.ThesisId,
                    NewTopic = viewModel.Topic,
                    OldTopic = viewModel.OldTopic,
                    FormStatusId = 1
                };
                db.FormThesisSubjectChangeProposals.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisSubjectChangeProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}