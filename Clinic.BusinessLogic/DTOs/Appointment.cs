using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.BusinessLogic.DTOs
{
    public class AppointmentDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int PatientId { get; set; }
        public string? Status { get; set; }
        public string? Cancel_Reason { get; set; }


        //[ForeignKey("PatientId")]
        //public virtual Patient Patient { get; set; }
    }
}
