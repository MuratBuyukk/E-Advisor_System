using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class CampusBusiness : IDatabaseBusiness<Campus>
    {
        public void Add(Campus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Campuses.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Campus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Campuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Campuses.Find(id);
                db.Campuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Campus Get(Expression<Func<Campus, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Campuses.Find(expression);
            }
        }

        public List<Campus> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Campuses.ToList();
            }
        }

        public List<Campus> GetAll(Expression<Func<Campus, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Campuses.Where(expression).ToList();
            }
        }

        public Campus GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public Campus GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Campuses.Find(id);
            }
        }

        public void Update(Campus entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Campuses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}