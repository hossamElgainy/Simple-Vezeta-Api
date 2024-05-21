using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeta.Application;
using Vezeta.Application.DTO;
using VezetaApi.ApiResponse;
using VezetaCore.Models;


namespace VezetaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCoupon(CouponDto dto)
        {
            var discount = new Discount
            {
                DiscountType = dto.DiscountType,
                NoOfRequests = dto.NoOfRequests,
                discountCode = dto.discountCode,
                Value = dto.Value,
                Active = true
            };
            try
            {
                await _unitOfWork.Repository<Discount>().Add(discount);
                await _unitOfWork.Complete();
                return Ok(new OkResponse<bool>(true,"Data Saved Successfully"));
            }catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In Saving The Data"));
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var Coupon = _unitOfWork.Repository<Discount>().GetByAnyColumn(z=>z.Id ==id).Result;
            if (Coupon == null)
                return NotFound(new NotFoundResponse("To Coupon With This Id"));
            try
            {
                _unitOfWork.Repository<Discount>().Delete(Coupon);
                await _unitOfWork.Complete();
                return Ok(new OkResponse<bool>(true, "Coupon Deleted Successfully"));
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In Deleteing This Coupon"));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> DeactivateCoupon(int Id)
        {
            var Existedcoupon = _unitOfWork.Repository<Discount>().GetByAnyColumn(z => z.Id == Id).Result;
            if (Existedcoupon == null)
                return NotFound(new NotFoundResponse("To Coupon With This Id"));
            Existedcoupon.Active = false;
            try
            {
                _unitOfWork.Repository<Discount>().Update(Existedcoupon);
                await _unitOfWork.Complete();
                return Ok(new OkResponse<bool>(true, "Coupon DeActivated Successfully"));
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In DeActivated This Coupon"));
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> UpdateCoupon(int Id,CouponDto dto)
        {
            var Existedcoupon = _unitOfWork.Repository<Discount>().GetByAnyColumn(z => z.Id == Id).Result;
            if (Existedcoupon == null)
                return NotFound(new NotFoundResponse("To Coupon With This Id"));
            var AppliedToRequest = _unitOfWork.Repository<Booking>().GetByAnyColumn(z => z.DiscoundCode == Existedcoupon.discountCode).Result;
            if (AppliedToRequest is not null)
                return BadRequest(new BadRequestResponse("This Coupon Applied To Request"));

            Existedcoupon.discountCode = dto.discountCode;
            Existedcoupon.NoOfRequests = dto.NoOfRequests;
            Existedcoupon.DiscountType = dto.DiscountType;
            Existedcoupon.Value = dto.Value;
            Existedcoupon.Active = Existedcoupon.Active;
            
            try
            {
                _unitOfWork.Repository<Discount>().Update(Existedcoupon);
                await _unitOfWork.Complete();
                return Ok(new OkResponse<bool>(true, "Coupon Updated Successfully"));
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest(new BadRequestResponse("Error In Updating This Coupon"));
            }
        }
    }
}
