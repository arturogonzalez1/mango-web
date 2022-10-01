using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos.ProductDtos;

namespace Mango.Services.ProductAPI.Configurations;

public class AutoMapperConfiguration
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<ProductStoreDto, Product>();
            config.CreateMap<ProductUpdateDto, Product>();
            config.CreateMap<Product, ProductGetDto>();
        });
    }
}
