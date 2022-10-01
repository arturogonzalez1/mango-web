using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;

namespace Mango.Services.ProductAPI.Seeds;

public class ProductSeed
{
    public static void Seed(ApplicationDbContext context)
    {
        var list = new List<Product>();

        if (!context.Products.Any(p => p.Id == 1))
        {
            list.Add(new Product
            {
                Name = "Samosa",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "",
                CategoryName = "Appetizer"
            });
        }

        if (!context.Products.Any(p => p.Id == 2))
        {
            list.Add(new Product
            {
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "",
                CategoryName = "Appetizer"
            });
        }

        if (!context.Products.Any(p => p.Id == 3))
        {
            list.Add(new Product
            {
                Name = "Sweet Pie",
                Price = 10.99,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "",
                CategoryName = "Dessert"
            });
        }

        if (!context.Products.Any(p => p.Id == 4))
        {
            list.Add(new Product
            {
                Name = "Pav Bhaji",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "",
                CategoryName = "Entree"
            });
        }

        if (list.Any())
        {
            context.Products.AddRangeAsync(list);
            context.SaveChanges();
        }
    }
}
