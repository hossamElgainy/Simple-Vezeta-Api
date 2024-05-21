using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Vezeta.Application.DTO.Doctors
{
    public class AllDoctorsForAdminDto
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Gender { get; set; }
        public string Specialize { get; set; }
    }
}
