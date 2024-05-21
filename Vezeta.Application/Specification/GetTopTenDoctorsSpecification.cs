using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetTopTenDoctorsSpecification:BaseSpecification<Booking>
    {
        public GetTopTenDoctorsSpecification():base()
        {
            Includes.Add(z => z.Doctor);
            Includes.Add(z => z.Doctor.specialization);
            Includes.Add(z => z.Doctor.User);
        }
    }
}
