using Mango.Services.ShoppingCartAPI.Models.Dtos.CartDtos;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDto> GetByUserId(string userId);
        Task<CartDto> Store(CartDto cartDto);
        Task<CartDto> Update(CartDto cartDto);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
