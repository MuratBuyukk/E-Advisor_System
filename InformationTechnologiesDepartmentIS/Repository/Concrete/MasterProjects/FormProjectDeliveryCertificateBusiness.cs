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
    public class FormProjectDeliveryCertificateBusiness : IDatabaseBusiness<FormProjectDeliveryCertificate>
    {
        GraduationProjectBusiness graduationProjectBusiness = new GraduationProjectBusiness();

        public void Add(FormProjectDeliveryCertificate entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectDeliveryCertificates.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormProjectDeliveryCertificate entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectDeliveryCertificates.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormProjectDeliveryCertificates.Find(id);
                db.FormProjectDeliveryCertificates.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormProjectDeliveryCertificate Get(Expression<Func<FormProjectDeliveryCertificate, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectDeliveryCertificates.Find(expression);
            }
        }

        public List<FormProjectDeliveryCertificate> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectDeliveryCertificates
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormProjectDeliveryCertificate> GetAll(Expression<Func<FormProjectDeliveryCertificate, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectDeliveryCertificates
                    .Include(p => p.GraduationProject)
                    .Include(p => p.GraduationProject.Student)
                    .Include(p => p.GraduationProject.Academician)
                    .Include(p => p.GraduationProject.Program)
                    .Include(p => p.FormStatus)
                    .Where(expression)
                    .ToList();
            }
        }

        public FormProjectDeliveryCertificate GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormProjectDeliveryCertificates.Find(id);
            }
        }
        public FormProjectDeliveryCertificate GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(FormProjectDeliveryCertificate entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormProjectDeliveryCertificates.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ProjectDeliveryDateViewModel ProjectDeliveryCertificateFormViewModel(Guid studentId)
        {
            var project = graduationProjectBusiness.ProjectViewModel(studentId);
            var relatedForm = GetAll(f => f.ProjectId == project.ProjectId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormProjectDeliveryCertificate();
                relatedForm.FormStatusId = 0;
            }
            var form = new ProjectDeliveryDateViewModel
            {
                GraduationProject = project,
                Form = relatedForm
            };
            return form;
        }

        public void sendProjectDeliveryCertificateForm(ProjectDeliveryDateViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormProjectDeliveryCertificate
                {
                    FormDate = DateTime.Now,
                    DeliveryDate = viewModel.DeliveryDate,
                    IsEnglish = viewModel.IsEnglish,
                    ProjectId = viewModel.ProjectId,
                    FormStatusId = 1
                };
                db.FormProjectDeliveryCertificates.Add(form);
                db.SaveChanges();
            }
        }

        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormProjectDeliveryCertificates.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }

    }
}