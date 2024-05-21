using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeta.Application;
using Vezeta.Application.DTO.BokkingDtos;
using Vezeta.Application.DTO.Doctors;
using Vezeta.Application.Specification;
using Vezeta.VezetaApi.Helpers;
using VezetaApi.ApiResponse;
using VezetaCore.ENum;
using VezetaCore.Models;

namespace VezetaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTotalNoOfRequests()
        {
            var BookingRepo = _unitOfWork.Repository<Booking>();
            var PendIngRequestsCount  =await BookingRepo.CountAsync(z=>z.Status== RequestStatus.Pending);
            var CompletedRequestsCount = await BookingRepo.CountAsync(z=>z.Status== RequestStatus.Completed);
            var CanceledRequestsCount = await BookingRepo.CountAsync(z=>z.Status== RequestStatus.Canceled);
            var RequestsToReturnDto = new NoOfRequestsDto()
            {
                TotalRequests = PendIngRequestsCount + CanceledRequestsCount + CompletedRequestsCount,
                PendingRequests = PendIngRequestsCount,
                CompletedRequests = CompletedRequestsCount,
                CanceledRequests = CanceledRequestsCount
            };
            return Ok(new OkResponse<NoOfRequestsDto>(RequestsToReturnDto));
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllDoctorRequests([FromQuery]BaseSpecParamWithSearch specParam)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var spec = new GetAllRequestsForDoctorSpecification(specParam,UserId);
            var Requests =await _unitOfWork.Repository<ApplicationUser>().GetAllWithSpecAsync(spec);

            int doctorId = _unitOfWork.Repository<Doctor>().GetByAnyColumn(z => z.UserId == UserId).Result.Id;
            int TotalRequestsCount = _unitOfWork.Repository<Booking>().CountAsync(z => z.DoctorId == doctorId).Result;


            var mappedRequests = Requests.SelectMany(z => z.Doctor.booking.Select(b => new GetAllDoctorRequestsDto()
            {

                PatientImage =  b.Patient.Image,
                PatientName =b.Patient.FullName,
                PatientGender = b.Patient.Gender.ToString(),
                PatientEmail =b.Patient.Email,
                PatientPhone=b.Patient.PhoneNumber,
                RequestId = b.Id,
                Day = b.AppointMents.Days.ToString(),
                Time =b.AppointMentTimes.Time,
                RequestStatus = b.Status.ToString(),

            })).ToList();
            return Ok(new Pagination<GetAllDoctorRequestsDto>(specParam.PageIndex, specParam.PageSize, TotalRequestsCount, mappedRequests));
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPatientRequests([FromQuery]BaseSpecParamWithSearch specParam)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var spec = new GetAllRequestsForPatientSpecification(specParam,UserId);
            var Requests =await _unitOfWork.Repository<Booking>().GetAllWithSpecAsync(spec);

            int TotalRequestsCount = _unitOfWork.Repository<Booking>().CountAsync(z => z.PatientId == UserId).Result;
            var mappedRequests = Requests.Select(b => new GetPatientRequestsDto()
            {

                DoctorImage = b.Doctor.User.Image,
                DoctorName = b.Doctor.User.FullName,
                DoctorGender = b.Doctor.User.Gender.ToString(),
                DoctorEmail = b.Doctor.User.Email,
                DoctorPhone = b.Doctor.User.PhoneNumber,
                DoctorSpecialize = (b.Doctor.specialization != null) ? b.Doctor.specialization.Name : string.Empty,
                Price = (b.Doctor.DoctorPrice != null) ? b.Doctor.DoctorPrice.Price : 0,
                RequestId = b.Id,
                Day = b.AppointMents.Days.ToString(),
                Time = b.AppointMentTimes.Time,
                RequestStatus = b.Status.ToString(),
                DiscountCode = b.DiscoundCode ?? string.Empty,
                FinalPrice = b.FinalPrice

            }).ToList();
            return Ok(new Pagination<GetPatientRequestsDto>(specParam.PageIndex, specParam.PageSize, TotalRequestsCount, mappedRequests));
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("[action]")]
        public async Task<IActionResult> MakeRequestComplete(int RequestId)
        {
            var Request = _unitOfWork.Repository<Booking>().GetByAnyColumn(z=>z.Id ==RequestId).Result;
            if (Request is null)
                return NotFound(new NotFoundResponse("No Request With This Id"));
            Request.Status = RequestStatus.Completed;
            try
            {
                _unitOfWork.Repository<Booking>().Update(Request);
                await _unitOfWork.Complete();

                return Ok(new OkResponse<bool>(true, "Booking Status Updated Successfully"));
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In Updating Booking Status"));
            }


        }
        [Authorize(Roles = "Doctor")]
        [HttpPut("[action]")]
        public async Task<IActionResult> MakeRequestCanceled(int RequestId)
        {
            var Request = _unitOfWork.Repository<Booking>().GetByAnyColumn(z=>z.Id ==RequestId).Result;
            if (Request is null)
                return NotFound(new NotFoundResponse("No Request With This Id"));
            Request.Status = RequestStatus.Canceled;
            try
            {
                _unitOfWork.Repository<Booking>().Update(Request);
                await _unitOfWork.Complete();

                return Ok(new OkResponse<bool>(true, "Booking Status Updated Successfully"));
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In Updating Booking Status"));
            }


        }
        [Authorize(Roles = "Patient")]
        [HttpPost("[action]")]
        public async Task<IActionResult> MadeARequest(MadeARequestDto dto)
        {
            // get the logged in userId
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // get doctor data according to his Id
            var doctor = _unitOfWork.Repository<Doctor>().GetByAnyColumn(z => z.UserId == dto.DoctorId).Result;
            var doctorPrice = _unitOfWork.Repository<DoctorPrice>().GetByAnyColumn(z => z.DoctorId == doctor.Id).Result.Price;

            // check if the patient have a discount Coupon
            if(!string.IsNullOrEmpty(dto.DisCountCode))
            {
                // get the coupon from the database  and check if it is valid and active
                var discount = _unitOfWork.Repository<Discount>().GetByAnyColumn(z => z.discountCode == dto.DisCountCode).Result;
                if(discount is not null && discount.Active==true)
                {
                    // check if this coupon is used before
                    var UserDiscountBefore = _unitOfWork.Repository<Booking>().GetByAnyColumn(z => z.DiscoundCode == dto.DisCountCode).Result;
                    if (UserDiscountBefore is not null)
                        return BadRequest(new BadRequestResponse($"This Coupon {dto.DisCountCode} Used Before"));

                    // compare the NoOfPatient Requests With The Coupon Requests
                    var PatientRequests = _unitOfWork.Repository<Booking>().GetCountWithSpecAsync(new BaseSpecification<Booking>(z => z.PatientId == UserId)).Result;
                    if(PatientRequests < discount.NoOfRequests)
                        return BadRequest(new BadRequestResponse($" Sorry You Should Complete {discount.NoOfRequests} Requests To Use This Coupon"));

                    // calculate final Price If There is a discount coupon
                    decimal finalPrice = 0;
                    if(discount.DiscountType == DiscountType.Value)
                    {
                        finalPrice = doctorPrice - discount.Value;
                    }
                    else if (discount.DiscountType == DiscountType.Percentage)
                    {
                        finalPrice = doctorPrice - (doctorPrice * (discount.Value/100));
                    }
                    else
                    {
                        finalPrice = doctorPrice;
                    }

                    var BookingRequest = new Booking()
                    {
                        Status = RequestStatus.Pending,
                        DiscoundCode = dto.DisCountCode,
                        FinalPrice = finalPrice,
                        PatientId = UserId,
                        DoctorId = doctor.Id,
                        AppointmentsId = dto.AppointMentId,
                        AppointmentTimesId = dto.TimeId
                    };
                    try
                    {

                        await _unitOfWork.Repository<Booking>().Add(BookingRequest);
                        await _unitOfWork.Complete();

                        return Ok(new OkResponse<bool>(true, " You Booking Saved Successfully"));
                    }
                    catch
                    {
                        _unitOfWork.Rollback();
                        return BadRequest(new BadRequestResponse("Error In Saving Your Booking"));
                    }
                }
                return NotFound(new NotFoundResponse("This Coupon has Expired"));
            }
            var request = new Booking()
            {
                Status = RequestStatus.Pending,
                DiscoundCode = null,
                FinalPrice = doctorPrice,
                PatientId = UserId,
                DoctorId = doctor.Id,
                AppointmentsId = dto.AppointMentId,
                AppointmentTimesId = dto.TimeId
            };
            try
            {

                await _unitOfWork.Repository<Booking>().Add(request);
                await _unitOfWork.Complete();

                return Ok(new OkResponse<bool>(true, " You Booking Saved Successfully"));
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In Saving Your Booking"));
            }                    
        }
    }
}
