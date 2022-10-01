using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos.ProductDtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductGetDto>> Get()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductGetDto>>(products);
        }

        public async Task<ProductGetDto> Get(int id)
        {
            var product = await _context.Products.Where(product => product.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductGetDto>(product);
        }

        public async Task<ProductGetDto> Store(ProductStoreDto productStoreDto)
        {
            var product = _mapper.Map<Product>(productStoreDto);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductGetDto>(product);
        }

        public async Task<ProductGetDto> Update(ProductUpdateDto productUpdateDto)
        {
            var product = _mapper.Map<Product>(productUpdateDto);

            _context.Update(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductGetDto>(product);
        }

        public async Task<ProductGetDto> Delete(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(product => product.Id == id);

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return _mapper.Map<ProductGetDto>(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
