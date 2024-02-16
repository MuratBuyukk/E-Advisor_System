using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesFacultyIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class DepartmentBusiness : IDatabaseBusiness<Department>
    {
        FacultyBusiness facultyBusiness = new FacultyBusiness();
        public void Add(Department entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Departments.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Department entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Departments.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Departments.Find(id);
                db.Departments.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Department Get(Expression<Func<Department, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Departments.Find(expression);
            }
        }

        public List<Department> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Departments.ToList();
            }
        }

        public List<Department> GetAll(Expression<Func<Department, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Departments.Where(expression).ToList();
            }
        }

        public Department GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Departments.Find(id);
            }
        }

        public Department GetByGuid(Guid id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Departments.Find(id);
            }
        }

        public void Update(Department entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Departments.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Department> GetDepartments()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var departments = db.Departments
                    .Include(d => d.Faculty)
                     .Include(d => d.Faculty.Campus) 
                    .ToList();
                return departments;
            }
        }

        public ManageDepartmentViewModel GetDepartmentViewModel()
        {
            var viewModel = new ManageDepartmentViewModel();
            viewModel.Faculties = facultyBusiness.GetAll();
            viewModel.Departments = GetDepartments();
            return viewModel;
        }
    }
}