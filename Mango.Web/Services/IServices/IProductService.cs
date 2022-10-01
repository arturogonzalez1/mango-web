using Mango.Web.Models.Dtos.ProductDtos;

namespace Mango.Web.Services.IServices;

public interface IProductService : IBaseService
{
    Task<T> GetAsync<T>(string token);
    Task<T> GetAsync<T>(int id, string token);
    Task<T> StoreAsync<T>(ProductStoreDto productStoreDto, string token);
    Task<T> UpdateAsync<T>(int id, ProductUpdateDto productUpdateDto, string token);
    Task<T> DeleteAsync<T>(int id, string token);
}
