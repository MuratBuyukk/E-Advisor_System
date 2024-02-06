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
    public class FormProjectSubjectChangeProposalBusiness : IDatabaseBusiness<FormProjectSubjectChangeProposal>
    {
        StudentBusiness studentBusiness = new StudentBusiness();
        GraduationProjectBusiness graduationProjectBusiness = new GraduationProjectBusiness();
        public void Add(FormProjectSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectSubjectChangeProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormProjectSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormProjectSubjectChangeProposals.Find(id);
                db.FormProjectSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormProjectSubjectChangeProposal Get(Expression<Func<FormProjectSubjectChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectSubjectChangeProposals.Find(expression);
            }
        }

        public List<FormProjectSubjectChangeProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectSubjectChangeProposals
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.GraduationProject.Program.Academician)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormProjectSubjectChangeProposal> GetAll(Expression<Func<FormProjectSubjectChangeProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectSubjectChangeProposals
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.FormStatus)
                    .Where(expression).ToList();
            }
        }

        public FormProjectSubjectChangeProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectSubjectChangeProposals.Find(id);
            }
        }
        public FormProjectSubjectChangeProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormProjectSubjectChangeProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectSubjectChangeProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ProjectSubjectChangeProposalViewModel SubjectChangeFormViewModel(Guid studentId)
        {
            var project = graduationProjectBusiness.ProjectViewModel(studentId);
            var relatedForm = GetAll(f => f.ProjectId == project.ProjectId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormProjectSubjectChangeProposal();
                relatedForm.FormStatusId = 0;
            }
            var form = new ProjectSubjectChangeProposalViewModel
            {
                GraduationProject = project,
                Form = relatedForm,
            };
            return form;
        }

        public void sendSubjectChangeProposalForm(ProjectSubjectChangeProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormProjectSubjectChangeProposal
                {
                    FormDate = DateTime.Now,
                    ProjectId = viewModel.ProjectId,
                    NewTopic = viewModel.Topic,
                    OldTopic = viewModel.OldTopic,
                    FormStatusId = 1
                };
                db.FormProjectSubjectChangeProposals.Add(form);
                db.SaveChanges();
            }
        }
        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormProjectSubjectChangeProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}