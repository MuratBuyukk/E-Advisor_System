using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Web;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using InformationTechnologiesDepartmentIS.Models.ViewModels.MasterThesisViewModels;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using InformationTechnologiesProgramIS.Repository.Concrete;

namespace InformationTechnologiesDepartmentIS.Repository.Concrete.MasterTheses
{
    public class MasterThesBusiness : IDatabaseBusiness<MasterThes>
    {
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        StudentBusiness studentBusiness = new StudentBusiness();
        ProgramBusiness programBusiness = new ProgramBusiness();
        public void Add(MasterThes entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.MasterTheses.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(MasterThes entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.MasterTheses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.MasterTheses.Find(id);
                db.MasterTheses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public MasterThes Get(Expression<Func<MasterThes, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.MasterTheses.Find(expression);
            }
        }

        public List<MasterThes> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.MasterTheses.ToList();
            }
        }

        public List<MasterThes> GetAll(Expression<Func<MasterThes, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.MasterTheses.Where(expression).ToList();
            }
        }

        public MasterThes GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.MasterTheses.Find(id);
            }
        }
        public MasterThes GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(MasterThes entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.MasterTheses.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ThesisViewModel ThesisViewModel(Guid studentId)
        {
            var student = studentBusiness.GetByGuid(studentId);
            int programId = (int)student.ProgramId;
            var program = programBusiness.GetById(programId);
            var head = academicianBusiness.GetByGuid((Guid)program.HeadId);
            var thesis = GetAll(t => t.StudentId == studentId).FirstOrDefault();
            var thesisViewModel = new ThesisViewModel
            {
                ThesisId = thesis.ThesisId,
                Student = student,
                Advisor = academicianBusiness.GetByGuid(thesis.AdvisorId),
                CoAdvisor = thesis.CoAdvisorId != null ? academicianBusiness.GetByGuid((Guid)thesis.CoAdvisorId) : null,
                Topic = thesis.Topic,
                Title = thesis.Title,
                IsEnglish = (bool)thesis.IsEnglish,
                ProgramName = program.ProgramName,
                ProgramHead = head.AcademicianFirstName + " " + head.AcademicianLastName,
                IsPlagiarised = thesis.IsPlagiarised ?? false,
                DeliveryDate = thesis.DeliveryDate.HasValue ? thesis.DeliveryDate.Value : (DateTime?)null,
            };
            return thesisViewModel;
        }

    }
}