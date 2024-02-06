using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;
using Microsoft.Ajax.Utilities;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterProjects
{
    public class FormProjectConsultantProposalBusiness : IDatabaseBusiness<FormProjectConsultantProposal>
    {
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        StudentBusiness studentBusiness = new StudentBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();
        public void Add(FormProjectConsultantProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectConsultantProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormProjectConsultantProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectConsultantProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormProjectConsultantProposals.Find(id);
                db.FormProjectConsultantProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormProjectConsultantProposal Get(Expression<Func<FormProjectConsultantProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantProposals.Find(expression);
            }
        }

        public List<FormProjectConsultantProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantProposals
                    .Include(p => p.Student)
                    .Include(p => p.Academician)
                    .Include(p => p.FormStatus)
                    .Include(p => p.Program)
                    .Include(p => p.Program.Academician)
                    .ToList();
            }
        }

        public List<FormProjectConsultantProposal> GetAll(Expression<Func<FormProjectConsultantProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantProposals
                    .Include(p => p.Student)
                    .Include(p => p.Academician)
                    .Include(p => p.FormStatus)
                    .Include(p => p.Program)
                    .Include(p => p.Program.Academician)
                    .Where(expression)
                    .ToList();
            }
        }

        public FormProjectConsultantProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantProposals.Find(id);
            }
        }
        public FormProjectConsultantProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormProjectConsultantProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectConsultantProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
        public ProjectConsultantProposalViewModel AdvisorFormViewModel(Guid studentId)
        {
            var student = studentBusiness.GetByGuid(studentId);
            var projectConsultantProposal = GetAll(p => p.StudentId == student.UserId).LastOrDefault();
            int programId = (int)student.ProgramId;
            var program = programBusiness.GetById(programId);
            var programHead = (academicianBusiness.GetByGuid((Guid)program.HeadId));
            var academicians = academicianBusiness.GetAll().Where(a => a.ProgramId == programId).ToList();
            Academician advisor = null;

            if (projectConsultantProposal != null)
            {
                advisor = academicianBusiness.GetByGuid(projectConsultantProposal.AcademicianId);
            }
            else
            {
                projectConsultantProposal = new FormProjectConsultantProposal();
                projectConsultantProposal.FormStatusId = 0;
            }
            var advisorFormViewModel = new ProjectConsultantProposalViewModel
            {
                Academicians = academicians,
                Student = student,
                ProgramName = program.ProgramName,
                ProgramHead = programHead.AcademicianFirstName + " " + programHead.AcademicianLastName,
                ProgramId = programId,
                Form = projectConsultantProposal,
                Advisor = advisor != null ? advisor.AcademicianFirstName + " " + advisor.AcademicianLastName : ""
            };
            return advisorFormViewModel;
        }

        public void sendConsultantProposalForm(ProjectConsultantProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var formProjectConsultant = new FormProjectConsultantProposal
                {
                    AcademicianId = viewModel.AcademicianId,
                    FormDate = DateTime.Now,
                    StudentId = viewModel.StudentId,
                    Topic = viewModel.Topic,
                    ProgramId = viewModel.ProgramId,
                    FormStatusId = 1     
                };
                db.FormProjectConsultantProposals.Add(formProjectConsultant);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormProjectConsultantProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}