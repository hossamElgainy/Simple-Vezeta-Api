using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetAllRequestsForDoctorSpecification:BaseSpecification<ApplicationUser>
    {
        public GetAllRequestsForDoctorSpecification(BaseSpecParamWithSearch specParam,string DoctorId):base(z=>z.Id==DoctorId) 
        {
            Includes.Add(z => z.Doctor.booking);
            IncludeStrings.Add("Doctor.booking.Patient");
            IncludeStrings.Add("Doctor.booking.AppointMents");
            IncludeStrings.Add("Doctor.booking.AppointMentTimes");
            ApplyPagination(specParam.PageSize * (specParam.PageIndex - 1), specParam.PageSize);

        }
    }
}
