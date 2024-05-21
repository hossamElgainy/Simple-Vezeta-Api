using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.ENum;

namespace VezetaCore.Models
{
    public class Discount
    {
        public int Id { get; set; }
        [Required]
        public string discountCode { get; set; }
        [Required]
        public int NoOfRequests { get; set;}
        [Required]
        public DiscountType DiscountType { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        [Required]
        public decimal Value { get; set; }
        public bool Active { get; set; }
    }
}
