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
    public class FormThesisConsultantProposalBusiness : IDatabaseBusiness<FormThesisConsultantProposal>
    {
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        StudentBusiness studentBusiness = new StudentBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();
        public void Add(FormThesisConsultantProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConsultantProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisConsultantProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConsultantProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisConsultantProposals.Find(id);
                db.FormThesisConsultantProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisConsultantProposal Get(Expression<Func<FormThesisConsultantProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantProposals.Find(expression);
            }
        }

        public List<FormThesisConsultantProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantProposals
                    .Include(p => p.Student)
                    .Include(p => p.Academician)
                    .Include(p => p.Academician1)
                    .Include(p => p.FormStatus)
                    .Include(p => p.Program)
                    .Include(p => p.Program.Academician)
                    .ToList();
            }
        }

        public List<FormThesisConsultantProposal> GetAll(Expression<Func<FormThesisConsultantProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantProposals.Where(expression).ToList();
            }
        }

        public FormThesisConsultantProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisConsultantProposals.Find(id);
            }
        }
        public FormThesisConsultantProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormThesisConsultantProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisConsultantProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
        public ThesisConsultantProposalViewModel AdvisorFormViewModel(Guid studentId)
        {
            var student = studentBusiness.GetByGuid(studentId);
            var thesisConsultantProposal = GetAll(p => p.StudentId == student.UserId).LastOrDefault();
            int programId = (int)student.ProgramId;
            var program = programBusiness.GetById(programId);
            var academician = academicianBusiness.GetByGuid((Guid)program.HeadId);
            var academicians = academicianBusiness.GetAll().Where(a => a.ProgramId == programId).ToList();
            Academician advisor = null;
            Academician coAdvisor = null;
            if (thesisConsultantProposal != null)
            {
                advisor = academicianBusiness.GetByGuid(thesisConsultantProposal.AdvisorId);
                coAdvisor = academicianBusiness.GetByGuid((Guid)thesisConsultantProposal.CoAdvisorId);
            }
            else
            {
                thesisConsultantProposal = new FormThesisConsultantProposal();
                thesisConsultantProposal.FormStatusId = 0;
            }
            var advisorFormViewModel = new ThesisConsultantProposalViewModel
            {
                Academicians = academicians,
                Student = student,
                ProgramName = program.ProgramName,
                ProgramHead = academician.AcademicianFirstName + " " + academician.AcademicianLastName,
                ProgramId = programId,
                FormThesisConsultantProposal = thesisConsultantProposal,
                Advisor = advisor != null ? advisor.AcademicianFirstName + " " + advisor.AcademicianLastName : "",
                CoAdvisor = coAdvisor != null ? coAdvisor.AcademicianFirstName + " " + coAdvisor.AcademicianLastName : "",
            };
            return advisorFormViewModel;
        }

        public void sendConsultantProposalForm(ThesisConsultantProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var formProjectConsultant = new FormThesisConsultantProposal
                {
                    AdvisorId = viewModel.AdvisorId,
                    CoAdvisorId = viewModel.CoAdvisorId,
                    FormDate = DateTime.Now,
                    StudentId = viewModel.StudentId,
                    Topic = viewModel.Topic,
                    ProgramId = viewModel.ProgramId,
                    FormStatusId = 1
                };
                db.FormThesisConsultantProposals.Add(formProjectConsultant);
                db.SaveChanges();
            }
        }
        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisConsultantProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}