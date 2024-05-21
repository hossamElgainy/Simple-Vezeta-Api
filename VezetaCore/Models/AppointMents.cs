using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.ENum;

namespace VezetaCore.Models
{
    public class AppointMents
    {
        public int Id { get; set; }

        public WeekDays Days { get; set; }
        [ForeignKey("doctor")]
        public int doctorId { get; set; }
        public Doctor doctor { get; set; }
        public ICollection<AppointMentTimes> appointMentTimes { get; set; } =new HashSet<AppointMentTimes>();
    }
}
