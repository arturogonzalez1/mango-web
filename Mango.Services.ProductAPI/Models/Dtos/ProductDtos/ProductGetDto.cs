namespace Mango.Services.ProductAPI.Models.Dtos.ProductDtos;

public class ProductGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public string ImageUrl { get; set; }
}
