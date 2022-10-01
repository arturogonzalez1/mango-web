using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Models.Dtos.ProductDtos;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;

        public ProductRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductGetDto> GetProduct(int productId)
        {
            var httpResponse = await _httpClient.GetAsync($"/api/products/{productId}");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto<ProductGetDto>>(stringResponse);

            if (response != null && response.IsSuccess)
            {
                return response.Response;
            }

            return new ProductGetDto();
        }
    }
}
