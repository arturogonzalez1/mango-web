using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Messages;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Models.Dtos.CartDtos;
using Mango.Services.ShoppingCartAPI.RabbitMQSender;
using Mango.Services.ShoppingCartAPI.Repositories;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartAPIController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMessageBus _messageBus;
        private readonly IRabbitMQCartMessageSender _rabbitMQCartMessageSender;

        public CartAPIController(ICartRepository cartRepository,
            ICouponRepository couponRepository, 
            IProductRepository productRepository,
            IMessageBus messageBus,
            IRabbitMQCartMessageSender rabbitMQCartMessageSender)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _productRepository = productRepository;
            _messageBus = messageBus;
            _rabbitMQCartMessageSender = rabbitMQCartMessageSender;
        }

        [HttpGet("getcart/{userId}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> GetCart(string userId)
        {
            var response = new ResponseDto<CartDto>();

            try
            {
                var cartDto = await _cartRepository.GetByUserId(userId);
                response.Response = cartDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpPost("addcart")]
        public async Task<ActionResult<ResponseDto<CartDto>>> AddCart([FromBody] CartDto cartDto)
        {
            var response = new ResponseDto<CartDto>();

            try
            {
                var cartDt = await _cartRepository.Store(cartDto);
                response.Response = cartDt;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpPost("updatecart")]
        public async Task<ActionResult<ResponseDto<CartDto>>> UpdateCart([FromBody] CartDto cartDto)
        {
            var response = new ResponseDto<CartDto>();

            try
            {
                var cartDt = await _cartRepository.Store(cartDto);
                response.Response = cartDt;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpPost("removecart")]
        public async Task<ActionResult<ResponseDto<CartDto>>> RemoveCart([FromBody] int cartId)
        {
            var response = new ResponseDto<CartDto>();

            try
            {
                var isSuccess = await _cartRepository.RemoveFromCart(cartId);
                response.IsSuccess = isSuccess;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpPost("applycoupon")]
        public async Task<ActionResult<ResponseDto<CartDto>>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            var response = new ResponseDto<CartDto>();

            try
            {
                var isSuccess = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
                response.IsSuccess = isSuccess;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpPost("removecoupon")]
        public async Task<ActionResult<ResponseDto<CartDto>>> RemoveCoupon([FromBody] string userId)
        {
            var response = new ResponseDto<CartDto>();

            try
            {
                var isSuccess = await _cartRepository.RemoveCoupon(userId);
                response.IsSuccess = isSuccess;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<ResponseDto<string>>> Checkout(CheckoutHeaderDto checkoutHeader)
        {
            var response = new ResponseDto<string>();

            try
            {
                var cartDto = await _cartRepository.GetByUserId(checkoutHeader.UserId);
                if (cartDto == null)
                {
                    return BadRequest();
                }

                response.Errors = new List<string>();

                if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
                {
                    var coupon = await _couponRepository.GetCoupon(checkoutHeader.CouponCode);
                    if (checkoutHeader.DiscountTotal != coupon.DiscountAmount)
                    {
                        response.IsSuccess = false;
                        response.Errors.Add("Coupon price has changed, please confirm.");
                        response.Message = "The order details has changed, please confirm.";

                        return response;
                    }
                }

                foreach (var cartDetail in checkoutHeader.CartDetails)
                {
                    var productFromDb = await _productRepository.GetProduct(cartDetail.ProductId);

                    if (cartDetail.Product.Price != productFromDb.Price)
                    {
                        response.IsSuccess = false;
                        response.Errors.Add($"{productFromDb.Name}'s price has changed, please confirm.");
                        response.Message = "The order details has changed, please confirm.";

                        return response;
                    }
                }

                checkoutHeader.CartDetails = cartDto.CartDetails;

                // * Using Azure Service BUS
                //await _messageBus.PublishMessage(checkoutHeader, "checkoutqueue");

                // * Using RabbitMQ
                _rabbitMQCartMessageSender.SendMessage(checkoutHeader, "checkoutqueue");

                await _cartRepository.ClearCart(checkoutHeader.UserId);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }
    }
}
