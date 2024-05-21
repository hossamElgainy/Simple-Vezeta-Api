using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeta.Application.DTO.BokkingDtos
{
    public class GetPatientRequestsDto
    {
        public byte[] DoctorImage { get; set; }
        public string DoctorName { get; set; }
        public string DoctorGender { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorSpecialize { get; set; }
        public decimal Price { get; set; }

        public int RequestId { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }
        public string RequestStatus { get; set; }
        public string DiscountCode { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
