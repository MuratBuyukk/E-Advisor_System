using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses
{
    public class FormThesisDeliveryCertificateBusiness: IDatabaseBusiness<FormThesisDeliveryCertificate>
    {
        MasterThesBusiness masterThesBusiness = new MasterThesBusiness();

        public void Add(FormThesisDeliveryCertificate entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisDeliveryCertificates.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormThesisDeliveryCertificate entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisDeliveryCertificates.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormThesisDeliveryCertificates.Find(id);
                db.FormThesisDeliveryCertificates.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormThesisDeliveryCertificate Get(Expression<Func<FormThesisDeliveryCertificate, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisDeliveryCertificates.Find(expression);
            }
        }

        public List<FormThesisDeliveryCertificate> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisDeliveryCertificates
                    .Include(p => p.MasterThes.Student)
                    .Include(p => p.MasterThes.Academician)
                    .Include(p => p.MasterThes.Program)
                    .Include(p => p.MasterThes.Program.Academician)
                    .Include(p => p.FormStatus)
                    .ToList();
            }
        }

        public List<FormThesisDeliveryCertificate> GetAll(Expression<Func<FormThesisDeliveryCertificate, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisDeliveryCertificates.Where(expression).ToList();
            }
        }

        public FormThesisDeliveryCertificate GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormThesisDeliveryCertificates.Find(id);
            }
        }
        public FormThesisDeliveryCertificate GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(FormThesisDeliveryCertificate entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormThesisDeliveryCertificates.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisDeliveryDateViewModel ThesisDeliveryCertificateFormViewModel(Guid studentId)
        {
            var thesis = masterThesBusiness.ThesisViewModel(studentId);
            var relatedForm = GetAll(f => f.ThesisId == thesis.ThesisId).LastOrDefault();
            if (relatedForm == null)
            {
                relatedForm = new FormThesisDeliveryCertificate();
                relatedForm.FormStatusId = 0;
            }
            var form = new ThesisDeliveryDateViewModel
            {
                MasterThesis = thesis,
                Form = relatedForm
            };
            return form;
        }

        public void sendThesisDeliveryCertificateForm(ThesisDeliveryDateViewModel viewModel)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = new FormThesisDeliveryCertificate
                {
                    FormDate = DateTime.Now,
                    DeliveryDate = viewModel.DeliveryDate,
                    IsEnglish = viewModel.IsEnglish,
                    ThesisId = viewModel.ThesisId,
                    FormStatusId = 1
                };
                db.FormThesisDeliveryCertificates.Add(form);
                db.SaveChanges();
            }
        }
        public void UpdateFormStatus(int formId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var form = db.FormThesisDeliveryCertificates.SingleOrDefault(f => f.FormId == formId);
                if (form != null)
                {
                    form.FormStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }
    }
}