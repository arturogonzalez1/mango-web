using Mango.Services.ProductAPI.Models.Dtos.ProductDtos;

namespace Mango.Services.ProductAPI.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductGetDto>> Get();
    Task<ProductGetDto> Get(int id);
    Task<ProductGetDto> Store(ProductStoreDto productStoreDto);
    Task<ProductGetDto> Update(ProductUpdateDto productStoreDto);
    Task<ProductGetDto> Delete(int id);
}
