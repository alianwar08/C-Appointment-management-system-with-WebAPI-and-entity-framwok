using Clinic.BusinessLogic.DTOs;
using Clinic.DataAccess;
using Clinic.DataAccess.Entites;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.BusinessLogic
{
    public class AppointmentBO
    {

        private readonly ILogger<AppointmentBO> _logger;
        public  AppointmentBO()
        {
            // configure _logger;
        }

        public ResponseBO<AppointmentDto> GetAppointmentDetail(int AppointmentId)
        {
            ResponseBO<AppointmentDto> response = new ResponseBO<AppointmentDto>();

            try
            {
                ClinicDB db = new ClinicDB();
                var dr = (from n in db.Appointment
                          where n.Id == AppointmentId
                          select n).FirstOrDefault();
                if (dr == null)
                {
                    response.Status = "Error";
                    response.Message = "Data not found";
                    return response;
                }

                response.Status = "Success";
                response.Data = dr.Adapt<AppointmentDto>();
                return response;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogCritical(ex,ex.Message);

                response.Status = "Exception";
                response.Message = ex.Message;
                return response;

            }

        }

        public ResponseBO<AppointmentDto[]> GetAppointments(AppointmentFilter filter)
        {
            ResponseBO<AppointmentDto[]> response = new ResponseBO<AppointmentDto[]>();

            try
            {
                ClinicDB db = new ClinicDB();
                var drs = (from n in db.Appointment
                          select n);

                if (filter.AppointmentId != null)
                    drs = from n in drs
                          where n.Id == filter.AppointmentId
                          select n;

                if (filter.PatientId != null)
                    drs = from n in drs
                          where n.PatientId == filter.PatientId
                          select n;

                if (filter.DateFrom != null)
                    drs = from n in drs
                          where n.AppointmentDate >= filter.DateFrom
                          select n;

                if (filter.DateTo != null)
                    drs = from n in drs
                          where n.AppointmentDate <= filter.DateTo
                          select n;

                if (!string.IsNullOrEmpty(filter.PatientName))
                {
                    var ptns = (from n in db.Patient
                                where n.Name.Contains(filter.PatientName)
                               select n.Id);

                    drs = from n in drs
                          where ptns.Contains(n.PatientId)
                          select n;
                }
                               
                

                response.Status = "Success";
                response.Data = drs.Skip(filter.PageNo*filter.PageSize).Take(filter.PageSize).ToArray().Adapt<AppointmentDto[]>();
                response.TotalRecords = drs.Count();
                return response;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogCritical(ex, ex.Message);

                response.Status = "Exception";
                response.Message = ex.Message;
                return response;

            }

        }

        public ResponseBO<object> ChangeStatusToCancelled(int AppointmentId, string reason)
        {
            ResponseBO<object> response = new ResponseBO<object>();

            try
            {
                ClinicDB db = new ClinicDB();
                var dr = (from n in db.Appointment
                          where n.Id == AppointmentId
                          select n).FirstOrDefault();
                if (dr == null)
                {
                    response.Status = "Error";
                    response.Message = "Appointment not found";
                    return response;
                }

                if (dr.Status == "Cancelled")
                {
                    response.Status = "Error";
                    response.Message = "Appointment is already Cancelled";
                    return response;
                }

                dr.Status = "Cancelled";
                dr.Cancel_Reason = reason;
                db.SaveChanges();
                response.Status = "Success";
                return response;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogCritical(ex, ex.Message);

                response.Status = "Exception";
                response.Message = ex.Message;
                return response;

            }
        }

        public ResponseBO<AppointmentDto> AddAppointment(string PatientName, DateTime date)
        {
            ResponseBO<AppointmentDto> response = new ResponseBO<AppointmentDto>();

            try
            {
                ClinicDB db = new ClinicDB();
                var dr = new Patient() { Name = PatientName };

                db.Patient.Add(dr);
                db.SaveChanges();

                var apt = new Appointment() { AppointmentDate = date, PatientId = dr.Id , Status="New"};
                db.Appointment.Add(apt);
                db.SaveChanges();

                return GetAppointmentDetail(apt.Id);

                //response.Status = "Success";
                //response.Data = dr.Adapt<AppointmentDto>();
                //return response;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogCritical(ex, ex.Message);

                response.Status = "Exception";
                response.Message = ex.Message;
                return response;

            }

        }


    }

    public class AppointmentFilter
    { 
        public int? AppointmentId { get; set; }
        public string PatientName { get; set; }
        public int? PatientId { get; set; } 
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int PageNo { get; set; } = 0;
        public int PageSize { get; set; } = 20;


    }
}
