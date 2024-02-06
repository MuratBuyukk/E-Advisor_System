using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;


namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterProjects
{
    public class FormProjectTitleProposalBusiness : IDatabaseBusiness<FormProjectTitleProposal>
    {
        GraduationProjectBusiness graduationProjectBusiness = new GraduationProjectBusiness();
        public void Add(FormProjectTitleProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectTitleProposals.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormProjectTitleProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectTitleProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormProjectTitleProposals.Find(id);
                db.FormProjectTitleProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormProjectTitleProposal Get(Expression<Func<FormProjectTitleProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectTitleProposals.Find(expression);
            }
        }

        public List<FormProjectTitleProposal> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectTitleProposals
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormProjectTitleProposal> GetAll(Expression<Func<FormProjectTitleProposal, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectTitleProposals
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.FormStatus)
                    .Where(expression).ToList();
            }
        }

        public FormProjectTitleProposal GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectTitleProposals.Find(id);
            }
        }
        public FormProjectTitleProposal GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormProjectTitleProposal entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectTitleProposals.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ProjectTitleProposalViewModel ProjectTitleProposalFormViewModel(Guid studentId)
        {
            var project = graduationProjectBusiness.ProjectViewModel(studentId);
            var relatedForm = GetAll(f => f.ProjectId == project.ProjectId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormProjectTitleProposal();
                relatedForm.FormStatusId = 0;
            }
            var form = new ProjectTitleProposalViewModel
            {
                GraduationProject = project,
                Form = relatedForm,
            };
            return form;
        }

        public void sendProjectTitleProposalForm(ProjectTitleProposalViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormProjectTitleProposal
                {
                    FormDate = DateTime.Now,
                    ProjectId = viewModel.ProjectId,
                    Title = viewModel.Title,
                    FormStatusId = 1
                };
                db.FormProjectTitleProposals.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormProjectTitleProposals.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}