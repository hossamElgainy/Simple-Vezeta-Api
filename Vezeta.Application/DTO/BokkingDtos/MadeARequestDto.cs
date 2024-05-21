using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeta.Application.DTO.BokkingDtos
{
    public class MadeARequestDto
    {
        [Required]
        public string DoctorId { get; set; }
        public string? DisCountCode { get; set; }
        [Required]
        public int AppointMentId { get; set; }
        [Required]
        public int TimeId { get; set; }
    }
}
