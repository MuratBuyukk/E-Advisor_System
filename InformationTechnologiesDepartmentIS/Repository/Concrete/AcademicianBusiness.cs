using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class AcademicianBusiness : IDatabaseBusiness<Academician>
    {
        ProgramBusiness programBusiness = new ProgramBusiness();
        public void Add(Academician entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Academicians.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Academician entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Academicians.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Academicians.Find(id);
                db.Academicians.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Academician Get(Expression<Func<Academician, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Academicians.Find(expression);
            }
        }

        public List<Academician> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Academicians.ToList();
            }
        }

        public List<Academician> GetAll(Expression<Func<Academician, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Academicians.Where(expression).ToList();
            }
        }

        public Academician GetByGuid(Guid id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Academicians.Find(id);
            }
        }

        public Academician GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Academicians.Find(id);
            }
        }

        public void Update(Academician entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Academicians.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Academician> GetAcademicians()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var academicians = db.Academicians
                    .Include(a => a.Program)
                     .Include(a => a.Program.Department)
                     .Include(a => a.Program.Department.Faculty)
                     .Include(a => a.Program.Department.Faculty.Campus)
                    .ToList();
                return academicians;
            }
        }

        public ManageAcademicianViewModel GetAcademicianViewModel()
        {
            var viewModel = new ManageAcademicianViewModel();
            viewModel.Academicans = GetAcademicians();
            viewModel.Programs = programBusiness.GetPrograms();
            return viewModel;
        }

    }
}