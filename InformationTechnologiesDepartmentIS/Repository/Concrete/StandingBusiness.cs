using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;

namespace InformationTechnologiesStandingIS.Repository.Concrete
{
    public class StandingBusiness : IDatabaseBusiness<Standing>
    {
        public void Add(Standing entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Standings.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Standing entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Standings.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Standings.Find(id);
                db.Standings.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Standing Get(Expression<Func<Standing, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Standings.Find(expression);
            }
        }

        public List<Standing> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Standings.ToList();
            }
        }

        public List<Standing> GetAll(Expression<Func<Standing, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Standings.Where(expression).ToList();
            }
        }

        public Standing GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Standings.Find(id);
            }
        }

        public Standing GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }


        public void Update(Standing entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Standings.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}