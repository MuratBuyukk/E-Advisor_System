using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.GraduationProjectViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterProjects
{
    public class GraduationProjectBusiness : IDatabaseBusiness<GraduationProject>
    {
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        StudentBusiness studentBusiness = new StudentBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();

        public void Add(GraduationProject entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.GraduationProjects.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(GraduationProject entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.GraduationProjects.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.GraduationProjects.Find(id);
                db.GraduationProjects.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public GraduationProject Get(Expression<Func<GraduationProject, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.GraduationProjects.Find(expression);
            }
        }

        public List<GraduationProject> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.GraduationProjects.ToList();
            }
        }

        public List<GraduationProject> GetAll(Expression<Func<GraduationProject, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.GraduationProjects.Where(expression).ToList();
            }
        }

        public GraduationProject GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.GraduationProjects.Find(id);
            }
        }
        public GraduationProject GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(GraduationProject entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.GraduationProjects.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ProjectViewModel ProjectViewModel(Guid studentId)
        {
            var student = studentBusiness.GetByGuid(studentId);
            int programId = (int)student.ProgramId;
            var program = programBusiness.GetById(programId);
            var head = academicianBusiness.GetByGuid((Guid)program.HeadId);
            var project = GetAll(p => p.StudentId == studentId).FirstOrDefault();
            var projectViewModel = new ProjectViewModel
            {
                Academician = academicianBusiness.GetByGuid(project.AcademicianId),
                Student = student,
                ProgramName = program.ProgramName,
                ProgramHead = head.AcademicianFirstName + " " + head.AcademicianLastName,
                IsEnglish = (bool)project.IsEnglish,
                IsPlagiarised = (bool)project.IsPlagiarised,
                ProjectId = project.ProjectId,
                Title = project.Title ?? "",
                Topic = project.Topic,
                DeliveryDate = project.DeliveryDate ?? null,
            };
            return projectViewModel;
        }

    }
}