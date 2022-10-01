using Mango.Services.ProductAPI.Constants;
using Mango.Services.ProductAPI.Models.Dtos;
using Mango.Services.ProductAPI.Models.Dtos.ProductDtos;
using Mango.Services.ProductAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto<IEnumerable<ProductGetDto>>>> Get()
    {
        var response = new ResponseDto<IEnumerable<ProductGetDto>>();

        try
        {
            var productsDto = await _repository.Get();
            response.Response = productsDto;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ProductConstants.PRODUCT_GET_ERROR;
            response.Errors = new List<string> { ex.ToString() };
        }

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto<ProductGetDto>>> Get(int id)
    {
        var response = new ResponseDto<ProductGetDto>();

        try
        {
            var productDto = await _repository.Get(id);
            response.Response = productDto;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ProductConstants.PRODUCT_GETBYID_ERROR;
            response.Errors = new List<string> { ex.ToString() };
        }

        return Ok(response);
    }

    //[Authorize(Roles = SD.Admin)]
    [HttpPost]
    public async Task<ActionResult<ResponseDto<ProductGetDto>>> Post([FromBody] ProductStoreDto productStoreDto)
    {
        var response = new ResponseDto<ProductGetDto>();

        try
        {
            var productDto = await _repository.Store(productStoreDto);
            response.Response = productDto;
            response.Message = ProductConstants.PRODUCT_CREATED;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ProductConstants.PRODUCT_CREATE_ERROR;
            response.Errors = new List<string> { ex.ToString() };
        }

        return response;
    }

    //[Authorize(Roles = SD.Admin)]
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseDto<ProductGetDto>>> Put(int id, [FromBody] ProductUpdateDto productUpdateDto)
    {
        var response = new ResponseDto<ProductGetDto>();

        try
        {
            var productDto = await _repository.Update(productUpdateDto);
            response.Response = productDto;
            response.Message = ProductConstants.PRODUCT_UPDATED;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ProductConstants.PRODUCT_UPDATE_ERROR;
            response.Errors = new List<string> { ex.ToString() };
        }

        return response;
    }

    //[Authorize(Roles = SD.Admin)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseDto<ProductGetDto>>> Delete(int id)
    {
        var response = new ResponseDto<ProductGetDto>();

        try
        {
            var productDto = await _repository.Delete(id);
            response.Response = productDto;
            response.Message = ProductConstants.PRODUCT_DELETED;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ProductConstants.PRODUCT_DELETE_ERROR;
            response.Errors = new List<string> { ex.ToString() };
        }

        return response;
    }
}
