using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses
{
    public class FormThesisConcultantAndSubjectChangeProposalBusiness : IDatabaseBusiness<FormThesisConsultantAndSubjectChangeProposal>
    {
        /*---NOT Consultant Change Proposal'View Model Kullanıyor Tüm Parametreler Aynı işlevide Aynı----*/
        StudentBusiness studentBusiness = new StudentBusiness();
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();
        MasterThesBusiness masterThesisBusiness = new MasterThesBusiness();
        public void Add(FormThesisConsultantAndSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConsultantAndSubjectChangeProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisConsultantAndSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConsultantAndSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisConsultantAndSubjectChangeProposals.Find(id);
                db.FormThesisConsultantAndSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisConsultantAndSubjectChangeProposal Get(Expression<Func<FormThesisConsultantAndSubjectChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantAndSubjectChangeProposals.Find(expression);
            }
        }

        public List<FormThesisConsultantAndSubjectChangeProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantAndSubjectChangeProposals
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.MasterThes.Academician)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.Academician)
                    .Include(p => p.Academician1)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisConsultantAndSubjectChangeProposal> GetAll(Expression<Func<FormThesisConsultantAndSubjectChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantAndSubjectChangeProposals.Where(expression).ToList();
            }
        }

        public FormThesisConsultantAndSubjectChangeProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantAndSubjectChangeProposals.Find(id);
            }
        }
        public FormThesisConsultantAndSubjectChangeProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisConsultantAndSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConsultantAndSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisConsultantAndSubjectChangeViewModel AdvisorSubjectChangeFormViewModel(Guid studentId)
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
                consultantChangeProposal = new FormThesisConsultantAndSubjectChangeProposal();
                consultantChangeProposal.FormStatusId = 0;
            }
            var form = new ThesisConsultantAndSubjectChangeViewModel
            {
                Academicians = academicians,
                MasterThesis = thesis,
                Form = consultantChangeProposal,
                Advisor = advisor != null ? advisor.AcademicianFirstName + " " + advisor.AcademicianLastName : "",
            };
            return form;
        }

        public void sendConsultantSubjectChangeProposalForm(ThesisConsultantAndSubjectChangeViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisConsultantAndSubjectChangeProposal
                {
                    FormDate = DateTime.Now,
                    ThesisId = viewModel.ThesisId,
                    NewTopic = viewModel.Topic,
                    NewAdvisorId = viewModel.AcademicianId,
                    OldAdvisorId = viewModel.OldAdvisorId,
                    OldTopic = viewModel.OldTopic,
                    FormStatusId = 1
                };
                db.FormThesisConsultantAndSubjectChangeProposals.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisConsultantAndSubjectChangeProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}