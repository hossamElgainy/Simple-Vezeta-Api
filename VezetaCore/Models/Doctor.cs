using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VezetaCore.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        public int specializationId { get; set; }
        public Specialization? specialization { get; set; }

        [StringLength(450)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<AppointMents>? appointMents { get; set; } = new HashSet<AppointMents>(); 
        public ICollection<Booking>? booking { get; set; } = new HashSet<Booking>();
        public DoctorPrice? DoctorPrice { get; set; }
    }
}
