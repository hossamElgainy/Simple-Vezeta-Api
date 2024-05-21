using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VezetaApi.ApiResponse;
using VezetaCore.Models;
using Vezeta.Application;
using Vezeta.Application.Specification;
using Vezeta.Application.DTO.Doctors;
using Vezeta.VezetaApi.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace VezetaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;
        private long _alloedMaxLength = 5 * 1024 * 1024;
        private List<string> _alloedExtensions = new List<string> { ".jpg", ".png" };

        public DoctorsController(UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] dtoNewDoctor dto)
        {
            if (ModelState.IsValid)
            {
                if (!_alloedExtensions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    return BadRequest("Only .png And .jpg Images Are Allowed!");

                if (dto.Image.Length > _alloedMaxLength)
                    return BadRequest("Image Size Should Be Less Than 5MB");



                using var dataStream = new MemoryStream();
                await dto.Image.CopyToAsync(dataStream);

                ApplicationUser appUser = new()
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Gender = dto.Gender,
                    DateOfBirth = dto.DateOfBirth,
                    Image = dataStream.ToArray(),
                };


                try
                {
                    IdentityResult result = await _userManager.CreateAsync(appUser, dto.Password);
                    await _unitOfWork.Complete();

                    await _userManager.AddToRoleAsync(appUser, "Doctor");
                    await _unitOfWork.Complete();

                    var user = await _userManager.FindByEmailAsync(appUser.Email);
                    Doctor doctor = new()
                    {
                        specializationId = dto.SpecializationId,
                        UserId = user.Id
                    };
                    await _unitOfWork.Repository<Doctor>().Add(doctor);
                    await _unitOfWork.Complete();

                    return Ok(new OkResponse<bool>(true,"Doctor added successfully"));
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    return BadRequest(ex.InnerException);
                }
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> NumOFDoctors()
        {
            var usersInRoleCount = await _userManager.GetUsersInRoleAsync("Doctor");

            return Ok(new OkResponse<int>(usersInRoleCount.Count));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTopTenDoctors()
        {
            var BookingRepo = _unitOfWork.Repository<Booking>();
            var getAllBookings = await BookingRepo.GetAllWithSpecAsync(new GetTopTenDoctorsSpecification());
            var MappedTopTenDoctors = getAllBookings.Select(z => new TopTenDoctorsDto()
            {
                Image = z.Doctor.User.Image,
                FullName = z.Doctor.User.FullName,
                Specialization = (z.Doctor.specialization != null) ? z.Doctor.specialization.Name : string.Empty,
                NoOfRequests = _unitOfWork.Repository<Booking>().GetCountWithSpecAsync(new BaseSpecification<Booking>(z => z.DoctorId == z.Doctor.Id)).Result,
            }).OrderByDescending(z => z.NoOfRequests).Take(10).ToList();
            return Ok(new OkResponse<List<TopTenDoctorsDto>>(MappedTopTenDoctors));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOneDoctorForAdmin(string Id)
        {
            var DoctorRepo = _unitOfWork.Repository<ApplicationUser>();
            var DoctorData = await DoctorRepo.GetEntityWithSpecAsync(new GetOneDoctorForAdminSpecification(Id));
            if (DoctorData is null)
                return NotFound(new NotFoundResponse("No data provided for this user"));
            var MappedDoctor = new GetDoctorForAdminDto()
            {
                Image = DoctorData.Image,
                FullName = DoctorData.FullName,
                Email = DoctorData.Email,
                Phone = DoctorData.PhoneNumber,
                Gender = DoctorData.Gender.ToString(),
                Specialization = DoctorData.Doctor?.specialization?.Name
            };
            return Ok(new OkResponse<GetDoctorForAdminDto>(MappedDoctor));
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("[action]")]
        public async Task<IActionResult> AllDoctorsForPatient([FromQuery]BaseSpecParamWithSearch specParam)
        {
            var spec = new GetAllDoctorsForPatientSpecification(specParam);
            var AllDoctors = await _unitOfWork.Repository<Doctor>().GetAllWithSpecAsync(spec);
            var DoctorsCount = await _unitOfWork.Repository<Doctor>().GetCountWithSpecAsync(new BaseSpecification<Doctor>());
            var mappedDoctors = AllDoctors.Select(z => new AllDoctorsForPatientDto()
            {
                Id = z.User.Id,
                Image = z.User.Image,
                FullName = z.User.FullName,
                Email = z.User.Email,
                Phone = z.User.PhoneNumber,
                Gender = z.User.Gender.ToString(),
                Specialize = z.specialization?.Name,
                Price = (z.DoctorPrice !=null)? z.DoctorPrice.Price:0,
                appointMents = z.appointMents.Select(a => new AppointMentDto()
                {
                    Day = a.Days.ToString(),
                    Times = a.appointMentTimes.Select(t=>new AppointMentTimeDto()
                    {
                        Id=t.Id,
                        time = t.Time
                    }).ToList()
                }).ToList()
            }).ToList();
            return Ok(new Pagination<AllDoctorsForPatientDto>(specParam.PageIndex, specParam.PageSize, DoctorsCount, mappedDoctors));
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteDoctor(string Id)
        {
            var DoctorRepo = _unitOfWork.Repository<ApplicationUser>();
            var DoctorData = DoctorRepo.GetBYIdAsync(Id).Result;
            if (DoctorData is null)
                return NotFound(new NotFoundResponse("No data provided for this user"));
            try {
                DoctorRepo.Delete(DoctorData);
                await _unitOfWork.Complete();
                return Ok(new OkResponse<bool>(true,"Doctor Deleted Successfully"));

            }
            catch (Exception ex) {
                _unitOfWork.Rollback();
                return BadRequest(ex);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> UpdateDoctor(string Id,[FromForm] UpdateDoctorDto dto)
        {
            var UserRepo = _unitOfWork.Repository<ApplicationUser>();
            var ExistedUser = UserRepo.GetByAnyColumn(z => z.Id == Id).Result;
            if (ExistedUser == null)
                return NotFound(new NotFoundResponse("To User With This Id"));

            var DoctorRepo = _unitOfWork.Repository<Doctor>();
            var ExistedDoctor = DoctorRepo.GetByAnyColumn(z => z.UserId == Id).Result;
            if (ExistedDoctor == null)
                return NotFound(new NotFoundResponse("To Doctor With This Id"));

            if (!_alloedExtensions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                return BadRequest("Only .png And .jpg Images Are Allowed!");

            if (dto.Image.Length > _alloedMaxLength)
                return BadRequest("Image Size Should Be Less Than 5MB");



            using var dataStream = new MemoryStream();
            await dto.Image.CopyToAsync(dataStream);
            ExistedUser.FirstName = dto.FirstName;
            ExistedUser.LastName = dto.LastName;
            ExistedUser.Email = dto.Email;
            ExistedUser.PhoneNumber = dto.Phone;
            ExistedUser.Gender = dto.Gender;
            ExistedUser.DateOfBirth = dto.DateOfBirth;
            ExistedUser.Image = dataStream.ToArray();
            ExistedDoctor.specializationId = dto.SpecializeId;
           

            try
            {
                _unitOfWork.Repository<ApplicationUser>().Update(ExistedUser);
                await _unitOfWork.Complete();
                _unitOfWork.Repository<Doctor>().Update(ExistedDoctor);
                await _unitOfWork.Complete();
                return Ok(new OkResponse<bool>(true, "Doctor Data Updated Successfully"));
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In Updating Doctor Data"));
            }
        }
    }
}
