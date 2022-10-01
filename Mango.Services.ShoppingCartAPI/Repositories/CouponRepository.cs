using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Models.Dtos.CouponDtos;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var httpResponse = await _httpClient.GetAsync($"/api/coupons/{couponName}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto<CouponDto>>(stringResponse);

            if (response != null && response.IsSuccess)
            {
                return response.Response;
            }

            return new CouponDto();
        }
    }
}
