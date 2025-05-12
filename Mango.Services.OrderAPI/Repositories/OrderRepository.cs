using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _context;

        public OrderRepository(DbContextOptions<ApplicationDbContext> context)
        {
            _context = context;
        }

        public async Task<bool> Store(OrderHeader orderHeader)
        {
            await using var _db = new ApplicationDbContext(_context);
            
            if (orderHeader.CouponCode == null)
            {
                orderHeader.CouponCode = string.Empty;
            }

            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_context);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(oh => oh.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }
        }
    }
}
