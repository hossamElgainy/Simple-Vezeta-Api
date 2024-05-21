using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeta.Application.DTO.Doctors
{
    public class GetDoctorForAdminDto
    {
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }

        public string Specialization { get; set; }
    }
}
