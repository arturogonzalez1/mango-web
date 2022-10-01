using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI.Seeds
{
    public class CouponSeed
    {
        public static void Seed(ApplicationDbContext context)
        {
            var list = new List<Coupon>();

            if (!context.Coupons.Any(p => p.CouponId == 1))
            {
                list.Add(new Coupon
                {
                    CouponCode = "10OFF",
                    DiscountAmount = 10
                });
            }

            if (!context.Coupons.Any(p => p.CouponId == 2))
            {
                list.Add(new Coupon
                {
                    CouponCode = "20OFF",
                    DiscountAmount = 20
                });
            }

            if (list.Any())
            {
                context.Coupons.AddRangeAsync(list);
                context.SaveChanges();
            }
        }
    }
}
