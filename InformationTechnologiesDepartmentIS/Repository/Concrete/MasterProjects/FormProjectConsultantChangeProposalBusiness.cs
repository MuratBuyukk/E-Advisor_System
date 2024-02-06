using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;


namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterProjects
{
    public class FormProjectConsultantChangeProposalBusiness : IDatabaseBusiness<FormProjectConsultantChangeProposal>
    {
        StudentBusiness studentBusiness = new StudentBusiness();
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();
        GraduationProjectBusiness graduationProjectBusiness = new GraduationProjectBusiness();
        FormProjectConsultantProposalBusiness formProjectConsultantProposal = new FormProjectConsultantProposalBusiness();
        public void Add(FormProjectConsultantChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectConsultantChangeProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormProjectConsultantChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectConsultantChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormProjectConsultantChangeProposals.Find(id);
                db.FormProjectConsultantChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormProjectConsultantChangeProposal Get(Expression<Func<FormProjectConsultantChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantChangeProposals.Find(expression);
            }
        }

        public List<FormProjectConsultantChangeProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantChangeProposals
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.GraduationProject.Program.Academician)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.Academician)
                    .Include(p => p.Academician1)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormProjectConsultantChangeProposal> GetAll(Expression<Func<FormProjectConsultantChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantChangeProposals
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.Academician)
                    .Include(p => p.Academician1)
                    .Include(p => p.FormStatus)
                    .Where(expression).ToList();
             
            }
        }

        public FormProjectConsultantChangeProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectConsultantChangeProposals.Find(id);
            }
        }
        public void Update(FormProjectConsultantChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectConsultantChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ProjectConsultantChangeProposalViewModel AdvisorChangeFormViewModel(Guid studentId)
        {
            var student = studentBusiness.GetByGuid(studentId);
            int programId = (int)student.ProgramId;
            var program = programBusiness.GetById(programId);
            var academicians = academicianBusiness.GetAll().Where(a => a.ProgramId == programId).ToList();
            var project = graduationProjectBusiness.ProjectViewModel(studentId);
            var consultantChangeProposal = GetAll().Where(f => f.ProjectId == project.ProjectId).LastOrDefault();
            Academician academician = null;
            if (consultantChangeProposal != null)
            {
                academician = academicianBusiness.GetByGuid(consultantChangeProposal.NewAcademicianId);
            }
            else
            {
                consultantChangeProposal = new FormProjectConsultantChangeProposal();
                consultantChangeProposal.FormStatusId = 0;
            }
            var form = new ProjectConsultantChangeProposalViewModel
            {
                Academicians = academicians,
                GraduationProject = project,
                Form = consultantChangeProposal,
                Advisor = academician != null ? academician.AcademicianFirstName + " " + academician.AcademicianLastName : "",
            };
            return form;
        }

        public void sendConsultantChangeProposalForm(ProjectConsultantChangeProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormProjectConsultantChangeProposal
                {
                    FormDate = DateTime.Now,
                    ProjectId = viewModel.ProjectId,
                    NewTopic = viewModel.Topic,
                    NewAcademicianId = viewModel.AcademicianId,
                    OldTopic = viewModel.OldTopic,
                    OldAdvisorId = viewModel.OldAcademicianId,
                    FormStatusId = 1
                };
                db.FormProjectConsultantChangeProposals.Add(form);
                db.SaveChanges();
            }
        }

        public FormProjectConsultantChangeProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormProjectConsultantChangeProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}
