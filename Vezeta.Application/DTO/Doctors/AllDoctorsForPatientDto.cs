using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VezetaCore.ENum;

namespace Vezeta.Application.DTO.Doctors
{
    public class AllDoctorsForPatientDto
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Gender { get; set; }
        public string Specialize { get; set; }
        public decimal Price { get; set; }
        public List<AppointMentDto> appointMents { get; set; }

    }
    public class AppointMentDto
    {
        public string Day { get; set; }
        public List<AppointMentTimeDto> Times { get; set; }

    }
    public class AppointMentTimeDto
    {
        public int Id { get; set; }
        public TimeSpan time { get; set; }
       
    }
}
