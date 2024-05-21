using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeta.Application.DTO.PatientDtos
{
    public class GetOnePatientForAdminDto
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public ICollection<PatientRequestsForAdminDto> Requests { get; set; }
    }
    public class PatientRequestsForAdminDto
    {
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string disCountCode { get; set; }
        public decimal FinalPrice { get; set; }
        public byte[] DoctorImage { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }

    }
}
