using Mango.Web.Models;
using Mango.Web.Models.Dtos.ProductDtos;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> GetAsync<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T> GetAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = token
            });
        }

        public async Task<T> StoreAsync<T>(ProductStoreDto productStoreDto, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = productStoreDto,
                Url = SD.ProductAPIBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T> UpdateAsync<T>(int id, ProductUpdateDto productUpdateDto, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = productUpdateDto,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = token
            });
        }

        public async Task<T> DeleteAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/products/" + id,
                AccessToken = token
            });
        }
    }
}
