﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.AdministratorViewModels;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete
{
    public class StudentBusiness : IDatabaseBusiness<Student>
    {
        public void Add(Student entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Students.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Student entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Students.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Students.Find(id);
                db.Students.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public void Delete(Guid id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Students.Find(id);
                db.Students.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public Student Get(Expression<Func<Student, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Students.Find(expression);
            }
        }

        public List<Student> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Students.ToList();
            }
        }

        public List<Student> GetAll(Expression<Func<Student, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Students.Where(expression).ToList();
            }
        }

        public Student GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Students.Find(id);
            }
        }

        public Student GetByGuid(Guid id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Students.Find(id);
            }
        }
        public void Update(Student entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Students.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public int GetStudentProjectStatusById(Guid id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var project = db.Projects.FirstOrDefault(p => p.Student.UserId == id);

                if (project != null)
                {
                    return (int)project.ProjectStatusId;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<Student> getStudents() {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Students
                    .Include(p => p.Program)
                    .Include(p => p.Standing)
                    .ToList();
            }
        }

        public AddUserViewModel AddUserViewModel()
        {
            var students = getStudents();
            ProgramBusiness programBusiness = new ProgramBusiness();
            var programs = programBusiness.GetAll();
            var addUserViewModel = new AddUserViewModel
            {
                Students = students,
                Programs = programs
            };
            return addUserViewModel;
        }

    }
}