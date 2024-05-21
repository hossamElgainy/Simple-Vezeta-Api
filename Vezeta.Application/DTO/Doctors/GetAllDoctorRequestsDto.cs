using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeta.Application.DTO.Doctors
{
    public class GetAllDoctorRequestsDto
    {
        public byte[] PatientImage { get; set; }
        public string PatientName { get; set; }
        public string PatientGender { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
        public int RequestId { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }
        public string RequestStatus { get; set; }
    }
}
