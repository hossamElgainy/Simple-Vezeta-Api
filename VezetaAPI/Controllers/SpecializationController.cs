using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeta.Application;
using Vezeta.Application.DTO.SpecializationsDtos;
using Vezeta.Application.Specification;
using VezetaApi.ApiResponse;
using VezetaCore.Models;


namespace VezetaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {       
        private IUnitOfWork _unitOfWork;       
        public SpecializationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTopFiveSpecialization()
        {
            var result = await _unitOfWork.Repository<Booking>().GetAllWithSpecAsync(new GetTopFiveSpecializationSpecification());
            var hoso =result.Select(z => new TopFiveSpecializationDto()
            {
                FullName=z.Doctor.specialization.Name,
                NoOfRequests=_unitOfWork.Repository<Booking>().GetCountWithSpecAsync(new BaseSpecification<Booking>(z => z.DoctorId == z.Doctor.Id)).Result,
            }).OrderByDescending(z=>z.NoOfRequests).ToList();
            return Ok(new OkResponse<List<TopFiveSpecializationDto>>(hoso));
        }
    }
}
