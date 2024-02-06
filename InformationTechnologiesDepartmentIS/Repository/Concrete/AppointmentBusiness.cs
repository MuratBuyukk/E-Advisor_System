using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using InformationTechnologiesDepartmentIS.Models;
using InformationTechnologiesDepartmentIS.Repository.Abstract;
using Microsoft.Ajax.Utilities;
using InformationTechnologiesDepartmentIS.Repository.Concrete;
using InformationTechnologiesDepartmentIS.Models.ViewModels;
using System.Diagnostics;
using InformationTechnologiesAppointmentStatusIS.Repository.Concrete;

namespace InformationTechnologiesAppointmentIS.Repository.Concrete
{
    public class AppointmentBusiness : IDatabaseBusiness<Appointment>
    {
        AcademicianBusiness academicianBusiness = new AcademicianBusiness();
        AppointmentStatusBusiness appointmentStatusBusiness = new AppointmentStatusBusiness();
        StudentBusiness studentBusiness = new StudentBusiness();
        public void Add(Appointment entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Appointments.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(Appointment entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Appointments.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var entity = db.Appointments.Find(id);
                db.Appointments.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Appointment Get(Expression<Func<Appointment, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Appointments.Find(expression);
            }
        }

        public List<Appointment> GetAll()
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Appointments
                    .Include(a => a.Student)
                    .Include(a => a.AppointmentStatus)
                    .Include(a => a.Academician)
                    .ToList();
            }
        }

        public List<Appointment> GetAll(Expression<Func<Appointment, bool>> expression)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Appointments
                    .Include(a => a.Student)
                    .Include(a => a.AppointmentStatus)
                    .Include(a => a.Academician)
                    .Where(expression).ToList();
            }
        }

        public Appointment GetById(int id)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                return db.Appointments
                    .Include(a => a.Student)
                    .Include(a => a.AppointmentStatus)
                    .Include(a => a.Academician)
                    .FirstOrDefault(a => a.AppointmentId == id);
            }
        }
        public Appointment GetByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Update(Appointment entity)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                db.Appointments.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void UpdateAppointmentStatus(int appointmentId, int statusId)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var appointment = db.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId);
                if (appointment != null)
                {
                    appointment.AppointmentStatusId = statusId;
                    db.SaveChanges();
                }
            }
        }

        public List<Appointment> GetAcademicianAppointment(Guid id)
        {
            var appointments = GetAll(a => a.AcademicianId == id).ToList();
            return appointments;
        }

        public List<Appointment> GetStudentAppointment(Guid id)
        {
            var appointments = GetAll(a => a.StudentId == id).ToList();
            return appointments;
        }

        public AppointmentViewModel GetAppointmentViewModel()
        {
            var academicians = academicianBusiness.GetAll();
            var appointments = new List<Appointment>();
            var appointmentViewModel = new AppointmentViewModel
            {
                Academicians = academicians,
                Appointments = appointments,
            };
            return appointmentViewModel;
        }

        public AppointmentViewModel GetAppointmentViewModelByFilter(string filterDate)
        {
            var academicians = academicianBusiness.GetAll();
            var appointments = GetAll().Where(a => a.AppointmentDate.ToString() == filterDate).ToList();
            var appointmentViewModel = new AppointmentViewModel
            {
                Academicians = academicians,
                Appointments = appointments,
            };
            return appointmentViewModel;
        }

        public AppointmentViewModel GetAppointmentViewModelByFilter(Guid academicianId, string appointmentDate)
        {
            var academicians = academicianBusiness.GetAll();
            List<Appointment> appointments;
            DateTime now = DateTime.Now;
            if (!String.IsNullOrEmpty(appointmentDate))
            {
                appointments = GetAll()
                    .Where(a => a.AcademicianId == academicianId && a.AppointmentDate.Value.ToString("dd.MM.yyyy") == appointmentDate &&
                    a.AppointmentDate > now)
                    .ToList();
            }
            else
            {
                appointments = GetAll()
                    .Where(a => a.AcademicianId == academicianId && a.AppointmentDate > now)
                    .ToList();
            }

            var appointmentViewModel = new AppointmentViewModel
            {
                Academicians = academicians,
                Appointments = appointments,
            };

            return appointmentViewModel;
        }

        public string RequestAppointment(int appointmentId, Guid studentId, string topic)
        {
            using (var context = new ITDepartmentDbEntities())
            {
                var appointment = context.Appointments
                    .SingleOrDefault(a => a.AppointmentId == appointmentId);

                var existingAppointmentOnSameDay = context.Appointments
                .Where(a => a.StudentId == studentId && a.AppointmentDate.HasValue &&
                 (DbFunctions.TruncateTime(a.AppointmentDate) == DbFunctions.TruncateTime(appointment.AppointmentDate) && a.AppointmentStatusId!=3 && appointment.AcademicianId==a.AcademicianId)).Any();

                if (appointment != null)
                {
                    if (existingAppointmentOnSameDay)
                    {
                        return "exist";
                    }
                    else
                    {
                        if (appointment.AppointmentStatusId == 1)
                        {
                            appointment.AppointmentTopic = topic;
                            appointment.StudentId = studentId;
                            appointment.AppointmentStatusId = 2;
                            context.SaveChanges();
                            return "success";
                        }
                    }
                }
                return "fail";
            }
        }

        public void CancelAppointment(int appointmentId)
        {
            using (var context = new ITDepartmentDbEntities())
            {
                var appointment = context.Appointments
                    .SingleOrDefault(a => a.AppointmentId == appointmentId);

                if (appointment != null && appointment.AppointmentStatusId == 2)
                {
                    appointment.AppointmentStatusId = 1;
                    appointment.AppointmentTopic = null;
                    appointment.StudentId = null;
                    context.SaveChanges();
                }

            }
        }

        public AppointmentViewModel GetAppointmentViewModelForAdmin(bool isCurrent)
        {
            DateTime currentTime = DateTime.Now;
            List<Appointment> appointments = new List<Appointment>();
            if (isCurrent)
            {
                appointments = GetAll()
                .Where(a => a.AppointmentDate >= currentTime).ToList();
            }
            else
            {
                appointments = GetAll()
                .Where(a => a.AppointmentDate < currentTime).ToList();
            }
            var academicians = academicianBusiness.GetAll();
            var students = studentBusiness.GetAll();
            var appointmentStatus = appointmentStatusBusiness.GetAll();

            var appointmentViewModel = new AppointmentViewModel
            {
                Academicians = academicians,
                Appointments = appointments,
                Appointment = new Appointment(),
                Students = students,
                AppointmentStatus = appointmentStatus
            };
            return appointmentViewModel;
        }


        public void ChangeAppointmentStatusToOutDated()
        {
            DateTime currentTime = DateTime.Now;
            using (var db = new ITDepartmentDbEntities())
            {
                var appointment = db.Appointments.Where(a => a.AppointmentDate < currentTime);
                if (appointment != null)
                {
                    foreach (var appt in appointment)
                    {
                        appt.AppointmentStatusId = 4;
                    }
                    db.SaveChanges();
                }
            }
        }

        public void ChangeRenewability(int appointmentId, bool value, Guid guid)
        {
            using (var db = new ITDepartmentDbEntities())
            {
                var appointment = db.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId);
                if (appointment != null)
                {
                    appointment.IsRenewable = value;

                    if (appointment.AppointmentDate is DateTime)
                    {
                        DateTime sevenDaysAgo = ((DateTime)appointment.AppointmentDate).AddDays(-7);
                        DateTime sevenDaysLater = ((DateTime)appointment.AppointmentDate).AddDays(7);

                        var appointmentsToUpdate = db.Appointments
                            .Where(a => a.AcademicianId == guid)
                            .Where(a => a.AppointmentDate == sevenDaysAgo || a.AppointmentDate == sevenDaysLater)
                            .ToList();

                        foreach (var appt in appointmentsToUpdate)
                        {
                            appt.IsRenewable = value;
                        }
                        db.SaveChanges();
                    }
                }
            }
        }
        public AppointmentViewModel GetAppointmentViewModelForStudent()
        {

            var academicians = academicianBusiness.GetAll();
            var appointments = GetAll();
            var students = studentBusiness.GetAll();
            var appointmentStatus = appointmentStatusBusiness.GetAll();
            var appointmentViewModel = new AppointmentViewModel
            {
                Academicians = academicians,
                Appointments = appointments,
                Appointment = new Appointment(),
                Students = students,
                AppointmentStatus = appointmentStatus
            };
            return appointmentViewModel;
        }
    }
}