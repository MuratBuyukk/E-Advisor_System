using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesDepartmentIS.Repository.Concrete;

namespace InformationTechnologiesFacultyIS.Repository.Concrete
{
    public class FacultyBusiness : IDatabaseBusiness<Faculty>
    {
        public void Add(Faculty entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Faculties.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Faculty entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Faculties.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Faculties.Find(id);
                db.Faculties.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Faculty Get(Expression<Func<Faculty, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Faculties.Find(expression);
            }
        }

        public List<Faculty> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Faculties.ToList();
            }
        }

        public List<Faculty> GetAll(Expression<Func<Faculty, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Faculties.Where(expression).ToList();
            }
        }

        public Faculty GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Faculties.Find(id);
            }
        }
        public Faculty GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(Faculty entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Faculties.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Faculty> GetFaculties()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var faculties = db.Faculties
                .Include(f => f.Campus)
                .ToList();
                return faculties;
            }
        }

        public ManageFacultyViewModel GetFacultyViewModel()
        {
            CampusBusiness campusBusiness = new CampusBusiness();
            ManageFacultyViewModel viewModel = new ManageFacultyViewModel();
            viewModel.Faculties = GetFaculties();
            viewModel.Campuses = campusBusiness.GetAll();
            return viewModel;
        }
    }
}