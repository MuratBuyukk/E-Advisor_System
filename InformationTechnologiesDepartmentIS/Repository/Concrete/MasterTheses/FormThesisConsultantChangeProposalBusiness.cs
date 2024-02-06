using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses
{
    public class FormThesisConcultantChangeProposalBusiness : IDatabaseBusiness<FormThesisConcultantChangeProposal>
    {
        StudentBusiness studentBusiness = new StudentBusiness();
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();
        MasterThesBusiness masterThesisBusiness = new MasterThesBusiness();
        public void Add(FormThesisConcultantChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConcultantChangeProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisConcultantChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConcultantChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisConcultantChangeProposals.Find(id);
                db.FormThesisConcultantChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisConcultantChangeProposal Get(Expression<Func<FormThesisConcultantChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConcultantChangeProposals.Find(expression);
            }
        }

        public List<FormThesisConcultantChangeProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConcultantChangeProposals
                    .Include(p => p.MasterThes)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.Academician)
                    .Include(p => p.Academician1)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisConcultantChangeProposal> GetAll(Expression<Func<FormThesisConcultantChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConcultantChangeProposals.Where(expression).ToList();
            }
        }

        public FormThesisConcultantChangeProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConcultantChangeProposals.Find(id);
            }
        }
        public FormThesisConcultantChangeProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisConcultantChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConcultantChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisConsultantChangeProposalViewModel AdvisorChangeFormViewModel(Guid studentId)
        {
            var student = studentBusiness.GetByGuid(studentId);
            int programId = (int)student.ProgramId;
            var program = programBusiness.GetById(programId);
            var academicians = academicianBusiness.GetAll().Where(a => a.ProgramId == programId).ToList();
            var thesis = masterThesisBusiness.ThesisViewModel(studentId);
            var consultantChangeProposal = GetAll(f => f.ThesisId == thesis.ThesisId).LastOrDefault();
            Academician advisor = null;
            if (consultantChangeProposal != null)
            {
                advisor = academicianBusiness.GetByGuid(consultantChangeProposal.NewAdvisorId);
            }
            else
            {
                consultantChangeProposal = new FormThesisConcultantChangeProposal();
                consultantChangeProposal.FormStatusId = 0;
            }
            var form = new ThesisConsultantChangeProposalViewModel
            {
                Academicians = academicians,
                MasterThesis = thesis,
                FormThesisConsultantChangeProposal = consultantChangeProposal,
                Advisor = advisor != null ? advisor.AcademicianFirstName + " " + advisor.AcademicianLastName : "",
            };
            return form;
        }

        public void sendConsultantChangeProposalForm(ThesisConsultantChangeProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisConcultantChangeProposal
                {
                    FormDate = DateTime.Now,
                    ThesisId = viewModel.ThesisId,
                    NewTopic = viewModel.Topic,
                    NewAdvisorId = viewModel.AcademicianId,
                    OldAdvisorId = viewModel.OldAdvisorId,
                    OldTopic = viewModel.OldTopic,
                    FormStatusId = 1
                };
                db.FormThesisConcultantChangeProposals.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisConcultantChangeProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}