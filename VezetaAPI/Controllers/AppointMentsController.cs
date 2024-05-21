using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeta.Application;
using Vezeta.Application.DTO.AppointmentsDtos;
using Vezeta.Application.Specification;
using VezetaApi.ApiResponse;
using VezetaCore.Models;

namespace VezetaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointMentsController : ControllerBase
    {

        private IUnitOfWork _unitOfWork;
        public AppointMentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddAppointMents(AddDoctorAppointMentsDto dto)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var LoggedInUser = _unitOfWork.Repository<ApplicationUser>().GetBYIdAsync(UserId).Result;
            int doctorId = _unitOfWork.Repository<Doctor>().GetByAnyColumn(z => z.UserId == UserId).Result.Id;

            if (LoggedInUser is null)
                return NotFound(new NotFoundResponse("This Doctor not found"));


            try
            {
                var doctorPrice = new DoctorPrice
                {
                    DoctorId = doctorId,
                    Price = dto.Price
                };
                await _unitOfWork.Repository<DoctorPrice>().Add(doctorPrice);
                await _unitOfWork.Complete();

                foreach (var days in dto.appointMents)
                {
                    var appointment = new AppointMents
                    {
                        Days = days.Days,
                        doctorId = doctorId 
                    };
                    await _unitOfWork.Repository<AppointMents>().Add(appointment);
                    await _unitOfWork.Complete();

                    foreach (var timeDto in days.appointMentTimes)
                    {
                        var appointmenttime = new AppointMentTimes
                        {
                            Time = timeDto.Time,
                            appointMentId = appointment.Id
                        };
                        await _unitOfWork.Repository<AppointMentTimes>().Add(appointmenttime);
                        await _unitOfWork.Complete();
                    }
                }

                return Ok("Times added successfully");
            }
            catch
            {
                // Rollback the transaction in case of an exception
                _unitOfWork.Rollback();
                return BadRequest("Error In Adding Times Data");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDoctorWorkingDays(string doctorId)
        {
            var doctor =await _unitOfWork.Repository<Doctor>().GetEntityWithSpecAsync(new BaseSpecification<Doctor>(z => z.UserId == doctorId));
            var AllDays = await _unitOfWork.Repository<AppointMents>().GetAllWithSpecAsync(new BaseSpecification<AppointMents>(z => z.doctorId == doctor.Id));
            var mappedAppointMents = AllDays.Select(z=>new ListOfDoctorAppointmentsDto
            {
                Id =z.Id,
                Day =z.Days.ToString(),
            }).ToList();
            return Ok(new OkResponse<IReadOnlyList<ListOfDoctorAppointmentsDto>>(mappedAppointMents));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDoctorAppointmentsInDay(int AppointMentId)
        {
            var AllTimes =await _unitOfWork.Repository<AppointMentTimes>().GetAllWithSpecAsync(new BaseSpecification<AppointMentTimes>(z => z.appointMentId == AppointMentId));
            var mappedTimes = AllTimes.Select(z => new ListOfDoctorAppointmentsInDayDto
            {
                AppointMentTimeId = z.Id,
                Time = z.Time,
            }).ToList();
            return Ok(new OkResponse<IReadOnlyList<ListOfDoctorAppointmentsInDayDto>>(mappedTimes));
        }
    }
}
    

