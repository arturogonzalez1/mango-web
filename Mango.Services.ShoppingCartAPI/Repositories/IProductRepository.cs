using Mango.Services.ShoppingCartAPI.Models.Dtos.ProductDtos;

namespace Mango.Services.ShoppingCartAPI.Repositories
{
    public interface IProductRepository
    {
        Task<ProductGetDto> GetProduct(int productId);
    }
}
