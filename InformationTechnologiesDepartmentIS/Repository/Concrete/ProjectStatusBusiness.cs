using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class ProjectStatusBusiness : IDatabaseBusiness<ProjectStatu>
    {
        public void Add(ProjectStatu entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.ProjectStatus.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(ProjectStatu entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.ProjectStatus.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.ProjectStatus.Find(id);
                db.ProjectStatus.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public ProjectStatu Get(Expression<Func<ProjectStatu, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.ProjectStatus.Find(expression);
            }
        }

        public List<ProjectStatu> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.ProjectStatus.ToList();
            }
        }

        public List<ProjectStatu> GetAll(Expression<Func<ProjectStatu, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.ProjectStatus.Where(expression).ToList();
            }
        }

        public ProjectStatu GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.ProjectStatus.Find(id);
            }
        }

        public ProjectStatu GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }


        public void Update(ProjectStatu entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.ProjectStatus.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}