using Mango.Services.CouponAPI.Models.Dtos;
using Mango.Services.CouponAPI.Models.Dtos.CouponDtos;
using Mango.Services.CouponAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupons")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet("{couponCode}")]
        public async Task<ActionResult<ResponseDto<CouponDto>>> GetDiscountForCode(string couponCode)
        {
            var response = new ResponseDto<CouponDto>();

            try
            {
                var couponDto = await _couponRepository.GetCouponByCode(couponCode);
                response.Response = couponDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }
    }
}
