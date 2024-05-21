

using VezetaCore.Models;

namespace Vezeta.Application.Specification
{
    public class GetAllDoctorsForAdminSpecification:BaseSpecification<Doctor>
    {
        public GetAllDoctorsForAdminSpecification(BaseSpecParamWithSearch specParam):base(P => string.IsNullOrEmpty(specParam.SearchVal) || P.User.FullName.ToLower().Contains(specParam.SearchVal) || P.User.Email.ToLower().Contains(specParam.SearchVal) || P.specialization.Name.ToLower().Contains(specParam.SearchVal))
        {
            Includes.Add(z => z.User);
            Includes.Add(z => z.specialization);
            ApplyPagination(specParam.PageSize * (specParam.PageIndex - 1), specParam.PageSize);
        }
    }
}
