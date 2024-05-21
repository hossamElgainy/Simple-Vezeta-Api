

namespace Vezeta.Application.DTO.Doctors
{
    public class TopTenDoctorsDto
    {
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public int NoOfRequests { get; set; }
    }
}
