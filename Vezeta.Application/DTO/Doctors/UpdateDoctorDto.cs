using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.ENum;

namespace Vezeta.Application.DTO.Doctors
{
    public class UpdateDoctorDto
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int SpecializeId { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]

        public DateTime DateOfBirth { get; set; }
    }
}
