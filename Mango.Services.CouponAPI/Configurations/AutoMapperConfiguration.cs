using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dtos.CouponDtos;

namespace Mango.Services.CouponAPI.Configurations
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
            });
        }
    }
}
