
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VezetaCore.ENum;


namespace Vezeta.Application.DTO.AppointmentsDtos
{
    public class AddDoctorAppointMentsDto
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        public List<AppointMentDaysDto> appointMents { get; set; }

    }
    public class AppointMentDaysDto
    {
        [Required]
        public WeekDays Days { get; set; }
        public List<AppointMentTimesDto> appointMentTimes { get; set; }

    }
    public class AppointMentTimesDto
    {
        [Required]
        public TimeSpan Time { get; set; }
    }


}
