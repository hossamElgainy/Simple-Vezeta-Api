using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezetaCore.Models
{
    public class DoctorPrice
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        [ForeignKey("Doctor")]

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
