using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetAllDoctorsForPatientSpecification:BaseSpecification<Doctor>
    {
        public GetAllDoctorsForPatientSpecification(BaseSpecParamWithSearch specParam) : base(P => string.IsNullOrEmpty(specParam.SearchVal) || P.User.FullName.ToLower().Contains(specParam.SearchVal) || P.User.Email.ToLower().Contains(specParam.SearchVal) || P.specialization.Name.ToLower().Contains(specParam.SearchVal))
        {
            Includes.Add(z => z.User);
            Includes.Add(z => z.specialization);
            Includes.Add(z => z.appointMents);
            Includes.Add(z => z.DoctorPrice);
            IncludeStrings.Add("appointMents.appointMentTimes");
            ApplyPagination(specParam.PageSize * (specParam.PageIndex - 1), specParam.PageSize);
        }
    }
}
