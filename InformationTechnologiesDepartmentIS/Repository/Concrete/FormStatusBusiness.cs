using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;


namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class FormStatusBusiness : IDatabaseBusiness<FormStatus>
    {
        public void Add(FormStatus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormStatuses.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(FormStatus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormStatuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.FormStatuses.Find(id);
                db.FormStatuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public FormStatus Get(Expression<Func<FormStatus, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormStatuses.Find(expression);
            }
        }

        public List<FormStatus> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormStatuses.ToList();
            }
        }

        public List<FormStatus> GetAll(Expression<Func<FormStatus, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormStatuses.Where(expression).ToList();
            }
        }

        public FormStatus GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.FormStatuses.Find(id);
            }
        }
        public FormStatus GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(FormStatus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.FormStatuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}