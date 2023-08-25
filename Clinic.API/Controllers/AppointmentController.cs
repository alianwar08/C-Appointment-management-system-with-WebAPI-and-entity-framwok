using Clinic.BusinessLogic;
using Clinic.BusinessLogic.DTOs;
using Clinic.DataAccess.Entites;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clinic.API.Controllers
{
    [ApiController]
    [Route("[action]")]
    [SwaggerTag("This is an example SwaggerTag")]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(ILogger<AppointmentController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTodaysAppointments")]
        [SwaggerOperationAttribute("Admin can get list of all today’s appointments")]
        public ResponseBO<AppointmentDto[]>  GetTodaysAppointments()
        {
            Clinic.BusinessLogic.AppointmentBO BO = new BusinessLogic.AppointmentBO();
            var response = BO.GetAppointments(
                new BusinessLogic.AppointmentFilter()
                {
                    DateFrom = DateTime.Today,   
                    DateTo = DateTime.Today.AddDays(1).Date.AddTicks(-1)
                });

            return response;
        }


        [HttpGet(Name = "GetAppointmentsByRange")]
        [SwaggerOperationAttribute("Admin can filter appointments by date (future or history). Date validation rules are not yet implemented!")]
        public ResponseBO<AppointmentDto[]> GetAppointmentsByRange(DateTime DateFrom, DateTime DateTo)
        {
            Clinic.BusinessLogic.AppointmentBO BO = new BusinessLogic.AppointmentBO();
            var response = BO.GetAppointments(
                new BusinessLogic.AppointmentFilter()
                {
                    DateFrom = DateFrom,
                    DateTo = DateTo
                });

            return response;
        }

        [HttpGet(Name = "GetAppointmentsByPatientId")]
        [SwaggerOperationAttribute("Admin can preview patient appointments history")]
        public ResponseBO<AppointmentDto[]> GetAppointmentsByPatientId(int PatientID)
        {
            Clinic.BusinessLogic.AppointmentBO BO = new BusinessLogic.AppointmentBO();
            var response = BO.GetAppointments(
                new BusinessLogic.AppointmentFilter()
                {
                    PatientId = PatientID
                });

            return response;
        }

        [HttpGet(Name = "GetAppointmentsByPatientName")]
        [SwaggerOperationAttribute("Admin can filter appointments by patient name (search field)")]
        public ResponseBO<AppointmentDto[]> GetAppointmentsByPatientName(string PatientName)
        {
            Clinic.BusinessLogic.AppointmentBO BO = new BusinessLogic.AppointmentBO();
            var response = BO.GetAppointments(
                new BusinessLogic.AppointmentFilter()
                {
                 PatientName    = PatientName
                });

            return response;
        }


        [HttpGet(Name = "CancelAppointment")]
        [SwaggerOperationAttribute("Admin can cancel an appointment and log the reason")]
        public ResponseBO<object> CancelAppointment(int id,string reason)
        {
            Clinic.BusinessLogic.AppointmentBO BO = new BusinessLogic.AppointmentBO();
            var response = BO.ChangeStatusToCancelled(id, reason);
               
            return response;
        }

        [HttpPost(Name = "AddTodaysAppointment")]
        [SwaggerOperationAttribute("Admin can add new appointment")]
        public ResponseBO<AppointmentDto> AddTodaysAppointment(string PatientName)
        {
            Clinic.BusinessLogic.AppointmentBO BO = new BusinessLogic.AppointmentBO();
            var response = BO.AddAppointment(PatientName, DateTime.Now);

            return response;
        }
    }
}