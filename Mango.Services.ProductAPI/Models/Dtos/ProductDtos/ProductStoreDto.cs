using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models.Dtos.ProductDtos;

public class ProductStoreDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public double Price { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public string ImageUrl { get; set; }
}
