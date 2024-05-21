using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetTopFiveSpecializationSpecification:BaseSpecification<Booking>
    {
        public GetTopFiveSpecializationSpecification():base()
        {
            Includes.Add(z => z.Doctor);
            Includes.Add(z => z.Doctor.specialization);
            Includes.Add(z => z.Doctor.User);
            
        }
    }
}
