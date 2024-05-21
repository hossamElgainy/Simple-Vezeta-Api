using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeta.Application.DTO.AppointmentsDtos
{
    public class ListOfDoctorAppointmentsInDayDto
    {
        public int AppointMentTimeId { get; set; }
        public TimeSpan Time { get; set; }
    }
}
