using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesFacultyIS.Repository.Concrete;
using InformationTechnologiesDepartmentIS.Repository.Concrete;


namespace InformationTechnologiesProgramIS.Repository.Concrete
{
    public class ProgramBusiness : IDatabaseBusiness<Program>
    {

        public void Add(Program entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Programs.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Program entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Programs.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Programs.Find(id);
                db.Programs.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Program Get(Expression<Func<Program, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Programs.Find(expression);
            }
        }

        public List<Program> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Programs.ToList();
            }
        }

        public List<Program> GetAll(Expression<Func<Program, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Programs.Where(expression).ToList();
            }
        }

        public Program GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Programs.Find(id);
            }
        }
        public Program GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(Program entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Programs.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Program> GetPrograms()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var programs = db.Programs
                    .Include(p => p.Department) 
                     .Include(p => p.Academician)
                     .Include(p => p.Department.Faculty)
                     .Include(p => p.Department.Faculty.Campus)
                    .ToList();
                return programs;
            }
        }

        public ManageProgramViewModel GetProgramViewModel()
        {
            DepartmentBusiness departmentBusiness = new DepartmentBusiness();
            AcademicianBusiness academicianBusiness = new AcademicianBusiness();
            var viewModel = new ManageProgramViewModel();
            viewModel.Departments = departmentBusiness.GetAll();
            viewModel.Programs = GetPrograms();
            viewModel.Academicians = academicianBusiness.GetAll();
            return viewModel;
        }
    }
}