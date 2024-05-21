

using VezetaCore.ENum;

namespace Vezeta.Application.DTO
{
    public class CouponDto
    {
        public string discountCode { get; set; }
        public int NoOfRequests { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal Value { get; set; }
    }
}
