using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;


namespace InformationTechnologiesAppointmentStatusIS.Repository.Concrete
{
    public class AppointmentStatusBusiness : IDatabaseBusiness<AppointmentStatus>
    {
        public void Add(AppointmentStatus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.AppointmentStatuses.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(AppointmentStatus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.AppointmentStatuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.AppointmentStatuses.Find(id);
                db.AppointmentStatuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public AppointmentStatus Get(Expression<Func<AppointmentStatus, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.AppointmentStatuses.Find(expression);
            }
        }

        public List<AppointmentStatus> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.AppointmentStatuses.ToList();
            }
        }

        public List<AppointmentStatus> GetAll(Expression<Func<AppointmentStatus, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.AppointmentStatuses.Where(expression).ToList();
            }
        }

        public AppointmentStatus GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.AppointmentStatuses.Find(id);
            }
        }
        public AppointmentStatus GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(AppointmentStatus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.AppointmentStatuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}