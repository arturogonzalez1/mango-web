using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Models.Dtos.CartDtos;
using Mango.Web.Models.Dtos.ProductDtos;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService,
            ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var products = new List<ProductGetDto>();

            var response = await _productService.GetAsync<ResponseDto>("");

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductGetDto>>(response.Response.ToString());
            }

            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            var product = new ProductGetDto();

            var response = await _productService.GetAsync<ResponseDto>(productId, "");

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductGetDto>(response.Response.ToString());
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductGetDto productDto)
        {
            var cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value,
                    CouponCode = String.Empty
                }
            };

            var cartDetails = new CartDetailsDto()
            {
                Count = productDto.Count,
                ProductId = productDto.Id,
                CartHeader = cartDto.CartHeader
            };

            var response = await _productService.GetAsync<ResponseDto>(productDto.Id, "");

            if (response != null && response.IsSuccess)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductGetDto>(response.Response.ToString());
            }

            var cartDetailsDtos = new List<CartDetailsDto>();
            cartDetailsDtos.Add(cartDetails);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResponse = await _cartService.AddToCartAsync<ResponseDto>(cartDto, accessToken);

            if (addToCartResponse != null && addToCartResponse.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}