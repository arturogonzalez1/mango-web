using Mango.Web.Models.Dtos;
using Mango.Web.Models.Dtos.ProductDtos;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ActionResult> ProductIndex()
    {
        var products = new List<ProductGetDto>();

        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.GetAsync<ResponseDto>(accessToken);

        if (response != null && response.IsSuccess == true)
        {
            products = JsonConvert.DeserializeObject<List<ProductGetDto>>(response.Response.ToString());
        }
        return View(products);
    }

    public ActionResult Details(int id)
    {
        return View();
    }

    public ActionResult ProductCreate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductCreate(ProductStoreDto productStoreDto)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.StoreAsync<ResponseDto>(productStoreDto, accessToken);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(productStoreDto);
    }

    public async Task<ActionResult> ProductEdit(int id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.GetAsync<ResponseDto>(id, accessToken);

        if (response != null && response.IsSuccess)
        {
            var product = JsonConvert.DeserializeObject<ProductGetDto>(response.Response.ToString());

            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ProductEdit(int id, ProductUpdateDto productUpdateDto)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.UpdateAsync<ResponseDto>(id, productUpdateDto, accessToken);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(productUpdateDto);
    }

    [Authorize(Roles = SD.Admin)]
    public async Task<ActionResult> ProductDelete(int id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.GetAsync<ResponseDto>(id, accessToken);

        if (response != null && response.IsSuccess)
        {
            var product = JsonConvert.DeserializeObject<ProductGetDto>(response.Response.ToString());

            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = SD.Admin)]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ProductDelete(int id, ProductDeleteDto productDeleteDto)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.DeleteAsync<ResponseDto>(id, accessToken);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(ProductIndex));
        }

        return View(productDeleteDto);
    }
}
