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
    public class FormThesisExtensionProposalBusiness : IDatabaseBusiness<FormThesisExtensionProposal>
    {
        MasterThesBusiness masterThesBusiness = new MasterThesBusiness();
        public void Add(FormThesisExtensionProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisExtensionProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisExtensionProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisExtensionProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisExtensionProposals.Find(id);
                db.FormThesisExtensionProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisExtensionProposal Get(Expression<Func<FormThesisExtensionProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisExtensionProposals.Find(expression);
            }
        }

        public List<FormThesisExtensionProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisExtensionProposals
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.MasterThes.Academician)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisExtensionProposal> GetAll(Expression<Func<FormThesisExtensionProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisExtensionProposals.Where(expression).ToList();
            }
        }

        public FormThesisExtensionProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisExtensionProposals.Find(id);
            }
        }
        public FormThesisExtensionProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisExtensionProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisExtensionProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisExtensionProposalViewModel ThesisExtensionProposalFormViewModel(Guid studentId)
        {
            var thesis = masterThesBusiness.ThesisViewModel(studentId);
            var relatedForm = GetAll(f => f.ThesisId == thesis.ThesisId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormThesisExtensionProposal();
                relatedForm.FormStatusId = 0;
            }
            var form = new ThesisExtensionProposalViewModel
            {
                MasterThesis = thesis,
                Form = relatedForm,
            };
            return form;
        }

        public void sendThesisExtensionProposalForm(ThesisExtensionProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisExtensionProposal
                {
                    FormDate = DateTime.Now,
                    ThesisId = viewModel.ThesisId,
                    FormStatusId = 1
                };
                db.FormThesisExtensionProposals.Add(form);
                db.SaveChanges();
            }
        }
        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisExtensionProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}