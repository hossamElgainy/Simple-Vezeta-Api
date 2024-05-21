using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VezetaCore.ENum;

namespace VezetaCore.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        [Required]
        public RequestStatus Status { get; set; }
        public string? DiscoundCode { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        [Required]
        public decimal FinalPrice { get; set; }
        [Required]
        [StringLength(450)]
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        [Required]
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }


        [Required]
        [ForeignKey("AppointMents")]
        public int AppointmentsId { get; set; }
        public AppointMents AppointMents { get; set; }

        [Required]
        [ForeignKey("AppointMentTimes")]
        public int AppointmentTimesId { get; set; }
        public AppointMentTimes AppointMentTimes { get; set; }
    }
}
