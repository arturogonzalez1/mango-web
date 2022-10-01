namespace Mango.Services.ShoppingCartAPI.Models.Dtos.CartDtos
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public virtual CartHeaderDto CartHeader { get; set; }
        public virtual ProductGetDto Product { get; set; }
    }
}
