using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezetaCore.Models
{
    public class AppointMentTimes
    {
        public int Id { get; set; }
        public int appointMentId { get; set; }
        public AppointMents appointMent { get; set; }
        public TimeSpan Time { get; set; }
    }
}
