using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class AdvisorBusiness : IDatabaseBusiness<Advisor>
    {
        public void Add(Advisor entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Advisors.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Advisor entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Advisors.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Advisors.Find(id);
                db.Advisors.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Advisor Get(Expression<Func<Advisor, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Advisors.Find(expression);
            }
        }

        public List<Advisor> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Advisors.ToList();
            }
        }

        public List<Advisor> GetAll(Expression<Func<Advisor, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Advisors.Where(expression).ToList();
            }
        }

        public Advisor GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Advisors.Find(id);
            }
        }
        public Advisor GetByGuid(Guid id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Advisors.Find(id);
            }
        }
       
        public void Update(Advisor entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Advisors.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void UpdateStudentQuota(Guid projectAdvisorId)
        {
            using (var context = new ITDepartmentDbEntities())
            {
                var projectAdvisor = context.Advisors.SingleOrDefault(pa => pa.UserId == projectAdvisorId);
                if (projectAdvisor != null)
                {
                    projectAdvisor.StudentQuota = (projectAdvisor.StudentQuota)-1;
                    context.SaveChanges();
                }
            }
        }
        public int GetQuotasByAdvisorId(Guid advisorId)
        {
            int studentQuota = GetAll()
            .Where(p => p.UserId == advisorId)
            .Select(p => p.StudentQuota)
            .FirstOrDefault().Value;
            return studentQuota;
        }
    }
}