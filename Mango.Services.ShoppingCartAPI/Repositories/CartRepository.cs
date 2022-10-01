using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos.CartDtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDto> GetByUserId(string userId)
        {
            var cart = new Cart()
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId)
            };

            cart.CartDetails = await _context.CartDetails
                .Where(cd => cd.CartHeaderId == cart.CartHeader.CartHeaderId).Include(cd => cd.Product).ToListAsync();

            return _mapper.Map<CartDto>(cart);

        }

        public async Task<CartDto> Store(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);

            // check if the product exists, if not create it!
            var produdctInDb = await _context.Products
                .FirstOrDefaultAsync(product => product.Id == cartDto.CartDetails.FirstOrDefault()
                .ProductId);

            if (produdctInDb == null)
            {
                await _context.Products.AddAsync(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }

            // check if header is null

            var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(ch => ch.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                // create header and details
                await _context.CartHeaders.AddAsync(cart.CartHeader);
                await _context.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                cart.CartDetails.FirstOrDefault().CartHeader = null;
                await _context.CartDetails.AddAsync(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else {
                // if header is not null
                // check if details has te same product
                var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    cd => cd.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    cd.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().CartHeader = null;
                    await _context.CartDetails.AddAsync(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // update the count
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartHeader = null;
                    cart.CartDetails.FirstOrDefault().CartHeader = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> Update(CartDto cart)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                var cartDetails = await _context.CartDetails
                .FirstOrDefaultAsync(cd => cd.CartDetailsId == cartDetailsId);

                var totalCountOfCartItems = await _context.CartDetails.Where(cd => cd.CartHeaderId == cartDetails.CartHeaderId).CountAsync();

                _context.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders
                        .FirstOrDefaultAsync(ch => ch.CartHeaderId == cartDetails.CartHeaderId);

                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
            if (cartHeaderFromDb != null)
            {
                _context.RemoveRange(_context.CartDetails.Where(cd => cd.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _context.CartHeaders.Remove(cartHeaderFromDb);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
            cartHeaderFromDb.CouponCode = "";

            _context.CartHeaders.Update(cartHeaderFromDb);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
            cartHeaderFromDb.CouponCode = couponCode;

            _context.CartHeaders.Update(cartHeaderFromDb);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
