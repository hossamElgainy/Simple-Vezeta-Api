using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VezetaApi.ApiResponse;
using Vezeta.Application;
using Vezeta.Application.DTO.AccountDtos;
using Vezeta.Application.DTO;
using VezetaCore.Models;
using Vezeta.Application.Specification;
using Vezeta.Application.DTO.PatientDtos;
using Vezeta.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace VezetaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;
        private readonly IJwtToken _jwtToken;

        private long _alloedMaxLength = 5 * 1024 * 1024;
        private List<string> _alloedExtensions = new List<string> { ".jpg", ".png" };
        public PatientsController(IJwtToken jwtToken,UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtToken = jwtToken;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] dtoNewUser dto)
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
                    Image = dataStream.ToArray()
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, dto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Patient");
                    
                    var token = _jwtToken.GenerateToken(appUser,"Patient");
                    var expireAt = _jwtToken.ExtractValidToDateFromToken(token);
                    return Ok(new OkResponse<AuthDto>(new AuthDto
                    {
                        Id = appUser.Id,
                        Email = appUser.Email,
                        FullName = appUser.FullName,
                        Token = token,
                        ExpireAt = expireAt
                    }, "Patient Registered Successfully"));
                }
                else
                {
                    return BadRequest("False");
                }
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var AllPatients = await _userManager.GetUsersInRoleAsync("Patient");
            var user = AllPatients.Select(user => new PatientDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Image = user.Image
            }).ToList();

            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> NumOFPatients()
        {
            var usersInRoleCount = await _userManager.GetUsersInRoleAsync("Patient");

            return Ok(new OkResponse<int>(usersInRoleCount.Count));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOnePatientForAdmin(string Id)
        {
            var PatientRepo = _unitOfWork.Repository<ApplicationUser>();
            var PatientData =await PatientRepo.GetEntityWithSpecAsync(new GetOnePatientForPatientSpecification(Id));
            if (PatientData is null)
                return NotFound(new NotFoundResponse("No data provided for this user"));
            var MappedPatient = new GetOnePatientForAdminDto()
            {
                Id = Id,
                Image = PatientData.Image,
                FullName = PatientData.FullName,
                Email = PatientData.Email,
                Phone = PatientData.PhoneNumber,
                Gender = PatientData.Gender.ToString(),

                Requests = PatientData.PatientRequests.Select(d => new PatientRequestsForAdminDto
                {
                    Status = d.Status.ToString(),
                    Price = (d.Doctor.DoctorPrice !=null)?d.Doctor.DoctorPrice.Price:0,
                    disCountCode = d.DiscoundCode ??String.Empty,
                    FinalPrice = d.FinalPrice,
                    DoctorImage = d.Doctor.User.Image,
                    DoctorId = d.Doctor.User.Id,
                    DoctorName = d.Doctor.User.FullName,
                    Specialization = d.Doctor.specialization.Name,
                    Day = d.AppointMents.Days.ToString(),
                    Time = d.AppointMentTimes.Time
                }).ToList()
            };
            return Ok(new OkResponse<GetOnePatientForAdminDto>(MappedPatient));
        }
    }
}
