using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetAllRequestsForPatientSpecification : BaseSpecification<Booking>
    {
        public GetAllRequestsForPatientSpecification(BaseSpecParamWithSearch specParam,string PatientId) : base(S => S.PatientId == PatientId && (string.IsNullOrEmpty(specParam.SearchVal) || S.Doctor.User.FullName.ToLower().Contains(specParam.SearchVal)))
        {
            Includes.Add(z => z.AppointMents);
            Includes.Add(z => z.AppointMentTimes);
            Includes.Add(z => z.Doctor);
            Includes.Add(z => z.Doctor.User);
            Includes.Add(z => z.Doctor.specialization);
            Includes.Add(z => z.Doctor.DoctorPrice);
            ApplyPagination(specParam.PageSize * (specParam.PageIndex - 1), specParam.PageSize);

        }
    }
}
