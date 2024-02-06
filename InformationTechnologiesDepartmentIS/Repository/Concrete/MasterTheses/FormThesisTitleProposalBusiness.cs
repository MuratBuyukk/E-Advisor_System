using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses
{
    public class FormThesisTitleProposalBusiness : IDatabaseBusiness<FormThesisTitleProposal>
    {
        MasterThesBusiness masterThesBusiness = new MasterThesBusiness();
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        public void Add(FormThesisTitleProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisTitleProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisTitleProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisTitleProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisTitleProposals.Find(id);
                db.FormThesisTitleProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisTitleProposal Get(Expression<Func<FormThesisTitleProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisTitleProposals.Find(expression);
            }
        }

        public List<FormThesisTitleProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisTitleProposals
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.MasterThes.Academician)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisTitleProposal> GetAll(Expression<Func<FormThesisTitleProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisTitleProposals.Where(expression).ToList();
            }
        }

        public FormThesisTitleProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisTitleProposals.Find(id);
            }
        }
        public FormThesisTitleProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisTitleProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisTitleProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisTitleProposalViewModel ThesisTitleProposalFormViewModel(Guid studentId)
        {
            var thesis = masterThesBusiness.ThesisViewModel(studentId);
            var relatedForm = GetAll(f => f.ThesisId == thesis.ThesisId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormThesisTitleProposal();
                relatedForm.FormStatusId = 0;
            }
            var form = new ThesisTitleProposalViewModel
            {
                MasterThesis = thesis,
                Form = relatedForm,
            };
            return form;
        }

        public void sendThesisTitleProposalForm(ThesisTitleProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisTitleProposal
                {
                    FormDate = DateTime.Now,
                    ThesisId = viewModel.ThesisId,
                    Title = viewModel.Title,
                    FormStatusId = 1
                };
                db.FormThesisTitleProposals.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisTitleProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}