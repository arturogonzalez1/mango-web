﻿namespace Mango.Services.ShoppingCartAPI.Models.Dtos.CartDtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
