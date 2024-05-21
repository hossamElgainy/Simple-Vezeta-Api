using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetOneDoctorForAdminSpecification:BaseSpecification<ApplicationUser>
    {
        public GetOneDoctorForAdminSpecification(string Id):base(z=>z.Id ==Id)
        {
            Includes.Add(z => z.Doctor.specialization);
        }
    }
}
