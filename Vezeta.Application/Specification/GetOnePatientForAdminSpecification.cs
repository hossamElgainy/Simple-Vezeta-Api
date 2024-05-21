using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetOnePatientForPatientSpecification : BaseSpecification<ApplicationUser>
    {
        public GetOnePatientForPatientSpecification(string Id) : base(z => z.Id == Id)
        {
            Includes.Add(z => z.PatientRequests);
            IncludeStrings.Add("PatientRequests.Doctor.specialization");
            IncludeStrings.Add("PatientRequests.Doctor.DoctorPrice");
            IncludeStrings.Add("PatientRequests.Doctor.User");
            IncludeStrings.Add("PatientRequests.AppointMents");
            IncludeStrings.Add("PatientRequests.AppointMents.appointMentTimes");
        }
    }
}
